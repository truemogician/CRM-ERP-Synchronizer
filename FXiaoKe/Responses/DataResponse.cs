using Newtonsoft.Json;

namespace FXiaoKe.Responses {
	public class DataResponse<T> : BasicResponse {
		[JsonProperty("data")]
		public T Data { get; set; }
	}
}