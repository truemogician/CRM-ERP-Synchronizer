using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FXiaoKe.Models;
using FXiaoKe.Requests;
using FXiaoKe.Requests.Message;
using Kingdee;
using Kingdee.Forms;
using Kingdee.Requests;
using Kingdee.Requests.Query;
using Kingdee.Responses;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using Shared.Exceptions;
using Shared.Extensions;
using Shared.Serialization;
using TheFirstFarm.Models.Common;
using TheFirstFarm.Transform;
using TheFirstFarm.Transform.Entities;
using TheFirstFarm.Utilities;
using Client = FXiaoKe.Client;
using FModels = TheFirstFarm.Models.FXiaoKe;
using KModels = TheFirstFarm.Models.Kingdee;
using FResponses = FXiaoKe.Responses;
using KResponses = Kingdee.Responses;

namespace TheFirstFarm {
	using FClient = Client;
	using KClient = Kingdee.Client;
	using FCustomer = FModels.Customer;
	using KCustomer = KModels.Customer;
	using KContact = KModels.Contact;
	using FSalesOrder = FModels.SalesOrder;
	using KSalesOrder = KModels.SalesOrder;
	using FDeliveryOrder = FModels.DeliveryOrder;
	using KDeliveryOrder = KModels.OutboundOrder;
	using FReturnOrder = FModels.ReturnOrder;
	using KReturnOrder = KModels.ReturnOrder;
	using FInvoice = FModels.Invoice;
	using KInvoice = KModels.ReceivableBill;
	using FBasicResponse = FResponses.BasicResponse;
	using KBasicResponse = BasicResponse;

	public partial class MainForm : Form {
		public MainForm() {
			var fSection = (NameValueCollection)ConfigurationManager.GetSection("fXiaoKe");
			var kSection = (NameValueCollection)ConfigurationManager.GetSection("kingdee");
			FClient = new FClient(fSection["appId"], fSection["appSecret"], fSection["permanentCode"]) {Operator = new Staff {Id = fSection["operatorId"]}};
			KClient = new KClient(kSection["url"], kSection["databaseId"], kSection["username"], kSection["password"], (Language)Convert.ToInt32(kSection["languageId"]));
			MapManager = new MapManager(FClient, KClient);
			InitializeComponent();
		}

		public FClient FClient { get; }

		public KClient KClient { get; }

		public MapManager MapManager { get; }

		internal void UpdateTimeLeft() {
			var activeSync = Synchronizers.FirstOrDefault(sync => sync.Page == TabControl.SelectedTab);
			if (activeSync is not null && activeSync.SyncTimer.Enabled && activeSync.LastSyncTime.HasValue) {
				var span = activeSync.LastSyncTime.Value + TimeSpan.FromMilliseconds(activeSync.SyncTimer.Interval) - DateTime.Now;
				activeSync.TimeLeftBox.Text = (span.TotalSeconds >= 0 ? Convert.ToInt32(span.TotalSeconds) : activeSync.SyncTimer.Interval).ToString();
			}
		}

		internal partial class Synchronizer {
			private readonly MainForm _container;

			internal readonly Timer SyncTimer;

			internal readonly Queue<Task> Tasks = new();

			internal readonly HashSet<LogLevel> MessageLevels = new();

			private bool _synchronizing;

			internal Synchronizer(SyncModel model, MainForm container) {
				Model = model;
				_container = container;
				SyncTimer = new Timer {Enabled = false};
				SyncTimer.Tick += (_, _) => AutoSync();
				InitializeComponent();
				var _ = LoadStaffs();
			}

			internal FClient FClient => _container.FClient;

			internal KClient KClient => _container.KClient;

			internal MapManager MapManager => _container.MapManager;

			internal bool Synchronizing {
				get => _synchronizing;
				set {
					if (value == _synchronizing)
						return;
					if (!value && Tasks.Count > 0) {
						var task = Tasks.Dequeue();
						task.ContinueWith(_ => Synchronizing = false).Start();
					}
					else {
						_synchronizing = value;
						ManualSyncButton.SafeInvoke(btn => btn.Enabled = !value);
					}
				}
			}

