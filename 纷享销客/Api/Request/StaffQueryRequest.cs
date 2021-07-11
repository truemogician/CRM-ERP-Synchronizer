using System.ComponentModel.DataAnnotations;
using FXiaoKe.Api.Response;
using FXiaoKe.Utilities;
using Newtonsoft.Json;

namespace FXiaoKe.Api.Request {
	[Request("/cgi/user/getByMobile", HttpMethod.Post, typeof(StaffQueryResponse))]
	public class StaffQueryRequest : RequestBase {
		/// <summary>
		/// 员工手机号
		/// </summary>
		[JsonProperty("mobile")]
		[Required]
		public string PhoneNumber { get; set; }
	}
}