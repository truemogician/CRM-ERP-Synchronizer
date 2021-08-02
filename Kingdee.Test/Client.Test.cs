using System;
using System.Configuration;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Kingdee.Test {
	public class ClientTests {
		public Client Client { get; set; }

		[SetUp]
		public void Setup() {
			var fileMap = new ConfigurationFileMap(@"..\..\..\..\TheFirstFarm\App.config");
			var configuration = ConfigurationManager.OpenMappedMachineConfiguration(fileMap);
			var kSection = (configuration.GetSection("kingdee") as AppSettingsSection)!.Settings;
			Client = new Client(
				kSection["url"].Value,
				kSection["databaseId"].Value,
				kSection["username"].Value,
				kSection["password"].Value,
				(Language)Convert.ToInt32(kSection["languageId"].Value)
			);
		}

		[Test]
		public void ValidateLoginTest() {
			var resp = Client.ValidateLogin();
			Console.WriteLine(JsonConvert.SerializeObject(resp));
			Assert.IsTrue(resp);
		}

		[Test]
		public async Task ValidateLoginAsyncTest() {
			var resp = await Client.ValidateLoginAsync();
			Console.WriteLine(JsonConvert.SerializeObject(resp));
			Assert.IsTrue(resp);
		}
	}
}