			internal DateTime? LastSyncTime { get; set; }

			internal void ManualSync() {
				if (EndDatePicker.Value <= StartDatePicker.Value)
					MessageBox.Show(@"终止时间应该晚于起始时间", @"数据非法", MessageBoxButtons.OK, MessageBoxIcon.Error);
				else {
					Synchronizing = true;
					LogTextBox.Focus();
					var _ = Synchronize(StartDatePicker.Value.Date, EndDatePicker.Value.Date.AddDays(1))
						.ContinueWith(_ => Synchronizing = false);
				}
			}

			internal void StartSync() {
				SyncTimer.Interval = Convert.ToInt32(SyncIntervalPicker.Value * 60 * 1000);
				AutoSync();
				SyncTimer.Start();
				StartSyncButton.Enabled = false;
				StopSyncButton.Enabled = true;
				LogTextBox.Focus();
			}

			internal void StopSync() {
				SyncTimer.Stop();
				StartSyncButton.Enabled = true;
				StopSyncButton.Enabled = false;
			}

			internal void AutoSync() {
				if (Synchronizing)
					Tasks.Enqueue(
						new Task(
							() => {
								var now = DateTime.Now;
								Synchronize(LastSyncTime, now)
									.ContinueWith(
										_ => {
											LastSyncTime = now;
											Synchronizing = false;
										}
									);
							}
						)
					);
				else {
					Synchronizing = true;
					var now = DateTime.Now;
					var _ = Synchronize(LastSyncTime, now)
						.ContinueWith(
							_ => {
								LastSyncTime = now;
								Synchronizing = false;
							}
						);
				}
			}

			internal async Task Synchronize(DateTime? start, DateTime? end = null) {
				end ??= DateTime.Now;
				Information($"开始同步{(start.HasValue ? $"{start.Value:yyyy-MM-dd HH:mm:ss}到{end.Value:yyyy-MM-dd HH:mm:ss}之间" : $"{end.Value:yyyy-MM-dd HH:mm:ss}之前")}的{Model.GetValue()}数据");
				await (Model switch {
					SyncModel.Customer      => SyncCustomer(start, end.Value),
					SyncModel.SalesOrder    => SyncSalesOrder(start, end.Value),
					SyncModel.DeliveryOrder => SyncDeliveryOrder(start, end.Value),
					SyncModel.ReturnOrder   => SyncReturnOrder(start, end.Value),
					SyncModel.Invoice       => SyncInvoice(start, end.Value),
					SyncModel.Product       => SyncProduct(start, end.Value),
					_                       => throw new EnumValueOutOfRangeException()
				});
				NewLine();
			}

			private async Task LoadStaffs() {
				var apartment = await FClient.GetDepartmentDetailTree();
				var staffs = await FClient.GetStaffs(apartment);
				MapManager.Context.StaffMaps.AddOrUpdateRange(staffs.Select(staff => new StaffMap(staff.Id, staff.Number)));
				await MapManager.Context.SaveChangesAsync();
			}

			private void LogValidationResults(string title, IList<ValidationResult> results) {
				var builder = new StringBuilder();
				builder.AppendLine(title);
				for (var i = 0; i < results.Count; ++i) {
					var content = $"{string.Join(", ", results[i].MemberNames)}: {results[i].ErrorMessage}";
					if (i == results.Count - 1)
						builder.Append(content);
					else
						builder.AppendLine(content);
				}
				Warning(builder.ToString());
			}

