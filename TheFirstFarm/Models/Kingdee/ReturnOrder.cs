using System.Collections.Generic;
using Newtonsoft.Json;

namespace TheFirstFarm.Models.Kingdee {
	public class FBillTypeID {
		[JsonProperty("FNUMBER")]
		public string FNUMBER { get; set; }
	}

	public class FSaleOrgId {
		[JsonProperty("FNumber")]
		public string FNumber { get; set; }
	}

	public class FRetcustId {
		[JsonProperty("FNumber")]
		public string FNumber { get; set; }
	}

	public class FSaledeptid {
		[JsonProperty("FNumber")]
		public string FNumber { get; set; }
	}

	public class FReturnReason {
		[JsonProperty("FNumber")]
		public string FNumber { get; set; }
	}

	public class FHeadLocId {
		[JsonProperty("FNumber")]
		public string FNumber { get; set; }
	}

	public class FCorrespondOrgId {
		[JsonProperty("FNumber")]
		public string FNumber { get; set; }
	}

	public class FSaleGroupId {
		[JsonProperty("FNumber")]
		public string FNumber { get; set; }
	}

	public class FSalesManId {
		[JsonProperty("FNumber")]
		public string FNumber { get; set; }
	}

	public class FRetorgId {
		[JsonProperty("FNumber")]
		public string FNumber { get; set; }
	}

	public class FRetDeptId {
		[JsonProperty("FNumber")]
		public string FNumber { get; set; }
	}

	public class FStockerGroupId {
		[JsonProperty("FNumber")]
		public string FNumber { get; set; }
	}

	public class FStockerId {
		[JsonProperty("FNAME")]
		public string FNAME { get; set; }
	}

	public class FReceiveCusId {
		[JsonProperty("FNumber")]
		public string FNumber { get; set; }
	}

	public class FSettleCusId {
		[JsonProperty("FNumber")]
		public string FNumber { get; set; }
	}

	public class FReceiveCusContact {
		[JsonProperty("FNAME")]
		public string FNAME { get; set; }
	}

	public class FPayCusId {
		[JsonProperty("FNumber")]
		public string FNumber { get; set; }
	}

	public class FOwnerIdHead {
		[JsonProperty("FNumber")]
		public string FNumber { get; set; }
	}

	public class FSettleCurrId {
		[JsonProperty("FNumber")]
		public string FNumber { get; set; }
	}

	public class FSettleOrgId {
		[JsonProperty("FNumber")]
		public string FNumber { get; set; }
	}

	public class FChageCondition {
		[JsonProperty("FNumber")]
		public string FNumber { get; set; }
	}

	public class FSettleTypeId {
		[JsonProperty("FNumber")]
		public string FNumber { get; set; }
	}

	public class FLocalCurrId {
		[JsonProperty("FNumber")]
		public string FNumber { get; set; }
	}

	public class FExchangeTypeId {
		[JsonProperty("FNumber")]
		public string FNumber { get; set; }
	}

	public class SubHeadEntity {
		[JsonProperty("FEntryId")]
		public int FEntryId { get; set; }

		[JsonProperty("FSettleCurrId")]
		public FSettleCurrId FSettleCurrId { get; set; }

		[JsonProperty("FSettleOrgId")]
		public FSettleOrgId FSettleOrgId { get; set; }

		[JsonProperty("FChageCondition")]
		public FChageCondition FChageCondition { get; set; }

		[JsonProperty("FSettleTypeId")]
		public FSettleTypeId FSettleTypeId { get; set; }

		[JsonProperty("FLocalCurrId")]
		public FLocalCurrId FLocalCurrId { get; set; }

		[JsonProperty("FExchangeTypeId")]
		public FExchangeTypeId FExchangeTypeId { get; set; }

		[JsonProperty("FExchangeRate")]
		public int FExchangeRate { get; set; }
	}

	public class FMapId {
		[JsonProperty("FNumber")]
		public string FNumber { get; set; }
	}

	public class FMaterialId {
		[JsonProperty("FNumber")]
		public string FNumber { get; set; }
	}

	public class FAuxpropId { }

	public class FParentMatId {
		[JsonProperty("FNUMBER")]
		public string FNUMBER { get; set; }
	}

