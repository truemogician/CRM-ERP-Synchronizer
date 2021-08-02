using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace Kingdee {
	internal class ApiServiceRequest : ApiRequest {
		internal ApiServiceRequest(
			string serverUrl,
			bool async,
			Encoding encoder,
			CookieContainer cookies,
			string serviceName,
			object[] parameters
		)
			: base(serverUrl, async, encoder, cookies) {
			ServiceName = serviceName;
			Parameters = parameters == null ? new List<object>() : new List<object>(parameters);
		}

		public string ServiceName { get; }

		public List<object> Parameters { get; }

		public override Uri ServiceUri => IsAsync ? new Uri(new Uri(ServerUrl), "a\\" + ServiceName + ".common.kdsvc") : new Uri(new Uri(ServerUrl), ServiceName + ".common.kdsvc");

		public override string SerializeBody() {
			Body["rid"] = RequestId;
			Body["parameters"] = JsonConvert.SerializeObject(Parameters);
			return base.SerializeBody();
		}
	}
}