			private async Task<List<T>> GetFromKingdee<T>(DateTime? start, DateTime end, Func<T, string> getEntityName) where T : AuditableErpModel {
				var sentence = (new Field<T>(nameof(AuditableErpModel.LifeStatus)) == (Literal)Kingdee.Forms.LifeStatus.Audited) &
					(new Field<T>(nameof(AuditableErpModel.AuditionTime)) <= (Literal)end);
				if (start.HasValue)
					sentence &= new Field<T>(nameof(AuditableErpModel.AuditionTime)) >= (Literal)start.Value;
				var request = new QueryRequest<T>(new Sentence<T>(sentence));
				var result = await KClient.QueryAsync(request);
				if (result.IsT0) {
					var resp = result.AsT0;
					Error($"{resp.ResponseStatus}");
					return null;
				}
				string modelName = Model.GetValue();
				Information(result.AsT1.Count == 0 ? $"没有需要同步的{modelName}" : $"获取到{result.AsT1.Count}条{modelName}数据，开始同步");
				var validated = result.AsT1.Where(
						m => {
							var validationResults = m.Validate(true);
							if (validationResults.Count > 0)
								LogValidationResults($"{modelName}{getEntityName(m)}验证失败：", validationResults);
							return validationResults.Count == 0;
						}
					)
					.ToList();
				return validated;
			}

