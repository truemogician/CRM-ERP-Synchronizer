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

		[JsonProperty("FApproverId")]
		public int? AuditorId { get; set; }

		[JsonProperty("FApproveDate")]
		public DateTime? AuditionTime { get; set; }
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