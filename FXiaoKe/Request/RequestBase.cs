using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using FXiaoKe.Utilities;
using Newtonsoft.Json;
using Shared.Utilities;

namespace FXiaoKe.Request {
	[Request(null, HttpMethod.Post)]
	public abstract class RequestBase {
		public static implicit operator HttpRequestMessage(RequestBase self) {
			var attrs = self.GetType().GetRequestAttributes();
			var request = new HttpRequestMessage(
				attrs.First(attr => attr.Method is not null).Method,
				attrs.First(attr => attr.Path != "/").Url
			) {
				Content = new StringContent(JsonConvert.SerializeObject(self), Encoding.UTF8)
			};
			request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
			return request;
		}

		public virtual List<ValidationResult> Validate() => Utility.Validate(this);
	}
}