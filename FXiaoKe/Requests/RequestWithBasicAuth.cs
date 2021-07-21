using System;
using Newtonsoft.Json;

namespace FXiaoKe.Requests {
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
			if (string.IsNullOrEmpty(client.CorpAccessToken) || string.IsNullOrEmpty(client.CorpId))
				throw new InvalidOperationException($"{nameof(client.CorpAccessToken)} or {nameof(client.CorpId)} is empty");
			CorpAccessToken = client.CorpAccessToken;
			CorpId = client.CorpId;
		}
	}
}