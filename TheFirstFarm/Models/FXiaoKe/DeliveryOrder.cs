// ReSharper disable StringLiteralTypo
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FXiaoKe.Models;
using Newtonsoft.Json;
using Shared.Serialization;

namespace TheFirstFarm.Models.FXiaoKe {
	/// <summary>
	///		发货单
	/// </summary>
	[Model("object_q4iWp__c")]
	public class DeliveryOrder : CrmModelBase {
		/// <summary>
		/// 	单据编号
		/// </summary>
		[JsonProperty("name")]
		[MainField]
		[Required]
		public string Number { get; set; }

		/// <summary>
		/// 	ERP出库单号
		/// </summary>
		[JsonProperty("field_zoe2r__c")]
		public string ErpNumber { get; set; }

		/// <summary>
		/// 	单据类型
		/// </summary>
		[JsonProperty("field_6c8Jk__c")]
		public DeliveryOrderType? OrderType { get; set; }

		/// <summary>
		/// 	发货时间1
		/// </summary>
		[JsonProperty("field_602Xn__c")]
		[JsonConverter(typeof(NullableConverter<TimestampConverter, DateTime>))]
		public DateTime? DeliveryTime { get; set; }

		/// <summary>
		/// 	物流单号
		/// </summary>
		[JsonProperty("field_2V77p__c")]
		public string WaybillNumber { get; set; }

		/// <summary>
		/// 	关联客户
		/// </summary>
		[JsonProperty("field_w5851__c")]
		[ForeignKey(typeof(Customer))]
		public string CustomerId { get; set; }

		/// <summary>
		/// 	日期
		/// </summary>
		[JsonProperty("field_q5ye0__c")]
		[JsonConverter(typeof(NullableConverter<TimestampConverter, DateTime>))]
		public DateTime? Date { get; set; }

		/// <summary>
		/// 	关联销售订单
		/// </summary>
		[JsonProperty("field_Mf8o8__c")]
		[ForeignKey(typeof(SalesOrder))]
		public string SalesOrderId { get; set; }

		/// <summary>
		/// 	物流公司1
		/// </summary>
		[JsonProperty("field_8h68m__c")]
		public string LogisticsCompany { get; set; }

		/// <summary>
		/// 	物流状态1
		/// </summary>
		[JsonProperty("field_cNeoe__c")]
		public string LogisticsStatus { get; set; }

		[JsonIgnore]
		[SubModel(Eager = true, Cascade = true)]
		public List<DeliveryOrderDetail> Details { get; set; }
	}

	[JsonConverter(typeof(EnumValueConverter))]
	public enum DeliveryOrderType {
		/// <summary>
		/// 	标准销售出库单
		/// </summary>
		[EnumValue("9u11w22Xu")]
		Standard,

		/// <summary>
		/// 	寄售出库单
		/// </summary>
		[EnumValue("2xJNsq93O")]
		Consignment,

		/// <summary>
		/// 	零售出库单
		/// </summary>
		[EnumValue("3lZ48u49w")]
		Retail,

		/// <summary>
		/// 	分销购销销售出库单
		/// </summary>
		[EnumValue("sE5zk16oe")]
		Distribution,

		/// <summary>
		/// 	VMI出库单
		/// </summary>
		[EnumValue("9CcWD41O3")]
		Vmi,

		/// <summary>
		/// 	现销出库单
		/// </summary>
		[EnumValue("Fp2pooreH")]
		Cash,

		/// <summary>
		/// 	B2C销售出库单
		/// </summary>
		[EnumValue("wgXQPX32z")]
		B2C,

		/// <summary>
		/// 	其他
		/// </summary>
		[EnumValue("other")]
		Other
	}
}