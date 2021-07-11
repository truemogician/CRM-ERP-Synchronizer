using FXiaoKe.Utilities;
using Newtonsoft.Json;

namespace FXiaoKe.Models {
	/// <summary>
	/// 订单产品
	/// </summary>
	[Model("SalesOrderProductObj")]
	public class SalesOrderProduct {
		/// <summary>
		/// 物料编码
		/// </summary>
		[JsonProperty("product_id")]
		public string Id { get; set; }

		/// <summary>
		/// 物料名称
		/// </summary>
		[JsonProperty("field_cpaob__c")]
		public string Name { get; set; }

		/// <summary>
		/// 规格型号
		/// </summary>
		[JsonProperty("field_80y96__c")]
		public string Model { get; set; }

		/// <summary>
		/// 计量单位
		/// </summary>
		[JsonProperty("field_d767S__c")]
		public string Unit { get; set; }

		/// <summary>
		/// 数量
		/// </summary>
		[JsonProperty("quantity")]
		public string Quantity { get; set; }

		/// <summary>
		/// 含税单价
		/// </summary>
		[JsonProperty("sales_price")]
		public string Price { get; set; }

		/// <summary>
		/// 税率
		/// </summary>
		[JsonProperty("field_q9GkX__c")]
		public string TaxRate { get; set; }

		/// <summary>
		/// 金额
		/// </summary>
		[JsonProperty("field_y66kY__c")]
		public string Volume { get; set; }

		/// <summary>
		/// 要货日期
		/// </summary>
		[JsonProperty("field_hvdcm__c")]
		public string RequestDate { get; set; }

		/// <summary>
		/// 备注
		/// </summary>
		[JsonProperty("remark")]
		public string Remark { get; set; }

		/// <summary>
		/// 负责人
		/// </summary>
		[JsonProperty("owner")]
		public string Director { get; set; }
	}
}