using System.ComponentModel.DataAnnotations;
using FXiaoKe.Responses;
using Newtonsoft.Json;

namespace FXiaoKe.Requests {
	[Request("/cgi/user/list", typeof(StaffListResponse))]
	public class StaffListRequest : RequestWithBasicAuth {
		public StaffListRequest() { }

		public StaffListRequest(int departmentId, bool includeChild = true) {
			DepartmentId = departmentId;
			IncludeChild = includeChild;
		}

		[JsonProperty("departmentId")]
		[Required]
		public int DepartmentId { get; set; }

		[JsonProperty("fetchChild")]
		public bool IncludeChild { get; set; } = true;

		[JsonProperty("showDepartmentIdsDetail")]
		public bool ShowDepartmentDetail { get; } = true;
	}
}