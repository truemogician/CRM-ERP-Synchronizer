using FXiaoKe.Response;
using Newtonsoft.Json;

namespace FXiaoKe.Request {
	[Request("/cgi/department/detail", typeof(DepartmentDetailResponse))]
	public class DepartmentDetailRequest : RequestWithBasicAuth {
		public DepartmentDetailRequest(int departmentId) => DepartmentId = departmentId;

		[JsonProperty("departmentId")]
		public int DepartmentId { get; set; }
	}
}