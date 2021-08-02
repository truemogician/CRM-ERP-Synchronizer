using System.Collections.Generic;
using FXiaoKe.Models;
using Newtonsoft.Json;

namespace FXiaoKe.Responses {
	public class DepartmentListResponse : BasicResponse {
		[JsonProperty("departments")]
		public List<DepartmentInfo> Departments { get; set; }
	}
}