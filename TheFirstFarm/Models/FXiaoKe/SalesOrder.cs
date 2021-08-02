// ReSharper disable StringLiteralTypo
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FXiaoKe.Models;
using Newtonsoft.Json;
using Shared.Serialization;
using TheFirstFarm.Models.Common;

namespace TheFirstFarm.Models.FXiaoKe {
	/// <summary>
	///     销售订单
	/// </summary>
	[Model("SalesOrderObj")]
	public class SalesOrder : CrmModelBase {
		/// <summary>
		///     订单号
		/// </summary>
		[JsonProperty("field_D1Q3T__c")]
		[MainField]
		[RegularExpression(@"DD-\d{12,}")]
		[Required]
		public string Number { get; set; }

		/// <summary>
		///     单据类型
		/// </summary>
		[JsonProperty("field_6Xyq0__c")]
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
		public DateTime? DeliveryTime { get; set; }

		/// <summary>
		///     物流状态
		/// </summary>
		[JsonProperty("field_ij8tb__c")]
		public string LogisticsStatus { get; set; }

		/// <summary>
		///     订单产品
		/// </summary>
		[JsonIgnore]
		[SubModel(Eager = true, Cascade = true)]
		public List<SalesOrderProduct> Products { get; set; }
	}

	[JsonConverter(typeof(EnumValueConverter))]
	public enum SalesOrderType {
		/// <summary>
		///     标准销售订单
		/// </summary>
		[EnumValue("m45a848H2")]
		Standard,

		/// <summary>
		///     寄售销售订单
		/// </summary>
		[EnumValue("DoB9s22iK")]
		Consignment,

		/// <summary>
		///     受拖销售订单
		/// </summary>
		[EnumValue("Cyj781h4i")]
		Commissioned,

		/// <summary>
		///     直运销售订单
		/// </summary>
		[EnumValue("3Isyf25u3")]
		DirectShipment,

		/// <summary>
		///     退货订单
		/// </summary>
		[EnumValue("Xf5f90jGJ")]
		Return,

		/// <summary>
		///     分销调拨订单
		/// </summary>
		[EnumValue("l4YLa197k")]
		DistributionTransfer,

		/// <summary>
		///     分销购销订单
		/// </summary>
		[EnumValue("860L0Uolk")]
		Distribution,

		/// <summary>
		///     VMI销售订单
		/// </summary>
		[EnumValue("bTg34g7n5")]
		Vmi,

		/// <summary>
		///     现销订单
		/// </summary>
		[EnumValue("g9pCW5lM1")]
		Cash,

		/// <summary>
		///     其他
		/// </summary>
		[EnumValue("other")]
		Other
	}
}