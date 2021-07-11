using Newtonsoft.Json;

namespace FXiaoKe.Api.Response {
	public class BasicResponse : ResponseBase {
		[JsonProperty("errorDescription")]
		public ErrorCode ErrorDescription { get; set; }
	}
}