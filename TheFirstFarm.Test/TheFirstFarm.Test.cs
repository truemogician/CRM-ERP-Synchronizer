// ReSharper disable StringLiteralTypo
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FXiaoKe;
using FXiaoKe.Models;
using FXiaoKe.Requests.Message;
using FXiaoKe.Utilities;
using Kingdee.Forms;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NUnit.Framework;
using TheFirstFarm.Transform;
using TheFirstFarm.Transform.Entities;
using FModels = TheFirstFarm.Models.FXiaoKe;
using KModels = TheFirstFarm.Models.Kingdee;
using FRequests = FXiaoKe.Requests;
using KRequests = Kingdee.Requests;
using FResponses = FXiaoKe.Responses;
using KResponses = Kingdee.Responses;

namespace TheFirstFarm.Test {
	public class TheFirstFarmTests : TestBase {
		[TestCaseGeneric(GenericArgument = typeof(KModels.ReturnOrder), ExpectedResult = 15)]
		public int KingdeeFieldsTest<T>() where T : ErpModelBase {
			var fields = FormMeta<T>.QueryFields;
			Console.WriteLine(string.Join(", ", fields.Select(field => field.ToString("json"))));
			return fields.Count;
		}

		[TestCaseGeneric(GenericArgument = typeof(KModels.Customer))]
		[TestCaseGeneric(GenericArgument = typeof(KModels.Material))]
		[TestCaseGeneric(GenericArgument = typeof(KModels.SalesOrder))]
		[TestCaseGeneric(GenericArgument = typeof(KModels.OutboundOrder))]
		[TestCaseGeneric(GenericArgument = typeof(KModels.ReceivableBill))]
		[TestCaseGeneric(GenericArgument = typeof(KModels.ReturnOrder))]
		public void KingdeeQueryTest<T>() where T : ErpModelBase {
			var response = KClient.Query(new KRequests.QueryRequest<T>());
			Assert.IsTrue(response.IsT1);
			var result = response.AsT1;
			Console.WriteLine($@"{result?.Count} {typeof(T).Name}s found");
		}

		[TestCaseGeneric(GenericArgument = typeof(FModels.Customer))]
		[TestCaseGeneric(GenericArgument = typeof(FModels.Product))]
		[TestCaseGeneric(GenericArgument = typeof(FModels.SalesOrder))]
		[TestCaseGeneric(GenericArgument = typeof(FModels.DeliveryOrder))]
		[TestCaseGeneric(GenericArgument = typeof(FModels.Invoice))]
		[TestCaseGeneric(GenericArgument = typeof(FModels.ReturnOrder))]
		public async Task FXiaoKeQueryTest<T>(params FRequests.ModelFilter<T>[] filters) where T : CrmModelBase {
			FClient.Operator = await FClient.GetStaffByPhoneNumber("18118359138");
			var result = await FClient.QueryByCondition(filters);
			Console.WriteLine($@"{result?.Count} {typeof(T).Name}s found");
		}

		[TestCaseGeneric(132349, GenericArgument = typeof(KModels.Customer))]
		public async Task KingdeeUnauditAndDeleteTest<T>(params int[] ids) where T : ErpModelBase {
			var resp = await KClient.UnauditAndDeleteAsync(new KRequests.DeleteRequest<T>(ids));
			Console.WriteLine(JsonConvert.SerializeObject(resp));
			Assert.IsTrue(resp);
		}

		[Test]
		public async Task CustomerMapTest() {
			FClient.Operator = await FClient.GetStaffByPhoneNumber("18118359138");
			var customers = await FClient.GetAll<FModels.Customer>();
			await using var dbContext = new MapContext();
			foreach (var customer in customers)
				dbContext.CustomerMaps.AddOrUpdate(new CustomerMap(customer.DataId, customer.Number, customer.KingdeeId));
			Console.WriteLine($@"{await dbContext.SaveChangesAsync()} changes saved");
		}

