using Newtonsoft.Json;

namespace FXiaoKe.Api.Request {
	public class RequestBase {
		/// <summary>
		/// 企业应用访问公司合法性凭证
		/// </summary>
		[JsonProperty("corpAccessToken")]
		public string CorpAccessToken { get; set; }

		/// <summary>
		/// 企业ID
		/// </summary>
		[JsonProperty("corpId")]
		public string CorpId { get; set; }
	}
}