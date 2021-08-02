// ReSharper disable StringLiteralTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Kingdee.Forms;
using Kingdee.Forms.Enums;
using Newtonsoft.Json;
using TheFirstFarm.Models.Common;

namespace TheFirstFarm.Models.Kingdee {
	[Form("SAL_RETURNSTOCK")]
	public class ReturnOrder : ErpModelBase {
		[JsonProperty("FID")]
		[Key]
		public int Id { get; set; }

		[JsonProperty("FBillNo")]
		public string Number { get; set; }

		[JsonProperty("FBussinessType")]
		public BusinessType BusinessType { get; set; }

		[JsonProperty("FDate")]
		public DateTime? Date { get; set; }

		[JsonProperty("FRetcustId")]
		[Required]
		public NumberWrapper CustomerNumber { get; set; }

		[JsonProperty("FSalesManId")]
		[Required]
		public NumberWrapper SalesmanNumber { get; set; }

		[JsonProperty("FReturnReason")]
		public NumberWrapper ReturnReason { get; set; }

		[JsonProperty("FDocumentStatus")]
		public RecordStatus RecordStatus { get; set; }

		[JsonProperty("FApproveDate")]
		public DateTime? AuditDate { get; set; }

		[JsonProperty("FEntity")]
		[SubForm]
		public List<ReturnOrderDetail> Details { get; set; }
	}
}