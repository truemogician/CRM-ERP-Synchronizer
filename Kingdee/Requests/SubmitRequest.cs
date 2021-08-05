using Kingdee.Forms;
using Newtonsoft.Json;

namespace Kingdee.Requests {
	[Request("Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.Submit")]
	public class SubmitRequest<T> : DeleteRequest<T> where T : ErpModelBase {
		public SubmitRequest(params int[] ids) : base(ids) { }

		public SubmitRequest(params string[] numbers) : base(numbers) { }

		public SubmitRequest(params T[] entities) : base(entities) { }

		[JsonProperty("SelectedPostId")]
		public int StaffPostId { get; set; }
	}
}