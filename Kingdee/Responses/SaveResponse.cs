using System.Collections.Generic;
using Newtonsoft.Json;

namespace Kingdee.Responses {
	public class SaveResponse : BatchSaveResponse {
		[JsonProperty("Id")]
		public string Id { get; set; }

		[JsonProperty("Number")]
		public string Number { get; set; }
	}
}