using System.Net.Http;
using Newtonsoft.Json;

namespace FXiaoKe.Responses {
	public abstract class ResponseBase {
		[JsonIgnore]
		public HttpResponseMessage ResponseMessage { get; set; }
	}
}