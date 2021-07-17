using System;
using NUnit.Framework;

namespace Kingdee.Test {
	public class ClientTests {
		public Client Client { get; set; }

		[SetUp]
		public void Setup() {
			Client = new Client("http://120.27.55.22/k3cloud/");
		}

		[Test]
		public void ValidateLoginTest() {
			var json = Client.ValidateLogin("60b86b4a9ade83", "Administrator", "888888", 2052);
			Console.WriteLine(json);
			Assert.Pass();
		}
	}
}