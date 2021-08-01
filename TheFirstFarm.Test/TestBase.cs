// ReSharper disable StringLiteralTypo
using System;
using System.Configuration;
using FXiaoKe.Responses;
using Kingdee;
using NUnit.Framework;
using FClient = FXiaoKe.Client;
using KClient = Kingdee.Client;

namespace TheFirstFarm.Test {
	public abstract class TestBase {
		protected FClient FClient { get; set; }

		protected KClient KClient { get; set; }

		[SetUp]
		public void Setup() {
			var fileMap = new ConfigurationFileMap(@"..\..\..\..\TheFirstFarm\App.config");
			var configuration = ConfigurationManager.OpenMappedMachineConfiguration(fileMap);
			var fSection = (configuration.GetSection("fXiaoKe") as AppSettingsSection)!.Settings;
			FClient = new FClient(
				fSection["appId"].Value,
				fSection["appSecret"].Value,
				fSection["permanentCode"].Value
			);
			FClient.RequestFailed += (_, args) => {
				if (args.Response is BasicResponse resp)
					Console.WriteLine(resp.ErrorMessage);
			};
			var kSection = (configuration.GetSection("kingdee") as AppSettingsSection)!.Settings;
			KClient = new KClient(
				kSection["url"].Value,
				kSection["databaseId"].Value,
				kSection["username"].Value,
				kSection["password"].Value,
				(Language)Convert.ToInt32(kSection["languageId"].Value)
			);
		}
	}
}