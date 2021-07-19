// ReSharper disable StringLiteralTypo
using FXiaoKe.Models;
using Newtonsoft.Json;

namespace TheFirstFarm.Models.FXiaoKe {
	/// <summary>
	///     销售订单
	/// </summary>
	[Model("SalesOrderObj")]
	public class SalesOrder : ModelBase {
		/// <summary>
		///     单据类型
		/// </summary>
		[JsonProperty("field_6Xyq0__c")]
		public string ReceiptType { get; set; }

		/// <summary>
		///     业务类型
		/// </summary>
		[JsonProperty("field_Vah6d__c")]
		public string BusinessType { get; set; }

		/// <summary>
		///     订单号
		/// </summary>
		[JsonProperty("field_D1Q3T__c")]
		public string OrderNumber { get; set; }

		/// <summary>
		///     订单日期
		/// </summary>
		[JsonProperty("order_time")]
		public string Date { get; set; }

		/// <summary>
		///     客户名称
		/// </summary>
		[JsonProperty("account_id")]
		public string CustomerId { get; set; }

		/// <summary>
		///     负责人
		/// </summary>
		[JsonProperty("owner")]
		[ForeignKey(typeof(Staff))]
		public string OwnerId { get; set; }

		/// <summary>
		///     交货地址
		/// </summary>
		[JsonProperty("ship_to_add")]
		public string ShippingAddress { get; set; }

		/// <summary>
		///     物流公司
		/// </summary>
		[JsonProperty("field_1eGfE__c")]
		public string LogisticsCompany { get; set; }

		/// <summary>
		///     物流单号
		/// </summary>
		[JsonProperty("field_fL9kr__c")]
		public string WaybillNumber { get; set; }

		/// <summary>
		///     发货时间
		/// </summary>
		[JsonProperty("delivery_date")]
		public string DeliveryTime { get; set; }

		/// <summary>
		///     物流状态
		/// </summary>
		[JsonProperty("field_ij8tb__c")]
		public string LogisticsStatus { get; set; }
	}
}