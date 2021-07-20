using FXiaoKe.Models;
using Newtonsoft.Json;

namespace FXiaoKe.Response {
	public class DepartmentDetailResponse : BasicResponseWithDescription {
		[JsonProperty("department")]
		public Department Department { get; set; }
	}
}