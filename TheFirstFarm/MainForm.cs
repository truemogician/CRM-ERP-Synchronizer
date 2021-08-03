using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using FXiaoKe.Models;
using FXiaoKe.Requests;
using Kingdee;
using Kingdee.Forms;
using Kingdee.Requests;
using Kingdee.Requests.Query;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Shared.Exceptions;
using Shared.Extensions;
using TheFirstFarm.Transform;
using TheFirstFarm.Transform.Entities;
using TheFirstFarm.Utilities;
using Client = FXiaoKe.Client;
using FModels = TheFirstFarm.Models.FXiaoKe;
using KModels = TheFirstFarm.Models.Kingdee;

namespace TheFirstFarm {
	using FClient = Client;
	using KClient = Kingdee.Client;
	using FCustomer = FModels.Customer;
	using KCustomer = KModels.Customer;
	using FContact = FModels.Contact;
	using KContact = KModels.Contact;
	using FReturnOrder = FModels.ReturnOrder;
	using KReturnOrder = KModels.ReturnOrder;

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
			var activeSync = Synchronizers.FirstOrDefault(sync => sync.Page.TabIndex == TabControl.SelectedIndex);
			if (activeSync is not null && activeSync.SyncTimer.Enabled && activeSync.LastSyncTime.HasValue) {
				var span = activeSync.LastSyncTime.Value + TimeSpan.FromMilliseconds(activeSync.SyncTimer.Interval) - DateTime.Now;
				activeSync.TimeLeftBox.Text = (span.TotalSeconds >= 0 ? Convert.ToInt32(span.TotalSeconds) : activeSync.SyncTimer.Interval).ToString();
			}
		}

		internal partial class Synchronizer {
			private readonly MainForm _container;

			internal readonly Timer SyncTimer;

			internal readonly Queue<Task> Tasks = new();

			private bool _synchronizing;

			internal Synchronizer(SyncModel model, MainForm container) {
				Model = model;
				_container = container;
				SyncTimer = new Timer {Enabled = false};
				SyncTimer.Tick += (_, _) => AutoSync();
				InitializeComponent();
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
					var _ = Synchronize(StartDatePicker.Value, EndDatePicker.Value)
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

			internal Task Synchronize(DateTime? start, DateTime? end = null) {
				end ??= DateTime.Now;
				return Model switch {
					SyncModel.Customer      => SyncCustomer(start, end.Value),
					SyncModel.SalesOrder    => SyncSalesOrder(start, end.Value),
					SyncModel.DeliveryOrder => SyncDeliveryOrder(start, end.Value),
					SyncModel.ReturnOrder   => SyncReturnOrder(start, end.Value),
					SyncModel.Invoice       => SyncInvoice(start, end.Value),
					SyncModel.Product       => SyncProduct(start, end.Value),
					_                       => throw new EnumValueOutOfRangeException()
				};
			}

			internal async Task SyncCustomer(DateTime? start, DateTime end) {
				var filters = new List<ModelFilter<FCustomer>> {
					ModelFilter<FCustomer>.NotEqual(nameof(FCustomer.NeedSync), true),
					ModelFilter<FCustomer>.Is(nameof(FCustomer.KingdeeId), null),
					ModelFilter<FCustomer>.Equal(nameof(FCustomer.LifeStatus), LifeStatus.Normal),
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
				customers = customers.Where(
						cust => {
							var results = cust.Validate(true);
							if (results.Count == 0)
								return true;
							Warning($"客户{cust.Name}验证失败");
							foreach (var result in results)
								Warning($"{string.Join(", ", result.MemberNames)}: {result.ErrorMessage}");
							return false;
						}
					)
					.ToList();
				foreach (var c in customers) {
					var result = new KCustomer {
						Name = c.Name,
						Number = c.Number,
						CurrencyNumber = c.SettlementCurrency,
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
					var contact = new KContact {
						Name = c.Contact.Name,
						Number = c.Contact.Number,
						PhoneNumber = c.Contact.PhoneNumber,
						Address = c.Contact.Address
					};
					var contactSaveResp = await KClient.SaveAsync(new SaveRequest<KContact>(contact));
					if (!contactSaveResp) {
						Error($"保存联系人{contact.Name}时发生错误：{string.Join(", ", contactSaveResp.ResponseStatus.Errors.Select(e => $"{e.FieldName}: {e.Message}"))}");
						continue;
					}
					result.Contacts = new List<KCustomer.ContactRef> {new() {Number = contactSaveResp.Number}};
					foreach (var addr in c.Addresses)
						result.Addresses.Add(
							new KModels.CustomerAddress {
								Number = addr.Number,
								Location = addr.Address,
								IsShippingAddress = addr.IsShippingAddress,
								ContactWay = addr.ContactWay
							}
						);
					var map = new CustomerMap(c.DataId, c.Number);
					try {
						var _ = KClient.SaveAsync(new SaveRequest<KCustomer>(result))
							.ContinueWith(
								tsk => {
									map.KingdeeId = tsk.Result.Id;
									MapManager.Context.AddOrUpdate(map);
									Information($"客户\"{c.Name}\"同步成功，Id为{tsk.Result.Id}");
								}
							);
					}
					catch (Exception ex) {
						Error($"同步客户\"{c.Name}\"时发生错误，错误信息：{ex.Message}");
					}
					await MapManager.Context.SaveChangesAsync();
				}
			}

			private async Task<List<T>> GetFromKingdee<T>(DateTime? start, DateTime end, Func<T, string> validationFailMessage) where T : ErpModelBase {
				var sentence = (new Field<T>(nameof(ErpModelBase.Status)) == (Literal)Status.Audited) &
					(new Field<T>(nameof(ErpModelBase.AuditionTime)) <= (Literal)end);
				if (start.HasValue)
					sentence &= new Field<T>(nameof(ErpModelBase.AuditionTime)) >= (Literal)start.Value;
				var request = new QueryRequest<T>(new Sentence<T>(sentence));
				var result = await KClient.QueryAsync(request);
				if (result.IsT0) {
					var resp = result.AsT0;
					Log($"{string.Join(", ", resp.ResponseStatus.Errors.Select(e => $"{e.FieldName}: {e.Message}"))}", LogLevel.Error);
					return null;
				}
				return result.AsT1.Where(
						m => {
							var validationResults = m.Validate(true);
							if (validationResults.Count > 0) {
								Warning($"{validationFailMessage(m)}验证失败");
								foreach (var v in validationResults)
									Warning($"{string.Join(", ", v.MemberNames)}: {v.ErrorMessage}");
							}
							return validationResults.Count == 0;
						}
					)
					.ToList();
			}

			internal async Task SyncSalesOrder(DateTime? start, DateTime end) { }

			internal async Task SyncDeliveryOrder(DateTime? start, DateTime end) { }

			internal async Task SyncReturnOrder(DateTime? start, DateTime end) {
				var returnOrders = await GetFromKingdee<KReturnOrder>(start, end, x => $"销售退货单{x.Number}");
				if (returnOrders is null)
					return;
				foreach (var returnOrder in returnOrders) {
					var validationResults = returnOrder.Validate(true);
					if (validationResults.Count > 0) {
						Warning($"销售退货单{returnOrder.Number}验证失败");
						foreach (var v in validationResults)
							Warning($"{string.Join(", ", v.MemberNames)}: {v.ErrorMessage}");
						continue;
					}
					string ownerId = (await MapManager.FromMapProperty<StaffMap>(nameof(StaffMap.Number), (string)returnOrder.SalesmanNumber))?.FXiaoKeId;
					if (string.IsNullOrEmpty(ownerId)) {
						Warning($"CRM中不存在编号为{returnOrder.SalesmanNumber}的员工");
						continue;
					}
					string customerId = (await MapManager.FromMapProperty<CustomerMap>(nameof(CustomerMap.Number), (string)returnOrder.CustomerNumber))?.FXiaoKeId;
					if (string.IsNullOrEmpty(customerId)) {
						Warning($"CRM中不存在编号为{returnOrder.CustomerNumber}的客户，请检查数据或手动同步客户");
						continue;
					}
					var result = new FReturnOrder {
						OwnerId = ownerId,
						CustomerId = customerId,
						Date = returnOrder.Date,
						Reason = returnOrder.ReturnReason,
						Number = returnOrder.Number,
						BusinessType = returnOrder.BusinessType,
						Details = new List<FModels.ReturnOrderDetail>()
					};
					var success = true;
					foreach (var detail in returnOrder.Details) {
						string? productId = (await MapManager.FromMapProperty<ProductMap>(nameof(ProductMap.Number), (string)detail.MaterialNumber))?.FXiaoKeId;
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
					var map = new ReturnOrderMap(returnOrder.Id, returnOrder.Number);
					try {
						var _ = FClient.Create(result, false)
							.ContinueWith(
								tsk => {
									var resp = tsk.Result;
									map.FXiaoKeId = resp.DataId;
									MapManager.Context.AddOrUpdate(map);
									Information($"销售退货单\"{result.Number}\"同步成功，Id为{resp.DataId}");
								}
							);
					}
					catch (Exception ex) {
						Error($"同步销售退货单\"{result.Number}\"时发生错误，错误信息：{ex.Message}");
					}
					await MapManager.Context.SaveChangesAsync();
				}
			}

			internal async Task SyncInvoice(DateTime? start, DateTime end) { }

			internal async Task SyncProduct(DateTime? start, DateTime end) {
				var materials = await GetFromKingdee<KModels.Material>(start, end, x => $"物料{x.Name}");
				if (materials is null)
					return;
				if (materials.Count == 0) {
					Information("没有需要同步的物料");
					return;
				}
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
						MinOrderQuantity = m.MinOrderQuantity
					};
					try {
						var _ = FClient.Create(product, false)
							.ContinueWith(
								tsk => {
									var resp = tsk.Result;
									map.FXiaoKeId = resp.DataId;
									MapManager.Context.AddOrUpdate(map);
									Information($"物料\"{m.Name}\"同步成功，Id为{resp.DataId}");
								}
							);
					}
					catch (Exception ex) {
						Error($"同步物料\"{m.Name}\"时发生错误，错误信息：{ex.Message}");
					}
					await MapManager.Context.SaveChangesAsync();
				}
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

			internal void Log(string message, LogLevel logLevel, bool logTime = true) {
				var color = logLevel switch {
					LogLevel.Trace       => Color.DimGray,
					LogLevel.Debug       => Color.DodgerBlue,
					LogLevel.Information => Color.MediumSpringGreen,
					LogLevel.Warning     => Color.Gold,
					LogLevel.Error       => Color.Red,
					LogLevel.Critical    => Color.Magenta,
					_                    => throw new EnumValueOutOfRangeException()
				};
				if (logTime)
					LogTextBox.AppendText(DateTime.Now.ToString("HH:mm:ss.fff "), Color.Black);
				LogTextBox.AppendLine(message, color);
			}
		}
	}

	public enum LogLevel : byte {
		Trace,

		Debug,

		Information,

		Warning,

		Error,

		Critical
	}
}