// ReSharper disable StringLiteralTypo
using System.Collections.Generic;
using Kingdee.Forms;
using Newtonsoft.Json;
using Shared.Serialization;

namespace Kingdee.Requests {
	[Request("Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.Audit")]
	public class AuditRequest<T> : DeleteRequest<T> where T : ErpModelBase {
		protected AuditRequest() { }

		public AuditRequest(params int[] ids) : base(ids) { }

		public AuditRequest(params string[] numbers) : base(numbers) { }

		public AuditRequest(params T[] entities) : base(entities) { }

		public AuditRequest(DeleteRequest<T> request) {
			CreatorOrgId = request.CreatorOrgId;
			Ids = new List<int>(request.Ids);
			Numbers = new List<string>(request.Numbers);
			NetworkControl = request.NetworkControl;
		}

		/// <summary>
		///     交互标志集合
		/// </summary>
		[JsonProperty("InterationFlags")]
		[JsonConverter(typeof(StringCollectionConverter), ';')]
		public List<string> InteractionFlags { get; set; } = new();
	}
}