// ReSharper disable StringLiteralTypo
using System;
using FXiaoKe;
using Newtonsoft.Json;
using NUnit.Framework;
using Shared.Serialization;

namespace TheFirstFarm.Test {
	public class TestBase {
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
				if (args.Response is FXiaoKe.Responses.BasicResponse resp)
					Console.WriteLine(resp.ErrorMessage);
			};
			KingdeeClient = new Kingdee.Client("http://120.27.55.22/k3cloud/");
			Assert.IsTrue(KingdeeClient.ValidateLogin("60b86b4a9ade83", "Administrator", "888888", 2052));
		}
	}
}