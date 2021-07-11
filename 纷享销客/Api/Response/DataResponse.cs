using Newtonsoft.Json;

namespace FXiaoKe.Api.Response {
	public class DataResponse<T> : BasicResponse {
		[JsonProperty("data")]
		public T Data { get; set; }
	}
}