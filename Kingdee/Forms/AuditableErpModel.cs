// ReSharper disable StringLiteralTypo
using System;
using Newtonsoft.Json;

namespace Kingdee.Forms {
	public abstract class AuditableErpModel : ErpModelBase {
		/// <summary>
		///		审核人Id
		/// </summary>
		[JsonProperty("FApproverId")]
		public int? AuditorId { get; set; }

		/// <summary>
		///		审核时间
		/// </summary>
		[JsonProperty("FApproveDate")]
		public DateTime? AuditionTime { get; set; }
	}
}