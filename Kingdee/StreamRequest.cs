using System;
using System.Net;
using System.Text;

namespace Kingdee {
	internal class StreamRequest : ApiRequest {
		public StreamRequest(string serverUrl, Encoding encoder, CookieContainer cookies)
			: base(serverUrl, true, encoder, cookies) { }

		public override Uri ServiceUri => new(new Uri(ServerUrl), "stream.kdsvc");
	}
}