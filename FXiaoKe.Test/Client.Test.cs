using System;
using System.Linq;
using System.Threading.Tasks;
using FXiaoKe.Response;
using NUnit.Framework;

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
			Client.OnRequestFail += (_, args) => Assert.Fail(args.Response is BasicResponse resp ? resp.ErrorMessage : "Request failed");
			Client.OnValidationFail += (_, args) => Assert.Fail($"Validation failed: {string.Join(' ', args.Results.Select(res => res.ErrorMessage))}");
		}

		[Test]
		public async Task AuthorizationTest() => Assert.IsTrue(await Client.Authorize());

		[TestCase("18118359138")]
		public async Task SetOperatorTest(string phoneNumber) => Assert.IsTrue(await Client.SetOperator(phoneNumber));

		[TestCase("²âÊÔÏûÏ¢", "18118359138")]
		public async Task SendTextMessageTest(string message, params string[] phoneNumbers) {
			string[] ids = await Task.WhenAll(phoneNumbers.Select(number => Client.QueryStaff(number).ContinueWith(task => task.Result.Id)));
			Assert.DoesNotThrowAsync(() => Client.SendTextMessage(message, ids));
		}

		[Test]
		public async Task GetDepartmentInfoTreeTest() {
			var department = await Client.GetDepartmentInfoTree();
			Console.WriteLine($"Height: {department.Height}");
			Console.WriteLine($"Count: {department.Size}");
		}

		[Test]
		public async Task GetDepartmentDetailTreeTest() {
			var department = await Client.GetDepartmentDetailTree();
			Console.WriteLine($"Height: {department.Height}");
			Console.WriteLine($"Count: {department.Size}");
		}

		[Test]
		public async Task GetStaffsTest() {
			var department = await Client.GetDepartmentDetailTree();
			await Client.GetStaffs(department);
			foreach (var dep in department)
				Console.WriteLine($"{dep.Name}: {dep.Staffs?.Count ?? 0} staffs");
		}
	}
}