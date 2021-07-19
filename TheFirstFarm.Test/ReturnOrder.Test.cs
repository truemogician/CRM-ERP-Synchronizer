// ReSharper disable StringLiteralTypo
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FXiaoKe;
using Kingdee.Forms;
using Kingdee.Requests;
using Newtonsoft.Json;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Builders;
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
			var kingdeeModels = KingdeeClient.Query(new QueryRequest<ReturnOrder>()).AsT1;
			var fXiaoKeModels = kingdeeModels.Select(
				model =>
					new Models.FXiaoKe.ReturnOrder() {
						CustomerId = model.CustomerId.Number,
						Date = model.Date!.Value,
						OwnerId = model.SalesmanId.Number,
						Reason = model.ReturnReason,
						Id = model.Number
					}
			);
			foreach (var model in fXiaoKeModels) {
				var resp = await FXiaoKeClient.Create(model);
				Assert.IsTrue(resp);
			}
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
			return base.BuildFrom(genMethod, suite);
		}
	}
}