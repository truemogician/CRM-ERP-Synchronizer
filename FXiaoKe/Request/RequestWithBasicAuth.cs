using Newtonsoft.Json;

namespace FXiaoKe.Request {
	public class RequestWithBasicAuth : RequestBase {
		/// <summary>
		///     企业应用访问公司合法性凭证
		/// </summary>
		[JsonProperty("corpAccessToken")]
		public string CorpAccessToken { get; set; }

		/// <summary>
		///     企业ID
		/// </summary>
		[JsonProperty("corpId")]
		public string CorpId { get; set; }

		public virtual void UseClient(Client client) {
			CorpAccessToken = client.CorpAccessToken;
			CorpId = client.CorpId;
		}
	}
}