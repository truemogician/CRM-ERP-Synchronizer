using System.Collections.Generic;
using Newtonsoft.Json;

namespace Kingdee.Responses {
	public class BatchSaveResponse : BasicResponse {
		[JsonProperty("NeedReturnData")]
		public List<object> DataToReturn { get; set; }
	}
}