			internal async Task SyncCustomer(DateTime? start, DateTime end) {
				var filters = new List<ModelFilter<FCustomer>> {
					ModelFilter<FCustomer>.Equal(nameof(FCustomer.NeedSync), true),
					ModelFilter<FCustomer>.Is(nameof(FCustomer.KingdeeId), null),
					ModelFilter<FCustomer>.Equal(nameof(FCustomer.LifeStatus), FXiaoKe.Models.LifeStatus.Normal),
					ModelFilter<FCustomer>.LessEqual(nameof(FCustomer.CreationTime), end)
				};
				if (start.HasValue)
					filters.Add(ModelFilter<FCustomer>.GreaterEqual(nameof(FCustomer.LastModifiedTime), start.Value));
				List<FCustomer> customers;
				try {
					customers = await _container.FClient.QueryByCondition<FCustomer>(filters);
				}
				catch (Exception ex) {
					Log($"从纷享销客获取客户时发生错误：{ex.Message}", LogLevel.Error);
					return;
				}
				if (customers.Count == 0) {
					Information("没有需要同步的客户");
					return;
				}
				int totalCount = customers.Count;
				var successCount = 0;
				customers = customers.Where(
						cust => {
							var results = cust.Validate(true);
							if (results.Count == 0)
								return true;
							LogValidationResults($"客户{cust.Name}验证失败：", results);
							return false;
						}
					)
					.ToList();
				foreach (var c in customers) {
					var cust = new KCustomer {
						Name = c.Name,
						Number = c.Number,
						CurrencyNumber = c.Currency,
						CreatorOrgNumber = c.CreatorOrgId,
						UserOrgNumber = c.UserOrgId,
						InvoiceTitle = c.InvoiceTitle,
						TaxpayerId = c.TaxpayerId,
						OpeningBank = c.OpeningBank,
						BankAccount = c.BankAccount,
						BillingAddress = c.BillingAddress,
						PhoneNumber = c.InvoicePhoneNumber,
						Addresses = new List<KModels.CustomerAddress>()
					};
					if (cust.CurrencyNumber?.Number is null)
						cust.CurrencyNumber = Currency.CNY;
					var contact = new KContact {
						Name = c.Contact.Name,
						Number = c.Contact.Number,
						PhoneNumber = c.Contact.PhoneNumber,
						Address = c.Contact.Address
					};
					var hasContact = false;
					try {
						var mp = await MapManager.FromMapProperty<ContactMap>(nameof(ContactMap.Number), c.Contact.Number);
						if (mp?.KingdeeId is not null) {
							hasContact = true;
							contact.Id = mp.KingdeeId.Value;
						}
					}
					catch (Exception ex) {
						Error($"查询联系人{contact.Name}时发生错误：{ex.Message}");
						continue;
					}
					if (!hasContact)
						try {
							var contactSaveResp = await KClient.SaveAsync(new SaveRequest<KContact>(contact));
							if (!contactSaveResp)
								throw new Exception(contactSaveResp.ResponseStatus.ToString());
							contact.Id = contactSaveResp.Id!.Value;
							MapManager.Context.ContactMaps.AddOrUpdate(new ContactMap(c.Contact.DataId, contact.Number, contact.Id));
						}
						catch (Exception ex) {
							Error($"保存联系人{contact.Name}时发生错误：{ex.Message}");
							continue;
						}
					foreach (var addr in c.Addresses)
						cust.Addresses.Add(
							new KModels.CustomerAddress {
								Number = addr.Number,
								Location = addr.Address,
								IsShippingAddress = addr.IsShippingAddress,
								ContactWay = addr.ContactWay,
								ContactNumber = contact.Number
							}
						);
					if (c.Addresses.Count == 0)
						Warning($"客户{cust.Name}缺少地址，无法保存联系人");
					var map = new CustomerMap(c.DataId, c.Number);
					SaveResponse saveResp;
					try {
						saveResp = await KClient.SaveAsync(new SaveRequest<KCustomer>(cust));
						if (!saveResp)
							throw new Exception(saveResp.ResponseStatus.ToString());
						map.KingdeeId = saveResp.Id;
						MapManager.Context.CustomerMaps.AddOrUpdate(map);
						Debug($"客户\"{c.Name}\"同步成功，Id为{saveResp.Id}");
						cust.Id = saveResp.Id.Value;
					}
					catch (Exception ex) {
						Error($"同步客户\"{c.Name}\"时发生错误，错误信息：{ex.Message}");
						if (!hasContact)
							try {
								var deleteResp = await KClient.UnauditAndDeleteAsync(new DeleteRequest<KContact>(contact));
								if (deleteResp)
									Debug($"成功删除同步失败客户对应的联系人{contact.Name}");
								else
									throw new Exception(deleteResp.ResponseStatus.ToString());
							}
							catch (Exception exception) {
								Critical($"删除同步失败客户对应的联系人{contact.Name}失败：{exception.Message}");
							}
						continue;
					}
					var updater = new Updater<FCustomer>(c);
					updater.Update(nameof(FCustomer.NeedSync), false);
					updater.Update(nameof(FCustomer.SyncSuccess), true);
					updater.Update(nameof(FCustomer.KingdeeId), saveResp.Id);
					updater.Update(nameof(FCustomer.SyncResult), JsonConvert.SerializeObject(saveResp));
					try {
						var updationResp = await FClient.Update(updater);
						if (updationResp) {
							Debug($"反写CRM成功，客户{c.Name}同步结束");
							++successCount;
							continue;
						}
						Error($"同步结果反写失败：{updationResp.ErrorMessage}");
					}
					catch (Exception ex) {
						Error($"同步结果反写CRM发生错误：{ex.Message}");
					}
					try {
						KBasicResponse deleteResp;
						if (!hasContact) {
							deleteResp = await KClient.UnauditAndDeleteAsync(new DeleteRequest<KContact>(contact));
							if (!deleteResp)
								throw new Exception(deleteResp.ResponseStatus.ToString());
						}
						deleteResp = await KClient.UnauditAndDeleteAsync(new DeleteRequest<KCustomer>(cust));
						if (!deleteResp)
							throw new Exception(deleteResp.ResponseStatus.ToString());
					}
					catch (Exception ex) {
						Critical($"删除已同步客户及其联系人时发生错误：{ex.Message}");
					}
				}
				await MapManager.Context.SaveChangesAsync();
				Information($"同步完成，总计获取{totalCount}条客户数据，成功{successCount}条");
			}

