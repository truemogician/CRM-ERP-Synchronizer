using Newtonsoft.Json;

namespace FXiaoKe.Responses {
	public class DataResponse<T> : BasicResponseWithDescription {
		[JsonProperty("data")]
		public T Data { get; set; }
	}
}