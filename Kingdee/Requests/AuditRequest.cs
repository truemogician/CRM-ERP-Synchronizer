// ReSharper disable StringLiteralTypo
using System.Collections.Generic;
using Kingdee.Forms;
using Newtonsoft.Json;
using Shared.Serialization;

namespace Kingdee.Requests {
	public class AuditRequest<T> : DeleteRequest<T> where T : ErpModelBase {
		public AuditRequest(params int[] ids) : base(ids) { }

		public AuditRequest(params string[] numbers) : base(numbers) { }

		public AuditRequest(params T[] entities) : base(entities) { }

		/// <summary>
		///     交互标志集合
		/// </summary>
		[JsonProperty("InterationFlags")]
		[JsonConverter(typeof(StringCollectionConverter), ';')]
		public List<string> InteractionFlags { get; set; } = new();
	}
}