			internal async Task SyncSalesOrder(DateTime? start, DateTime end) {
				var orders = await GetFromKingdee<KSalesOrder>(start, end, x => x.Number);
				if (orders is null)
					return;
				var successCount = 0;
				foreach (var order in orders) {
					string ownerId = MapManager.GetByMapProperty<StaffMap>(nameof(StaffMap.Number), (string)order.SalesmanNumber)?.FXiaoKeId;
					if (string.IsNullOrEmpty(ownerId)) {
						Warning($"CRM中不存在编号为{order.SalesmanNumber}的员工");
						continue;
					}
					string customerId = (await MapManager.FromMapProperty<CustomerMap>(nameof(CustomerMap.Number), (string)order.CustomerNumber))?.FXiaoKeId;
					if (string.IsNullOrEmpty(customerId)) {
						Warning($"CRM中不存在编号为{order.CustomerNumber}的客户，请检查数据或手动同步客户");
						continue;
					}
					var result = new FSalesOrder {
						OwnerId = ownerId,
						CustomerId = customerId,
						CreationTime = order.CreationTime,
						Date = order.Date,
						KingdeeNumber = order.Number,
						OrderType = order.BillType,
						BusinessType = order.BusinessType,
						ShippingAddress = order.DeliveryAddressNumber,
						Products = new List<FModels.SalesOrderProduct>()
					};
					var success = true;
					foreach (var material in order.Materials) {
						string productId = (await MapManager.FromMapProperty<ProductMap>(nameof(ProductMap.Number), (string)material.MaterialNumber))?.FXiaoKeId;
						if (string.IsNullOrEmpty(productId)) {
							Warning($"CRM中不存在编号为{material.MaterialNumber}的产品，请检查数据或手动同步产品");
							success = false;
							break;
						}
						result.Products.Add(
							new FModels.SalesOrderProduct {
								ProductId = productId,
								OwnerId = ownerId,
								CreationTime = order.CreationTime,
								Quantity = material.Quantity,
								Price = material.UnitPrice,
								TaxRate = material.TaxRate,
								Money = material.Money,
								RequestDate = material.RequestDate,
								Remark = material.Remark
							}
						);
					}
					if (!success)
						continue;
					var map = new SalesOrderMap(order.Id, order.Number);
					try {
						var creationResp = await FClient.Create(result);
						map.FXiaoKeId = creationResp.DataId;
						MapManager.Context.SalesOrderMaps.AddOrUpdate(map);
						Debug($"销售订单\"{result.KingdeeNumber}\"同步成功，Id为{creationResp.DataId}");
						++successCount;
					}
					catch (Exception ex) {
						Error($"同步销售订单\"{result.KingdeeNumber}\"时发生错误，错误信息：{ex.Message}");
					}
				}
				await MapManager.Context.SaveChangesAsync();
				Information($"同步完成，总计获取{orders.Count}条销售订单数据，成功{successCount}条");
			}

