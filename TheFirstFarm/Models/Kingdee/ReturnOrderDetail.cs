// ReSharper disable StringLiteralTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Kingdee.Forms;
using Newtonsoft.Json;
using Shared.Serialization;
using TheFirstFarm.Models.Common;
using JsonConverterAttribute = Newtonsoft.Json.JsonConverterAttribute;

namespace TheFirstFarm.Models.Kingdee {
	public class ReturnOrderDetail {
		/// <summary>
		/// 物料编码
		/// </summary>
		[JsonProperty("FMaterialId")]
		[JsonInclude]
		[Required]
		public NumberWrapper MaterialId { get; set; }

		/// <summary>
		/// 物料名称
		/// </summary>
		[JsonProperty("FMaterialName")]
		[JsonInclude]
		[Required]
		public string MaterialName { get; set; }

		/// <summary>
		/// 规格型号
		/// </summary>
		[JsonProperty("FMaterialModel")]
		[JsonInclude]
		public string MaterialModel { get; set; }

		/// <summary>
		/// 销售单位
		/// </summary>
		[JsonProperty("FUnitID")]
		[JsonInclude]
		[Required]
		public NumberWrapper SaleUnit { get; set; }

		/// <summary>
		/// 实退数量
		/// </summary>
		[JsonProperty("FREALQTY")]
		[JsonInclude]
		[Required]
		public decimal ReturnAmount { get; set; }

		/// <summary>
		/// 含税单价
		/// </summary>
		[JsonProperty("FTAXPRICE")]
		[JsonInclude]
		public decimal UnitPrice { get; set; }

		/// <summary>
		/// 税率
		/// </summary>
		[JsonProperty("FTAXRATE")]
		[JsonInclude]
		public decimal TaxRate { get; set; }

		/// <summary>
		/// 金额
		/// </summary>
		[JsonProperty("FAMOUNT")]
		[JsonInclude]
		public decimal Volumn { get; set; }

		/// <summary>
		/// 退货类型
		/// </summary>
		[JsonProperty("FReturnType")]
		[JsonInclude]
		[Required]
		public ReturnTypeWrapper ReturnType { get; set; }

		[JsonProperty("FENTRYID")]
		public int FENTRYID { get; set; }

		[JsonProperty("FRowType")]
		public string FRowType { get; set; }

		[JsonProperty("FMapId")]
		public NumberWrapper FMapId { get; set; }

		[JsonProperty("FParentMatId")]
		public NumberWrapper FParentMatId { get; set; }

		[JsonProperty("FInventoryQty")]
		public int FInventoryQty { get; set; }

		[JsonProperty("FQty")]
		public int FQty { get; set; }

		[JsonProperty("FPRODUCEDATE")]
		public string FPRODUCEDATE { get; set; }

		[JsonProperty("FExpiryDate")]
		public string FExpiryDate { get; set; }

		[JsonProperty("FLot")]
		public NumberWrapper FLot { get; set; }

		[JsonProperty("FPriceBaseQty")]
		public int FPriceBaseQty { get; set; }

		[JsonProperty("FASEUNITID")]
		public NumberWrapper FASEUNITID { get; set; }

		[JsonProperty("FDeliverydate")]
		public string FDeliverydate { get; set; }

		[JsonProperty("FStockId")]
		public NumberWrapper FStockId { get; set; }

		[JsonProperty("FBOMId")]
		public NumberWrapper FBOMId { get; set; }

		[JsonProperty("FMtoNo")]
		public string FMtoNo { get; set; }

		[JsonProperty("FEntryDescription")]
		public string FEntryDescription { get; set; }

		[JsonProperty("FPriceDiscount")]
		public int FPriceDiscount { get; set; }

		[JsonProperty("FRmType")]
		public NumberWrapper FRmType { get; set; }

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
		public NumberWrapper FStockUnitID { get; set; }

		[JsonProperty("FStockQty")]
		public int FStockQty { get; set; }

		[JsonProperty("FStockBaseQty")]
		public int FStockBaseQty { get; set; }

		[JsonProperty("FOwnerTypeID")]
		public string FOwnerTypeID { get; set; }

		[JsonProperty("FOwnerID")]
		public NumberWrapper FOwnerID { get; set; }

		[JsonProperty("FRefuseFlag")]
		public string FRefuseFlag { get; set; }

		[JsonProperty("FSOEntryId")]
		public int FSOEntryId { get; set; }

		[JsonProperty("FTaxDetailSubEntity")]
		public List<FTaxDetailSubEntity> FTaxDetailSubEntity { get; set; }
	}

	public class ReturnTypeWrapper : WrapperBase<ReturnType> {
		[JsonProperty("FNumber")]
		[JsonConverter(typeof(MultipleStringEnumConverter), Platform.Kingdee)]
		public ReturnType Value { get; set; }

		protected override string ValueName => nameof(Value);
	}
}