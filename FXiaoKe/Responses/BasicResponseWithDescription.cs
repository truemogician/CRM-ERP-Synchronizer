using Newtonsoft.Json;

namespace FXiaoKe.Responses {
	public class BasicResponseWithDescription : BasicResponse {
		[JsonProperty("errorDescription")]
		public string ErrorDescription { get; set; }
	}
}