using System.ComponentModel.DataAnnotations;
using FXiaoKe.Response;
using Newtonsoft.Json;

namespace FXiaoKe.Request {
	[Request("/cgi/user/getByMobile", typeof(StaffQueryResponse))]
	public class StaffQueryRequest : RequestWithBasicAuth {
		public StaffQueryRequest() { }
		public StaffQueryRequest(string number) => PhoneNumber = number;

		/// <summary>
		///     员工手机号
		/// </summary>
		[JsonProperty("mobile")]
		[Required]
		public string PhoneNumber { get; set; }
	}
}