			internal async Task SyncDeliveryOrder(DateTime? start, DateTime end) {
				var orders = await GetFromKingdee<KDeliveryOrder>(start, end, x => x.Number);
				if (orders is null)
					return;
				var successCount = 0;
				foreach (var order in orders) {
					string ownerId = MapManager.GetByMapProperty<StaffMap>(nameof(StaffMap.Number), (string)order.SalesmanNumber)?.FXiaoKeId;
					if (string.IsNullOrEmpty(ownerId)) {
						Warning($"CRM中不存在编号为{order.SalesmanNumber}的员工");
						continue;
					}
					string customerId = (await MapManager.FromMapProperty<CustomerMap>(nameof(CustomerMap.Number), (string)order.CustomerNumber))?.FXiaoKeId;
					if (string.IsNullOrEmpty(customerId)) {
						Warning($"CRM中不存在编号为{order.CustomerNumber}的客户，请检查数据或手动同步客户");
						continue;
					}
					string salesOrderId = (await MapManager.FromMapProperty<SalesOrderMap>(nameof(SalesOrderMap.Number), order.SalesOrderNumber))?.FXiaoKeId;
					if (string.IsNullOrEmpty(customerId)) {
						Warning($"CRM中不存在编号为{order.SalesOrderNumber}的销售订单，请检查数据或手动同步销售订单");
						continue;
					}
					var result = new FDeliveryOrder {
						OwnerId = ownerId,
						CustomerId = customerId,
						SalesOrderId = salesOrderId,
						CreationTime = order.CreationTime,
						Date = order.Date,
						Number = order.Number,
						KingdeeNumber = order.Number,
						OrderType = order.BillType,
						Logistics = new List<FModels.LogisticsInfo>(),
						Details = new List<FModels.DeliveryOrderDetail>()
					};
					var success = true;
					foreach (var detail in order.Details) {
						string productId = (await MapManager.FromMapProperty<ProductMap>(nameof(ProductMap.Number), (string)detail.MaterialNumber))?.FXiaoKeId;
						if (string.IsNullOrEmpty(productId)) {
							Warning($"CRM中不存在编号为{detail.MaterialNumber}的产品，请检查数据或手动同步产品");
							success = false;
							break;
						}
						result.Details.Add(
							new FModels.DeliveryOrderDetail {
								ProductId = productId,
								OwnerId = ownerId,
								CreationTime = order.CreationTime,
								KingdeeId = order.Id,
								ExpectedAmount = detail.ExpectedAmount,
								ActualAmount = detail.ActualAmount
							}
						);
					}
					if (!success)
						continue;
					//foreach (var logistics in order.Logistics)
					//	result.Logistics.Add(
					//		new FModels.LogisticsInfo {
					//			CreationTime = order.CreationTime,
					//			DeliveryTime = logistics.DeliveryTime,
					//			Company = logistics.CompanyCode,
					//			WaybillNumber = logistics.WaybillNumber,
					//			Status = logistics.Status
					//		}
					//	);
					var map = new DeliveryOrderMap(order.Id, order.Number);
					try {
						var creationResp = await FClient.Create(result);
						map.FXiaoKeId = creationResp.DataId;
						MapManager.Context.DeliveryOrderMaps.AddOrUpdate(map);
						Debug($"销售出库单\"{result.Number}\"同步成功，Id为{creationResp.DataId}");
						++successCount;
					}
					catch (Exception ex) {
						Error($"同步销售出库单\"{result.Number}\"时发生错误，错误信息：{ex.Message}");
					}
				}
				await MapManager.Context.SaveChangesAsync();
				Information($"同步完成，总计获取{orders.Count}条销售出库单数据，成功{successCount}条");
			}

			internal async Task SyncReturnOrder(DateTime? start, DateTime end) {
				var orders = await GetFromKingdee<KReturnOrder>(start, end, x => x.Number);
				if (orders is null)
					return;
				var successCount = 0;
				foreach (var order in orders) {
					string ownerId = MapManager.GetByMapProperty<StaffMap>(nameof(StaffMap.Number), (string)order.SalesmanNumber)?.FXiaoKeId;
					if (string.IsNullOrEmpty(ownerId)) {
						Warning($"CRM中不存在编号为{order.SalesmanNumber}的员工");
						continue;
					}
					string customerId = (await MapManager.FromMapProperty<CustomerMap>(nameof(CustomerMap.Number), (string)order.CustomerNumber))?.FXiaoKeId;
					if (string.IsNullOrEmpty(customerId)) {
						Warning($"CRM中不存在编号为{order.CustomerNumber}的产品，请检查数据或手动同步产品");
						continue;
					}
					var result = new FReturnOrder {
						OwnerId = ownerId,
						CustomerId = customerId,
						Date = order.Date,
						Reason = order.ReturnReason,
						Number = order.Number,
						BusinessType = order.BusinessType,
						Details = new List<FModels.ReturnOrderDetail>()
					};
					var success = true;
					foreach (var detail in order.Details) {
						string productId = (await MapManager.FromMapProperty<ProductMap>(nameof(ProductMap.Number), (string)detail.MaterialNumber))?.FXiaoKeId;
						if (string.IsNullOrEmpty(productId)) {
							Warning($"CRM中不存在编号为{detail.MaterialNumber}的客户，请检查数据或手动同步客户");
							success = false;
							break;
						}
						result.Details.Add(
							new FModels.ReturnOrderDetail {
								ProductId = productId,
								OwnerId = ownerId,
								ReturnAmount = detail.ReturnAmount,
								UnitPrice = detail.UnitPrice,
								TaxRate = detail.TaxRate,
								Volume = detail.Money,
								ReturnType = detail.ReturnType
							}
						);
					}
					if (!success)
						continue;
					var map = new ReturnOrderMap(order.Id, order.Number);
					try {
						var creationResp = await FClient.Create(result, false);
						map.FXiaoKeId = creationResp.DataId;
						MapManager.Context.ReturnOrderMaps.AddOrUpdate(map);
						Debug($"销售订单\"{result.Number}\"同步成功，Id为{creationResp.DataId}");
						++successCount;
					}
					catch (Exception ex) {
						Error($"同步销售订单\"{result.Number}\"时发生错误，错误信息：{ex.Message}");
					}
				}
				await MapManager.Context.SaveChangesAsync();
				Information($"同步完成，总计获取{orders.Count}条销售订单数据，成功{successCount}条");
			}

