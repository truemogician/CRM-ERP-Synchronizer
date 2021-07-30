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
	public class ReturnOrderTests : TestBase {
		[TestCaseGeneric(GenericArgument = typeof(KModels.ReturnOrder), ExpectedResult = 18)]
		public int KingdeeFieldsTest<T>() where T : FormBase {
			var fields = FormMeta<T>.QueryFields;
			Console.WriteLine(string.Join(", ", fields.Select(field => field.ToString("json"))));
			return fields.Count;
		}

		[TestCaseGeneric(GenericArgument = typeof(KModels.ReturnOrder))]
		public void KingdeeQueryTest<T>() where T : FormBase {
			var response = KingdeeClient.Query(new KRequests.QueryRequest<T>());
			Assert.IsTrue(response.IsT1);
			foreach (var resp in response.AsT1)
				Console.WriteLine(JsonConvert.SerializeObject(resp, Formatting.Indented));
		}

		[TestCaseGeneric(GenericArgument = typeof(FModels.ReturnOrder))]
		[TestCaseGeneric(GenericArgument = typeof(FModels.Customer))]
		[TestCaseGeneric(GenericArgument = typeof(FModels.Product))]
		public async Task FXiaoKeQueryTest<T>(params FRequests.ModelFilter<T>[] filters) where T : CrmModelBase {
			FXiaoKeClient.Operator = await FXiaoKeClient.GetStaffByPhoneNumber("18118359138");
			var result = await FXiaoKeClient.QueryByCondition(filters);
			Console.WriteLine($@"{result?.Count} {typeof(T).Name}s found");
		}

		[Test]
		public async Task CustomerMapTest() {
			FXiaoKeClient.Operator = await FXiaoKeClient.GetStaffByPhoneNumber("18118359138");
			var customers = await FXiaoKeClient.GetAll<FModels.Customer>();
			await using var dbContext = new MapContext();
			foreach (var customer in customers)
				dbContext.CustomerMaps.Update(new CustomerMap(customer.DataId, customer.Number, customer.KingdeeId));
			Console.WriteLine($@"{await dbContext.SaveChangesAsync()} changes saved");
		}

		[Test]
		public async Task StaffMapTest() {
			var department = await FXiaoKeClient.GetDepartmentDetailTree();
			var staffs = await FXiaoKeClient.GetStaffs(department);
			await using var dbContext = new MapContext();
			dbContext.StaffMaps.AddOrUpdateRange(staffs.Select(staff => new StaffMap(staff.Id, staff.Number)));
			Console.WriteLine($@"{await dbContext.SaveChangesAsync()} changes saved");
		}

		[Test]
		public async Task ReturnOrderSaveTest() {
			FXiaoKeClient.RequestFailed += (sender, args) => {
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
					var _ = FXiaoKeClient.SendCompositeMessage(composite);
				}
				args.Continue = args.Response.ResponseMessage.IsSuccessStatusCode;
			};
			var kingdeeModels = KingdeeClient.Query(new KRequests.QueryRequest<KModels.ReturnOrder>()).AsT1;
			var department = await FXiaoKeClient.GetDepartmentDetailTree();
			var staffs = await FXiaoKeClient.GetStaffs(department);
			FXiaoKeClient.Operator = staffs.Single(staff => staff.Name == "周孝成");
			var dbContext = new MapContext();
			dbContext.StaffMaps.UpdateRange(staffs.Select(staff => new StaffMap(staff.Id, staff.Number)));
			await dbContext.SaveChangesAsync();
			var transformer = new MapManager(FXiaoKeClient, KingdeeClient, dbContext);
			var fXiaoKeModels = await Task.WhenAll(
				kingdeeModels.Select(
					async model => {
						string ownerId = transformer.GetByMapProperty<StaffMap>(nameof(StaffMap.Number), model.SalesmanNumber.Number).FXiaoKeId;
						return new FModels.ReturnOrder {
							CustomerId = (await transformer.FromMapPropertyF<CustomerMap>(nameof(CustomerMap.Number), model.CustomerNumber)).FXiaoKeId,
							Date = model.Date!.Value,
							OwnerId = ownerId,
							Reason = model.ReturnReason,
							Number = model.Number,
							BusinessType = model.BusinessType,
							Details = (await Task.WhenAll(
									model.Details.Select(
										async detail => new FModels.ReturnOrderDetail {
											ProductId = (await transformer.FromMapPropertyK<ProductMap>(nameof(ProductMap.Number), detail.MaterialNumber.Number)).FXiaoKeId,
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
					model.OwnerId = FXiaoKeClient.Operator.Id;
				if (!await transformer.HasFXiaoKeId<ReturnOrderMap>(model.DataId)) {
					if (useDefaultOwner) {
						var _ = FXiaoKeClient.SendTextMessage($"销售退货单缺少负责人，将使用默认负责人{FXiaoKeClient.Operator.Name}");
					}
					await FXiaoKeClient.Create(model, true);
				}
			}
			await dbContext.SaveChangesAsync();
			await dbContext.DisposeAsync();
		}
	}
}