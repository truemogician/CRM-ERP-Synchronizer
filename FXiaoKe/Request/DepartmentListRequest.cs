using FXiaoKe.Response;

namespace FXiaoKe.Request {
	[Request("/cgi/department/list", typeof(DepartmentListResponse))]
	public class DepartmentListRequest : RequestWithBasicAuth { }
}