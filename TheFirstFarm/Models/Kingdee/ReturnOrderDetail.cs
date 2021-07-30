// ReSharper disable StringLiteralTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
using System.ComponentModel.DataAnnotations;
using Kingdee.Forms;
using Newtonsoft.Json;
using Shared.Serialization;
using TheFirstFarm.Models.Common;

namespace TheFirstFarm.Models.Kingdee {
	[Form()]//TODO
	public class ReturnOrderDetail : FormBase {
		/// <summary>
		///     物料编码
		/// </summary>
		[JsonProperty("FMaterialId")]
		[Required]
		public NumberWrapper MaterialNumber { get; set; }

		/// <summary>
		///     物料名称
		/// </summary>
		//[JsonProperty("FMaterialName")]
		//[JsonInclude]
		//[Required]
		//public string MaterialName { get; set; }

		/// <summary>
		///     规格型号
		/// </summary>
		//[JsonProperty("FMaterialModel")]
		//[JsonInclude]
		//public string MaterialModel { get; set; }

		/// <summary>
		///     销售单位
		/// </summary>
		//[JsonProperty("FUnitID")]
		//[JsonInclude]
		//[Required]
		//public NumberWrapper SaleUnit { get; set; }

		/// <summary>
		///     实退数量
		/// </summary>
		[JsonProperty("FREALQTY")]
		[Required]
		public decimal ReturnAmount { get; set; }

		/// <summary>
		///     含税单价
		/// </summary>
		[JsonProperty("FTAXPRICE")]
		public decimal UnitPrice { get; set; }

		/// <summary>
		///     税率
		/// </summary>
		[JsonProperty("FTAXRATE")]
		public decimal TaxRate { get; set; }

		/// <summary>
		///     金额
		/// </summary>
		[JsonProperty("FAMOUNT")]
		public decimal Money { get; set; }

		/// <summary>
		///     退货类型
		/// </summary>
		[JsonProperty("FReturnType")]
		[Required]
		public ReturnTypeWrapper ReturnType { get; set; }
	}

	public class ReturnTypeWrapper : WrapperBase<ReturnType> {
		[JsonProperty("FNumber")]
		[JsonConverter(typeof(EnumValueConverter), Platform.Kingdee)]
		public ReturnType Value { get; set; }

		protected override string ValueName => nameof(Value);
	}
}