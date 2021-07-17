using System;
using System.Net;
using System.Text;

namespace Kingdee {
	internal class ApiProgressRequest : ApiRequest {
		private readonly string _reqId;

		internal ApiProgressRequest(
			string serverUrl,
			bool async,
			Encoding encoder,
			CookieContainer cookies,
			string reqId
		)
			: base(serverUrl, async, encoder, cookies)
			=> _reqId = reqId;

		public override Uri ServiceUri => IsAsync ? new Uri(new Uri(ServerUrl), "_prs_.kdsvc?rid=" + _reqId) : new Uri(new Uri(ServerUrl), "_pr_.kdsvc?rid=" + _reqId);
	}
}