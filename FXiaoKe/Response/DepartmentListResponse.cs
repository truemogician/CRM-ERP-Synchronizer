using System.Collections.Generic;
using FXiaoKe.Models;
using Newtonsoft.Json;

namespace FXiaoKe.Response {
	public class DepartmentListResponse : BasicResponseWithDescription {
		[JsonProperty("departments")]
		public List<DepartmentInfo> Departments { get; set; }
	}
}