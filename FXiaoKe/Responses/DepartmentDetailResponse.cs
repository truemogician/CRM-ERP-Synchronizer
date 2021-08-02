using FXiaoKe.Models;
using Newtonsoft.Json;

namespace FXiaoKe.Responses {
	public class DepartmentDetailResponse : BasicResponse {
		[JsonProperty("department")]
		public Department Department { get; set; }
	}
}