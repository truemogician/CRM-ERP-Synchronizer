using FXiaoKe.Responses;

namespace FXiaoKe.Requests {
	[Request("/cgi/department/list", typeof(DepartmentListResponse))]
	public class DepartmentListRequest : RequestWithBasicAuth { }
}