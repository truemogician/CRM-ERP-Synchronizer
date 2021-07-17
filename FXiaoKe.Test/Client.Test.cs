using System.Collections;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using FXiaoKe.Request.Message;

namespace FXiaoKe.Test {
	public class ClientTests {
		public Client Client { get; set; }

		[SetUp]
		public void Setup() {
			Client = new Client(
				"FSAID_1319ebe",
				"fe4fd3abb55a45d3ae5ed03b3bcb6fc8",
				"D63C0B6A42F171D173EF728CBFC12874"
			);
		}

		[Test]
		public async Task AuthorizationTest() => Assert.IsTrue(await Client.Authorize());

		[Test]
		public async Task SetOperatorTest() => Assert.IsTrue(await Client.SetOperator("18118359138"));

		[Test]
		public async Task SendTextMessageTest() {
			await Client.SetOperator("18118359138");
			var response = await Client.SendTextMessage("Test Message");
			Assert.IsTrue(response);
		}

		[Test]
		public async Task QueryByConditionTest() {
			await Client.SetOperator("18118359138");

		}
	}
}