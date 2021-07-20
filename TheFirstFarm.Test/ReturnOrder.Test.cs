// ReSharper disable StringLiteralTypo
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FXiaoKe;
using FXiaoKe.Request;
using FXiaoKe.Request.Message;
using FXiaoKe.Response;
using FXiaoKe.Utilities;
using Kingdee.Forms;
using Kingdee.Requests;
using Newtonsoft.Json;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Builders;
using Shared.Serialization;
using Shared.Utilities;
using TheFirstFarm.Models.Kingdee;

namespace TheFirstFarm.Test {
	public class ReturnOrderTests {
		public Client FXiaoKeClient { get; set; }

		public Kingdee.Client KingdeeClient { get; set; }

		[SetUp]
		public void Setup() {
			FXiaoKeClient = new Client(
				"FSAID_1319ebe",
				"fe4fd3abb55a45d3ae5ed03b3bcb6fc8",
				"D63C0B6A42F171D173EF728CBFC12874"
			);
			KingdeeClient = new Kingdee.Client("http://120.27.55.22/k3cloud/");
			Assert.IsTrue(KingdeeClient.ValidateLogin("60b86b4a9ade83", "Administrator", "888888", 2052));
			JsonConvert.DefaultSettings = () => new JsonSerializerSettings() {ContractResolver = new JsonIncludeResolver()};
		}

		[TestCaseGeneric(TypeArguments = new[] {typeof(ReturnOrder)}, ExpectedResult = 18)]
		public int KingdeeFieldsTest<T>() where T : FormBase {
			var fields = FormMeta<T>.QueryFields;
			Console.WriteLine(string.Join(", ", fields.Select(field => field.ToString("json"))));
			return fields.Count;
		}

		[TestCaseGeneric(TypeArguments = new[] {typeof(ReturnOrder)})]
		public void KingdeeQueryTest<T>() where T : FormBase {
			var response = KingdeeClient.Query(new QueryRequest<T>());
			Assert.IsTrue(response.IsT1);
			foreach (var resp in response.AsT1)
				Console.WriteLine(JsonConvert.SerializeObject(resp, Formatting.Indented));
		}

		[Test]
		public async Task RecordOrderSaveTest() {
			FXiaoKeClient.OnRequestFail += (sender, args) => {
				var reqType = args.Request.GetType();
				if (args.Request is MessageRequest) {
					args.Continue = true;
					return;
				}
				if (args.Response is BasicResponse resp) {
					var attr = args.Request.Attribute;
					var composite = new CompositeMessage(attr.ErrorMessage ?? "发生未知错误", Client.Origin) {
						Head = resp.ErrorMessage,
						Form = new List<LabelAndValue>() {
							("时间", DateTime.Now.ToString("yyyy-MM-dd H:mm:ss.fff")),
							("路径", attr.Url.PathAndQuery)
						}
					};
					if (reqType.IsAssignableToGeneric(typeof(CreationRequestBase<>))) {
						var modelType = ((dynamic)args.Request).Data.Model.GetType() as Type;
						composite.Form.Add(("对象", modelType.GetModelName()));
					}
					var _ = FXiaoKeClient.SendCompositeMessage(composite);
				}
				args.Continue = args.Response.ResponseMessage.IsSuccessStatusCode;
			};
			var kingdeeModels = KingdeeClient.Query(new QueryRequest<ReturnOrder>()).AsT1;
			var fXiaoKeModels = kingdeeModels.Select(
				model =>
					new Models.FXiaoKe.ReturnOrder() {
						CustomerId = model.CustomerId,
						Date = model.Date!.Value,
						OwnerId = model.SalesmanId,
						Reason = model.ReturnReason,
						Id = model.Number,
						BusinessType = model.BusinessType,
						Details = model.Details.Select(
								detail => new Models.FXiaoKe.ReturnOrderDetail() {
									ProductId = detail.MaterialId,
									ProductName = detail.MaterialName,
									Specification = detail.MaterialModel,
									SaleUnit = detail.SaleUnit,
									ReturnAmount = detail.ReturnAmount,
									UnitPrice = detail.UnitPrice,
									TaxRate = detail.TaxRate,
									Volume = detail.Volumn,
									ReturnType = Utilities.Utility.TransformEnum<ReturnType, Models.FXiaoKe.ReturnType>(detail.ReturnType) ?? throw new Exception(),
									OwnerId = model.SalesmanId
								}
							)
							.ToList()
					}
			);
			Assert.IsTrue(await FXiaoKeClient.SetOperator("18118359138"));
			foreach (var model in fXiaoKeModels)
				await FXiaoKeClient.Create(model);
		}
	}

	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public class TestCaseGenericAttribute : TestCaseAttribute, ITestBuilder {
		public TestCaseGenericAttribute(params object[] arguments)
			: base(arguments) { }

		public Type[] TypeArguments { get; set; }

		IEnumerable<TestMethod> ITestBuilder.BuildFrom(IMethodInfo method, NUnit.Framework.Internal.Test suite) {
			if (!method.IsGenericMethodDefinition)
				return base.BuildFrom(method, suite);

			if (TypeArguments == null || TypeArguments.Length != method.GetGenericArguments().Length) {
				var @params = new TestCaseParameters {RunState = RunState.NotRunnable};
				@params.Properties.Set("_SKIPREASON", $"{nameof(TypeArguments)} should have {method.GetGenericArguments().Length} elements");
				return new[] {new NUnitTestCaseBuilder().BuildTestMethod(method, suite, @params)};
			}

			var genMethod = method.MakeGenericMethod(TypeArguments);
			return BuildFrom(genMethod, suite);
		}
	}
}