			internal async Task SyncInvoice(DateTime? start, DateTime end) {
				var invoices = await GetFromKingdee<KInvoice>(start, end, x => x.Number);
				if (invoices is null)
					return;
				var successCount = 0;
				foreach (var invoice in invoices) {
					string ownerId = MapManager.GetByMapProperty<StaffMap>(nameof(StaffMap.Number), (string)invoice.SalesmanNumber)?.FXiaoKeId;
					if (string.IsNullOrEmpty(ownerId)) {
						Warning($"CRM中不存在编号为{invoice.SalesmanNumber}的员工");
						continue;
					}
					string customerId = (await MapManager.FromMapProperty<CustomerMap>(nameof(CustomerMap.Number), (string)invoice.CustomerNumber))?.FXiaoKeId;
					if (string.IsNullOrEmpty(customerId)) {
						Warning($"CRM中不存在编号为{invoice.CustomerNumber}的客户，请检查数据或手动同步客户");
						continue;
					}
					var set = invoice.Details.Select(x => x.SalesOrderNumber).Where(x => !string.IsNullOrEmpty(x)).ToHashSet();
					switch (set.Count) {
						case 0:
							Warning($"应收单{invoice.Number}缺少销售订单关联，请检查数据");
							continue;
						case > 1:
							Warning($"应收单{invoice.Number}关联了多个不相同的销售订单，请检查数据");
							continue;
					}
					string salesOrderNumber = set.First();
					string salesOrderId = (await MapManager.FromMapProperty<SalesOrderMap>(nameof(SalesOrderMap.Number), salesOrderNumber))?.FXiaoKeId;
					if (string.IsNullOrEmpty(customerId)) {
						Warning($"CRM中不存在编号为{salesOrderNumber}的销售订单，请检查数据或手动同步销售订单");
						continue;
					}
					var result = new FInvoice {
						OwnerId = ownerId,
						CustomerId = customerId,
						SalesOrderId = salesOrderId,
						CreationTime = invoice.CreationTime,
						Number = invoice.Number,
						Money = invoice.ExpectedMoney,
						BillType = invoice.BillTypeNumber
					};
					var map = new InvoiceMap(invoice.Id, invoice.Number);
					try {
						var creationResp = await FClient.Create(result);
						map.FXiaoKeId = creationResp.DataId;
						MapManager.Context.InvoiceMaps.AddOrUpdate(map);
						Debug($"应收单\"{result.Number}\"同步成功，Id为{creationResp.DataId}");
						++successCount;
					}
					catch (Exception ex) {
						Error($"同步应收单\"{result.Number}\"时发生错误，错误信息：{ex.Message}");
					}
				}
				await MapManager.Context.SaveChangesAsync();
				Information($"同步完成，总计获取{invoices.Count}条应收单数据，成功{successCount}条");
			}

