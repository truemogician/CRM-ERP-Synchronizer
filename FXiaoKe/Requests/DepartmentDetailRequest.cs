using FXiaoKe.Responses;
using Newtonsoft.Json;

namespace FXiaoKe.Requests {
	[Request("/cgi/department/detail", typeof(DepartmentDetailResponse))]
	public class DepartmentDetailRequest : RequestWithBasicAuth {
		public DepartmentDetailRequest(int departmentId) => DepartmentId = departmentId;

		[JsonProperty("departmentId")]
		public int DepartmentId { get; set; }
	}
}