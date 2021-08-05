using Kingdee.Forms;

namespace Kingdee.Requests {
	[Request("Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.Draft")]
	public class DraftRequest<T> : SaveRequest<T> where T : ErpModelBase {
		public DraftRequest() { }

		public DraftRequest(T data) : base(data) { }
	}
}