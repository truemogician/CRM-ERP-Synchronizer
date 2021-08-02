// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
using System;
using Newtonsoft.Json;
using Shared.Serialization;

namespace Kingdee.Forms {
	[Form]
	public abstract class ErpModelBase : ModelBase {
		[JsonProperty("FDocumentStatus")]
		public Status Status { get; set; }

		[JsonProperty("FModifierId")]
		public int? AuditorId { get; set; }

		[JsonProperty("FModifyDate")]
		public DateTime? AuditionTime { get; set; }

		[JsonProperty("FForbidderId")]
		public int? InvalidatorId { get; set; }

		[JsonProperty("FForbidDate")]
		public DateTime? InvalidatedTime { get; set; }
	}

	[JsonConverter(typeof(EnumValueConverter))]
	public enum Status : byte {
		[EnumValue("Z")]
		Staged,

		[EnumValue("A")]
		Created,

		[EnumValue("B")]
		Auditing,

		[EnumValue("C")]
		Audited,

		[EnumValue("D")]
		Reauditing
	}
}