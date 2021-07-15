using System.ComponentModel.DataAnnotations;
using FXiaoKe.Response;
using Newtonsoft.Json;

namespace FXiaoKe.Request {
	/// <summary>
	///     获取应用及授权请求
	/// </summary>
	[Request("/cgi/corpAccessToken/get/V2", typeof(AuthorizationResponse))]
	public class AuthorizationRequest : RequestBase {
		/// <summary>
		///     企业应用ID
		/// </summary>
		[JsonProperty("appId")]
		[Required]
		public string AppId { get; set; }

		/// <summary>
		///     企业应用凭证密钥
		/// </summary>
		[JsonProperty("appSecret")]
		[Required]
		public string AppSecret { get; set; }

		/// <summary>
		///     永久授权码
		/// </summary>
		[JsonProperty("permanentCode")]
		[Required]
		public string PermanentCode { get; set; }
	}
}