using System.Collections.Generic;
using Kingdee.Forms;

namespace Kingdee.Requests {
	[Request("Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.UnAudit")]
	public class UnauditRequest<T> : AuditRequest<T> where T : ErpModelBase {
		public UnauditRequest(params int[] ids) : base(ids) { }

		public UnauditRequest(params string[] numbers) : base(numbers) { }

		public UnauditRequest(params T[] entities) : base(entities) { }

		public UnauditRequest(DeleteRequest<T> request) {
			CreatorOrgId = request.CreatorOrgId;
			Ids = new List<int>(request.Ids);
			Numbers = new List<string>(request.Numbers);
			NetworkControl = request.NetworkControl;
		}
	}
}