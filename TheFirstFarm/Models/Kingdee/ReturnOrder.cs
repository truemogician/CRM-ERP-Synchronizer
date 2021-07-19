// ReSharper disable StringLiteralTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Kingdee.Forms;
using Kingdee.Forms.Enums;
using Newtonsoft.Json;
using TheFirstFarm.Models.Common;

namespace TheFirstFarm.Models.Kingdee {
	[Form("SAL_RETURNSTOCK")]
	public class ReturnOrder : FormBase {
		[JsonProperty("FID")]
		[Key]
		[JsonInclude]
		public int Id { get; set; }

		[JsonProperty("FBillNo")]
		[JsonInclude]
		public string Number { get; set; }

		[JsonProperty("FBussinessType")]
		[JsonInclude]
		public BusinessType BusinessType { get; set; }

		[JsonProperty("FDate")]
		[JsonInclude]
		public DateTime? Date { get; set; }

		[JsonProperty("FRetcustId")]
		[JsonInclude]
		public NumberWrapper CustomerId { get; set; }

		[JsonProperty("FSalesManId")]
		[JsonInclude]
		public NumberWrapper SalesmanId { get; set; }

		[JsonProperty("FReturnReason")]
		[JsonInclude]
		public NumberWrapper ReturnReason { get; set; }

		[JsonProperty("FEntity")]
		[SubForm]
		[JsonInclude]
		public List<ReturnOrderDetail> Details { get; set; }

		[JsonProperty("FDocumentStatus")]
		[JsonInclude]
		public RecordStatus RecordStatus { get; set; }

		[JsonProperty("FApproveDate")]
		[JsonInclude]
		public DateTime? AuditDate { get; set; }

		[JsonProperty("FBillTypeID")]
		public NumberWrapper FBillTypeID { get; set; }

		[JsonProperty("FSaleOrgId")]
		public NumberWrapper FSaleOrgId { get; set; }

		[JsonProperty("FSaledeptid")]
		public NumberWrapper FSaledeptid { get; set; }

		[JsonProperty("FHeadLocId")]
		public NumberWrapper FHeadLocId { get; set; }

		[JsonProperty("FCorrespondOrgId")]
		public NumberWrapper FCorrespondOrgId { get; set; }

		[JsonProperty("FSaleGroupId")]
		public NumberWrapper FSaleGroupId { get; set; }

		[JsonProperty("FRetorgId")]
		public NumberWrapper FRetorgId { get; set; }

		[JsonProperty("FRetDeptId")]
		public NumberWrapper FRetDeptId { get; set; }

		[JsonProperty("FStockerGroupId")]
		public NumberWrapper FStockerGroupId { get; set; }

		[JsonProperty("FStockerId")]
		public NameWrapper FStockerId { get; set; }

		[JsonProperty("FDescription")]
		public string FDescription { get; set; }

		[JsonProperty("FReceiveCusId")]
		public NumberWrapper FReceiveCusId { get; set; }

		[JsonProperty("FReceiveAddress")]
		public string FReceiveAddress { get; set; }

		[JsonProperty("FSettleCusId")]
		public NumberWrapper FSettleCusId { get; set; }

		[JsonProperty("FReceiveCusContact")]
		public NameWrapper FReceiveCusContact { get; set; }

		[JsonProperty("FPayCusId")]
		public NumberWrapper FPayCusId { get; set; }

		[JsonProperty("FOwnerTypeIdHead")]
		public string FOwnerTypeIdHead { get; set; }

		[JsonProperty("FOwnerIdHead")]
		public NumberWrapper FOwnerIdHead { get; set; }

		[JsonProperty("SubHeadEntity")]
		public SubHeadEntity SubHeadEntity { get; set; }
	}

	public class SubHeadEntity {
		[JsonProperty("FEntryId")]
		public int FEntryId { get; set; }

		[JsonProperty("FSettleCurrId")]
		public NumberWrapper FSettleCurrId { get; set; }

		[JsonProperty("FSettleOrgId")]
		public NumberWrapper FSettleOrgId { get; set; }

		[JsonProperty("FChageCondition")]
		public NumberWrapper FChageCondition { get; set; }

		[JsonProperty("FSettleTypeId")]
		public NumberWrapper FSettleTypeId { get; set; }

		[JsonProperty("FLocalCurrId")]
		public NumberWrapper FLocalCurrId { get; set; }

		[JsonProperty("FExchangeTypeId")]
		public NumberWrapper FExchangeTypeId { get; set; }

		[JsonProperty("FExchangeRate")]
		public int FExchangeRate { get; set; }
	}

	public class FAuxpropId { }

	public class FTaxDetailSubEntity {
		[JsonProperty("FDetailID")]
		public int FDetailID { get; set; }
	}
}