	public class FUnitID {
		[JsonProperty("FNumber")]
		public string FNumber { get; set; }
	}

	public class FLot {
		[JsonProperty("FNumber")]
		public string FNumber { get; set; }
	}

	public class FASEUNITID {
		[JsonProperty("FNumber")]
		public string FNumber { get; set; }
	}

	public class FStockId {
		[JsonProperty("FNumber")]
		public string FNumber { get; set; }
	}

	public class FBOMId {
		[JsonProperty("FNumber")]
		public string FNumber { get; set; }
	}

	public class FRmType {
		[JsonProperty("FNumber")]
		public string FNumber { get; set; }
	}

	public class FStockUnitID {
		[JsonProperty("FNumber")]
		public string FNumber { get; set; }
	}

	public class FOwnerID {
		[JsonProperty("FNumber")]
		public string FNumber { get; set; }
	}

	public class FTaxDetailSubEntity {
		[JsonProperty("FDetailID")]
		public int FDetailID { get; set; }
	}

	public class FEntity {
		[JsonProperty("FENTRYID")]
		public int FENTRYID { get; set; }

		[JsonProperty("FRowType")]
		public string FRowType { get; set; }

		[JsonProperty("FMapId")]
		public FMapId FMapId { get; set; }

		[JsonProperty("FMaterialId")]
		public FMaterialId FMaterialId { get; set; }

		[JsonProperty("FAuxpropId")]
		public FAuxpropId FAuxpropId { get; set; }

		[JsonProperty("FParentMatId")]
		public FParentMatId FParentMatId { get; set; }

		[JsonProperty("FUnitID")]
		public FUnitID FUnitID { get; set; }

		[JsonProperty("FInventoryQty")]
		public int FInventoryQty { get; set; }

		[JsonProperty("FQty")]
		public int FQty { get; set; }

		[JsonProperty("FPRODUCEDATE")]
		public string FPRODUCEDATE { get; set; }

		[JsonProperty("FExpiryDate")]
		public string FExpiryDate { get; set; }

		[JsonProperty("FLot")]
		public FLot FLot { get; set; }

		[JsonProperty("FPriceBaseQty")]
		public int FPriceBaseQty { get; set; }

		[JsonProperty("FASEUNITID")]
		public FASEUNITID FASEUNITID { get; set; }

		[JsonProperty("FDeliverydate")]
		public string FDeliverydate { get; set; }

		[JsonProperty("FStockId")]
		public FStockId FStockId { get; set; }

		[JsonProperty("FBOMId")]
		public FBOMId FBOMId { get; set; }

		[JsonProperty("FMtoNo")]
		public string FMtoNo { get; set; }

		[JsonProperty("FEntryDescription")]
		public string FEntryDescription { get; set; }

		[JsonProperty("FPriceDiscount")]
		public int FPriceDiscount { get; set; }

		[JsonProperty("FRmType")]
		public FRmType FRmType { get; set; }

		[JsonProperty("FIsReturnCheck")]
		public string FIsReturnCheck { get; set; }

		[JsonProperty("FCheckQty")]
		public int FCheckQty { get; set; }

		[JsonProperty("FBaseCheckQty")]
		public int FBaseCheckQty { get; set; }

		[JsonProperty("FQualifiedQty")]
		public int FQualifiedQty { get; set; }

		[JsonProperty("FBaseQualifiedQty")]
		public int FBaseQualifiedQty { get; set; }

		[JsonProperty("FUnqualifiedQty")]
		public int FUnqualifiedQty { get; set; }

		[JsonProperty("FBaseUnqualifiedQty")]
		public int FBaseUnqualifiedQty { get; set; }

		[JsonProperty("FJunkedQty")]
		public int FJunkedQty { get; set; }

		[JsonProperty("FBaseJunkedQty")]
		public int FBaseJunkedQty { get; set; }

		[JsonProperty("FJoinCheckQty")]
		public int FJoinCheckQty { get; set; }

		[JsonProperty("FBaseJoinCheckQty")]
		public int FBaseJoinCheckQty { get; set; }

		[JsonProperty("FJoinQualifiedQty")]
		public int FJoinQualifiedQty { get; set; }

