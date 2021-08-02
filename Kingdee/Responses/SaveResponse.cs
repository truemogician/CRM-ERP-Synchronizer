using Newtonsoft.Json;

namespace Kingdee.Responses {
	public class SaveResponse : BatchSaveResponse {
		[JsonProperty("Id")]
		public int Id { get; set; }

		[JsonProperty("Number")]
		public string Number { get; set; }
	}
}