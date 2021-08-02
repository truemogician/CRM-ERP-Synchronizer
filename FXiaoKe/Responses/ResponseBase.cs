using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using Newtonsoft.Json;
using Shared.Utilities;

namespace FXiaoKe.Responses {
	public abstract class ResponseBase {
		[JsonIgnore]
		public HttpResponseMessage ResponseMessage { get; set; }
	}
}