			internal async Task SyncProduct(DateTime? start, DateTime end) {
				var materials = await GetFromKingdee<KModels.Material>(start, end, x => x.Name);
				if (materials is null)
					return;
				if (materials.Count == 0) {
					Information("没有需要同步的物料");
					return;
				}
				var successCount = 0;
				foreach (var m in materials) {
					var map = new ProductMap(m.Id, m.Number);
					var product = new FModels.Product {
						AllowReturn = m.AllowReturn,
						BarCode = m.BarCode,
						Category = m.Group,
						Height = m.Height,
						Width = m.Width,
						Length = m.Length,
						Number = m.Number,
						Name = m.Name,
						Specification = m.Specification,
						ProductProperty = m.MaterialProperty,
						MeasurementUnit = m.Unit,
						ShelfLifeUnit = m.ShelfLifeUnit,
						ShelfLife = m.ShelfLife,
						MinOrderQuantity = m.MinOrderQuantity,
						CreationTime = m.CreationTime
					};
					try {
						var creationResp = await FClient.Create(product, false);
						map.FXiaoKeId = creationResp.DataId;
						MapManager.Context.AddOrUpdate(map);
						Debug($"物料\"{m.Name}\"同步成功，Id为{creationResp.DataId}");
						++successCount;
					}
					catch (Exception ex) {
						Error($"同步物料\"{m.Name}\"时发生错误，错误信息：{ex.Message}");
					}
				}
				await MapManager.Context.SaveChangesAsync();
				Information($"同步完成，总计获取{materials.Count}条物料数据，成功{successCount}条");
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal void Trace(string message, bool logTime = true) => Log(message, LogLevel.Trace, logTime);

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal void Debug(string message, bool logTime = true) => Log(message, LogLevel.Debug, logTime);

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal void Information(string message, bool logTime = true) => Log(message, LogLevel.Information, logTime);

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal void Warning(string message, bool logTime = true) => Log(message, LogLevel.Warning, logTime);

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal void Error(string message, bool logTime = true) => Log(message, LogLevel.Error, logTime);

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal void Critical(string message, bool logTime = true) => Log(message, LogLevel.Critical, logTime);

			internal void Log(string message, LogLevel logLevel, bool showTime = true, bool showLogLevel = true) {
				var color = logLevel switch {
					LogLevel.Trace       => Color.DimGray,
					LogLevel.Debug       => Color.DodgerBlue,
					LogLevel.Information => Color.MediumSpringGreen,
					LogLevel.Warning     => Color.Gold,
					LogLevel.Error       => Color.Red,
					LogLevel.Critical    => Color.Magenta,
					_                    => throw new EnumValueOutOfRangeException()
				};
				var now = DateTime.Now;
				if (showTime)
					LogTextBox.AppendText(now.ToString("HH:mm:ss.fff "), Color.Black);
				LogTextBox.AppendLine($"{(showLogLevel ? $"[{logLevel.GetValue()}] " : "")}{message}", color);
				if (MessageLevels.Contains(logLevel)) {
					var _ = FClient.SendCompositeMessage(
						new CompositeMessage("数据同步出现问题", "http://120.27.55.22/k3cloud/html5/index.aspx") {
							Form = {
								("级别", logLevel.GetValue()),
								("日期", now.ToString("yyyy年MM月dd日")),
								("时间", now.ToString("HH时mm分ss秒"))
							},
							Tail = message
						}
					);
				}
			}

			internal void NewLine() => LogTextBox.AppendLine("");
		}
	}

	public enum LogLevel : byte {
		[EnumValue("痕迹")]
		Trace,

		[EnumValue("调试")]
		Debug,

		[EnumValue("信息")]
		Information,

		[EnumValue("警告")]
		Warning,

		[EnumValue("错误")]
		Error,

		[EnumValue("严重错误")]
		Critical
	}
}