		[JsonProperty("FBaseJoinQualifiedQty")]
		public int FBaseJoinQualifiedQty { get; set; }

		[JsonProperty("FJoinJunkedQty")]
		public int FJoinJunkedQty { get; set; }

		[JsonProperty("FBaseJoinJunkedQty")]
		public int FBaseJoinJunkedQty { get; set; }

		[JsonProperty("FJoinUnqualifiedQty")]
		public int FJoinUnqualifiedQty { get; set; }

		[JsonProperty("FBaseJoinUnqualifiedQty")]
		public int FBaseJoinUnqualifiedQty { get; set; }

		[JsonProperty("FStockUnitID")]
		public FStockUnitID FStockUnitID { get; set; }

		[JsonProperty("FStockQty")]
		public int FStockQty { get; set; }

		[JsonProperty("FStockBaseQty")]
		public int FStockBaseQty { get; set; }

		[JsonProperty("FOwnerTypeID")]
		public string FOwnerTypeID { get; set; }

		[JsonProperty("FOwnerID")]
		public FOwnerID FOwnerID { get; set; }

		[JsonProperty("FRefuseFlag")]
		public string FRefuseFlag { get; set; }

		[JsonProperty("FSOEntryId")]
		public int FSOEntryId { get; set; }

		[JsonProperty("FTaxDetailSubEntity")]
		public List<FTaxDetailSubEntity> FTaxDetailSubEntity { get; set; }
	}

	public class Model {
		[JsonProperty("FID")]
		public int FID { get; set; }

		[JsonProperty("FBillTypeID")]
		public FBillTypeID FBillTypeID { get; set; }

		[JsonProperty("FBillNo")]
		public string FBillNo { get; set; }

		[JsonProperty("FDate")]
		public string FDate { get; set; }

		[JsonProperty("FSaleOrgId")]
		public FSaleOrgId FSaleOrgId { get; set; }

		[JsonProperty("FRetcustId")]
		public FRetcustId FRetcustId { get; set; }

		[JsonProperty("FSaledeptid")]
		public FSaledeptid FSaledeptid { get; set; }

		[JsonProperty("FReturnReason")]
		public FReturnReason FReturnReason { get; set; }

		[JsonProperty("FHeadLocId")]
		public FHeadLocId FHeadLocId { get; set; }

		[JsonProperty("FCorrespondOrgId")]
		public FCorrespondOrgId FCorrespondOrgId { get; set; }

		[JsonProperty("FSaleGroupId")]
		public FSaleGroupId FSaleGroupId { get; set; }

		[JsonProperty("FSalesManId")]
		public FSalesManId FSalesManId { get; set; }

		[JsonProperty("FRetorgId")]
		public FRetorgId FRetorgId { get; set; }

		[JsonProperty("FRetDeptId")]
		public FRetDeptId FRetDeptId { get; set; }

		[JsonProperty("FStockerGroupId")]
		public FStockerGroupId FStockerGroupId { get; set; }

		[JsonProperty("FStockerId")]
		public FStockerId FStockerId { get; set; }

		[JsonProperty("FDescription")]
		public string FDescription { get; set; }

		[JsonProperty("FReceiveCusId")]
		public FReceiveCusId FReceiveCusId { get; set; }

		[JsonProperty("FReceiveAddress")]
		public string FReceiveAddress { get; set; }

		[JsonProperty("FSettleCusId")]
		public FSettleCusId FSettleCusId { get; set; }

		[JsonProperty("FReceiveCusContact")]
		public FReceiveCusContact FReceiveCusContact { get; set; }

		[JsonProperty("FPayCusId")]
		public FPayCusId FPayCusId { get; set; }

		[JsonProperty("FOwnerTypeIdHead")]
		public string FOwnerTypeIdHead { get; set; }

		[JsonProperty("FOwnerIdHead")]
		public FOwnerIdHead FOwnerIdHead { get; set; }

		[JsonProperty("SubHeadEntity")]
		public SubHeadEntity SubHeadEntity { get; set; }

		[JsonProperty("FEntity")]
		public List<FEntity> FEntity { get; set; }
	}
}