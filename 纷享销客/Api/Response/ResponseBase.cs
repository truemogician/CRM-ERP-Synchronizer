using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace FXiaoKe.Api.Response {
	public class ResponseBase {
		/// <summary>
		/// 返回码
		/// </summary>
		[JsonProperty("errorCode")]
		[Required]
		public ErrorCode ErrorCode { get; set; }

		/// <summary>
		/// 对返回码的文本描述内容
		/// </summary>
		[JsonProperty("errorMessage")]
		[Required]
		public ErrorCode ErrorMessage { get; set; }
	}
}