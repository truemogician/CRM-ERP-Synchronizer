using Newtonsoft.Json;

namespace FXiaoKe.Response {
	public class DataResponse<T> : BasicResponseWithDescription {
		[JsonProperty("data")]
		public T Data { get; set; }
	}
}