		[Test]
		public async Task StaffMapTest() {
			var department = await FClient.GetDepartmentDetailTree();
			var staffs = await FClient.GetStaffs(department);
			await using var dbContext = new MapContext();
			dbContext.StaffMaps.AddOrUpdateRange(staffs.Select(staff => new StaffMap(staff.Id, staff.Number)));
			Console.WriteLine($@"{await dbContext.SaveChangesAsync()} changes saved");
		}

		[Test]
		public async Task ReturnOrderSaveTest() {
			FClient.RequestFailed += (sender, args) => {
				var reqType = args.Request.GetType();
				if (args.Request is MessageRequest) {
					args.Continue = true;
					return;
				}
				if (args.Response is FResponses.BasicResponse resp) {
					var attr = args.Request.Attribute;
					var composite = new CompositeMessage(attr.ErrorMessage ?? "发生未知错误", Client.Origin) {
						Head = resp.ErrorMessage,
						Form = new List<LabelAndValue> {
							("时间", DateTime.Now.ToString("yyyy-MM-dd H:mm:ss.fff")),
							("路径", attr.Url.PathAndQuery)
						}
					};
					if (reqType.IsAssignableToGeneric(typeof(FRequests.CreationRequestBase<>))) {
						var modelType = ((dynamic)args.Request).Data.Model.GetType() as Type;
						composite.Form.Add(("对象", modelType.GetModelName()));
					}
					Console.WriteLine(composite.ToString());
					var _ = FClient.SendCompositeMessage(composite);
				}
				args.Continue = args.Response.ResponseMessage.IsSuccessStatusCode;
			};
			var kingdeeModels = (await KClient.QueryAsync(new KRequests.QueryRequest<KModels.ReturnOrder>())).AsT1;
			var department = await FClient.GetDepartmentDetailTree();
			var staffs = await FClient.GetStaffs(department);
			FClient.Operator = staffs.Single(staff => staff.Name == "周孝成");
			var dbContext = new MapContext();
			dbContext.StaffMaps.UpdateRange(staffs.Select(staff => new StaffMap(staff.Id, staff.Number)));
			await dbContext.SaveChangesAsync();
			var transformer = new MapManager(FClient, KClient, dbContext);
			var fXiaoKeModels = await Task.WhenAll(
				kingdeeModels.Select(
					async model => {
						string ownerId = transformer.GetByMapProperty<StaffMap, string>(nameof(StaffMap.Number), model.SalesmanNumber)?.FXiaoKeId;
						return new FModels.ReturnOrder {
							CustomerId = (await transformer.FromMapProperty<CustomerMap>(nameof(CustomerMap.Number), (string)model.CustomerNumber))?.FXiaoKeId,
							Date = model.Date,
							OwnerId = ownerId,
							Reason = model.ReturnReason,
							Number = model.Number,
							BusinessType = model.BusinessType,
							Details = (await Task.WhenAll(
									model.Details.Select(
										async detail => new FModels.ReturnOrderDetail {
											ProductId = (await transformer.FromMapProperty<ProductMap>(nameof(ProductMap.Number), (string)detail.MaterialNumber))?.FXiaoKeId,
											ReturnAmount = detail.ReturnAmount,
											UnitPrice = detail.UnitPrice,
											TaxRate = detail.TaxRate,
											Volume = detail.Money,
											ReturnType = detail.ReturnType,
											OwnerId = ownerId
										}
									)
								))
								.AsList()
						};
					}
				)
			);
			foreach (var model in fXiaoKeModels) {
				bool useDefaultOwner = model.OwnerId is null;
				if (useDefaultOwner)
					model.OwnerId = FClient.Operator.Id;
				if (!await transformer.HasMapProperty<ReturnOrderMap>(nameof(ReturnOrderMap.Number), model.Number)) {
					if (useDefaultOwner) {
						var _ = FClient.SendTextMessage($"销售退货单缺少负责人，将使用默认负责人{FClient.Operator.Name}");
					}
					await FClient.Create(model, true);
				}
			}
			await dbContext.SaveChangesAsync();
			await dbContext.DisposeAsync();
		}
	}
}