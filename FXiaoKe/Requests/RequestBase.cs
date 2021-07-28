using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Shared.Utilities;

namespace FXiaoKe.Requests {
	[Request(null, HttpMethod.Post)]
	public abstract class RequestBase {
		[JsonIgnore]
		public RequestAttribute Attribute {
			get {
				var attrs = GetType().GetCustomAttributes<RequestAttribute>().AsList();
				if (attrs.Count <= 1)
					return attrs.SingleOrDefault();
				return new RequestAttribute {
					Url = attrs.FirstOrDefault(attr => attr.Path != "/")?.Url,
					Method = attrs.FirstOrDefault(attr => attr.Method is not null)?.Method,
					ResponseType = attrs.FirstOrDefault(attr => attr.ResponseType is not null)?.ResponseType,
					ErrorMessage = attrs.FirstOrDefault(attr => !string.IsNullOrEmpty(attr.ErrorMessage))?.ErrorMessage
				};
			}
		}

		public static implicit operator HttpRequestMessage(RequestBase self) {
			var attribute = self.Attribute;
			var request = new HttpRequestMessage(attribute.Method, attribute.Url) {
				Content = new StringContent(JsonConvert.SerializeObject(self), Encoding.UTF8)
			};
			request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
			return request;
		}

		public virtual List<ValidationResult> Validate(bool recursive = true) => Utility.Validate(this, recursive);
	}
}