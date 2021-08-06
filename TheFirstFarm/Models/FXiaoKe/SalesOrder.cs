// ReSharper disable StringLiteralTypo
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FXiaoKe.Models;
using Newtonsoft.Json;
using Shared.Serialization;
using Shared.Validation;
using TheFirstFarm.Models.Common;

namespace TheFirstFarm.Models.FXiaoKe {
	/// <summary>
	///     销售订单
	/// </summary>
	[Model("SalesOrderObj")]
	public class SalesOrder : CrmModelBase {
		[JsonProperty("name")]
		[MainField]
		[Generated]
		[RegularExpression(@"DD-\d{12,}")]
		public string Number { get; set; }

		/// <summary>
		///     订单号
		/// </summary>
		[JsonProperty("field_D1Q3T__c")]
		[Required]
		public string KingdeeNumber { get; set; }

		/// <summary>
		///     单据类型
		/// </summary>
		[JsonProperty("field_6Xyq0__c")]
		[JsonConverter(typeof(EnumValueConverter), Platform.FXiaoKe)]
		public SalesOrderType OrderType { get; set; }

		/// <summary>
		///     业务类型
		/// </summary>
		[JsonProperty("field_Vah6d__c")]
		public BusinessType BusinessType { get; set; }

		/// <summary>
		///     订单日期
		/// </summary>
		[JsonProperty("order_time")]
		[JsonConverter(typeof(NullableConverter<TimestampConverter>))]
		public DateTime? Date { get; set; }

		/// <summary>
		///     客户名称
		/// </summary>
		[JsonProperty("account_id")]
		[ForeignKey(typeof(Customer))]
		public string CustomerId { get; set; }

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
		[JsonConverter(typeof(NullableConverter<TimestampConverter>))]
		public DateTime? DeliveryTime { get; set; }

		/// <summary>
		///     物流状态
		/// </summary>
		[JsonProperty("field_ij8tb__c")]
		public string LogisticsStatus { get; set; }

		/// <summary>
		///     订单产品
		/// </summary>
		[SubModel]
		[CollectionMinCount(1)]
		public List<SalesOrderProduct> Products { get; set; }
	}
}