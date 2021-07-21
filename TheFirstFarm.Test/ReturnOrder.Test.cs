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
using Newtonsoft.Json;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Builders;
using Shared.Serialization;
using TheFirstFarm.Utilities;
using FModels = TheFirstFarm.Models.FXiaoKe;
using KModels = TheFirstFarm.Models.Kingdee;
using FRequests = FXiaoKe.Requests;
using KRequests = Kingdee.Requests;
using FResponses = FXiaoKe.Responses;
using KResponses = Kingdee.Responses;

namespace TheFirstFarm.Test {
	public class ReturnOrderTests {
		public Client FXiaoKeClient { get; set; }

		public Kingdee.Client KingdeeClient { get; set; }

		[SetUp]
		public void Setup() {
			JsonConvert.DefaultSettings = () => new JsonSerializerSettings {ContractResolver = new JsonIncludeResolver()};
			FXiaoKeClient = new Client(
				"FSAID_1319ebe",
				"fe4fd3abb55a45d3ae5ed03b3bcb6fc8",
				"D63C0B6A42F171D173EF728CBFC12874"
			);
			FXiaoKeClient.RequestFailed += (_, args) => {
				if (args.Response is FResponses.BasicResponse resp)
					Console.WriteLine(resp.ErrorMessage);
			};
			KingdeeClient = new Kingdee.Client("http://120.27.55.22/k3cloud/");
			Assert.IsTrue(KingdeeClient.ValidateLogin("60b86b4a9ade83", "Administrator", "888888", 2052));
		}

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
		public async Task FXiaoKeQueryTest<T>(params FRequests.ModelFilter<T>[] filters) where T : ModelBase {
			FXiaoKeClient.Operator = await FXiaoKeClient.GetStaffByPhoneNumber("18118359138");
			var result = await FXiaoKeClient.QueryByCondition(filters);
			Console.WriteLine($@"{result?.Count} {typeof(T).Name} found");
		}

		[Test]
		public async Task RecordOrderSaveTest() {
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
			var fXiaoKeModels = kingdeeModels.Select(
				model =>
					new FModels.ReturnOrder {
						CustomerId = model.CustomerId,
						Date = model.Date!.Value,
						OwnerId = staffs.SingleOrDefault(staff => !string.IsNullOrEmpty(model.SalesmanId) && staff.Number == model.SalesmanId)?.Id ?? FXiaoKeClient.Operator.Id,
						Reason = model.ReturnReason,
						Id = model.Number,
						BusinessType = model.BusinessType,
						Details = model.Details.Select(
								detail => new FModels.ReturnOrderDetail {
									ProductId = detail.MaterialId,
									ProductName = detail.MaterialName,
									Specification = detail.MaterialModel,
									SaleUnit = detail.SaleUnit,
									ReturnAmount = detail.ReturnAmount,
									UnitPrice = detail.UnitPrice,
									TaxRate = detail.TaxRate,
									Volume = detail.Volumn,
									ReturnType = Utility.TransformEnum<KModels.ReturnType, FModels.ReturnType>(detail.ReturnType) ?? throw new Exception(),
									OwnerId = model.SalesmanId
								}
							)
							.ToList()
					}
			);
			foreach (var model in fXiaoKeModels)
				await FXiaoKeClient.Create(model);
		}
	}

	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public class TestCaseGenericAttribute : TestCaseAttribute, ITestBuilder {
		public TestCaseGenericAttribute(params object[] arguments)
			: base(arguments) { }

		public Type GenericArgument {
			get => GenericArguments.SingleOrDefault();
			set => GenericArguments = new[] {value};
		}

		public Type[] GenericArguments { get; set; }

		IEnumerable<TestMethod> ITestBuilder.BuildFrom(IMethodInfo method, NUnit.Framework.Internal.Test suite) {
			if (!method.IsGenericMethodDefinition)
				return BuildFrom(method, suite);

			if (GenericArguments == null || GenericArguments.Length != method.GetGenericArguments().Length) {
				var @params = new TestCaseParameters {RunState = RunState.NotRunnable};
				@params.Properties.Set("_SKIPREASON", $"{nameof(GenericArguments)} should have {method.GetGenericArguments().Length} elements");
				return new[] {new NUnitTestCaseBuilder().BuildTestMethod(method, suite, @params)};
			}

			var genMethod = method.MakeGenericMethod(GenericArguments);
			return BuildFrom(genMethod, suite);
		}
	}
}