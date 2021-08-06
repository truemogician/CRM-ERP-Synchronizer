using System;
using System.ComponentModel.DataAnnotations;
using Kingdee.Forms;
using Newtonsoft.Json;

namespace TheFirstFarm.Models.Kingdee {
	public class SalesOrderMaterial : ModelBase {
		/// <summary>
		/// 	物料编码
		/// </summary>
		[JsonProperty("FMaterialId")]
		[Required]
		public NumberWrapper MaterialNumber { get; set; }

		/// <summary>
		/// 	销售数量
		/// </summary>
		[JsonProperty("FQty")]
		[Required]
		public decimal Quantity { get; set; }

		/// <summary>
		/// 	含税单价
		/// </summary>
		[JsonProperty("FTaxPrice")]
		public decimal UnitPrice { get; set; }

		/// <summary>
		/// 	税率
		/// </summary>
		[JsonProperty("FEntryTaxRate")]
		public decimal TaxRate { get; set; }

		/// <summary>
		/// 	金额
		/// </summary>
		[JsonProperty("FAmount")]

		public decimal Money { get; set; }

		/// <summary>
		/// 	要货日期
		/// </summary>
		[JsonProperty("FDeliveryDate")]
		[Required]
		public DateTime RequestDate { get; set; }

		/// <summary>
		/// 	备注
		/// </summary>
		[JsonProperty("FRemark")]
		public string Remark { get; set; }
	}
}