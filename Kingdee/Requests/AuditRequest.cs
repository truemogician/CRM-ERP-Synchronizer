// ReSharper disable StringLiteralTypo
using System.Collections.Generic;
using Kingdee.Forms;
using Newtonsoft.Json;
using Shared.Serialization;

namespace Kingdee.Requests {
	public class AuditRequest<T> : DeleteRequest<T> where T : FormBase {
		public AuditRequest(string idName, params T[] entities) : base(idName, entities) { }

		/// <summary>
		///     交互标志集合
		/// </summary>
		[JsonProperty("InterationFlags")]
		[JsonConverter(typeof(StringCollectionConverter), ';')]
		public List<string> InteractionFlags { get; set; } = new();
	}
}