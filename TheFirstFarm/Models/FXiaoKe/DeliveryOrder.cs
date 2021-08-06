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
	///     发货单
	/// </summary>
	[Model("object_q4iWp__c", Custom = true)]
	public class DeliveryOrder : CrmModelBase {
		/// <summary>
		///     单据编号
		/// </summary>
		[JsonProperty("name")]
		[MainField]
		[Required]
		public string Number { get; set; }

		/// <summary>
		///     ERP出库单号
		/// </summary>
		[JsonProperty("field_zoe2r__c")]
		public string KingdeeNumber { get; set; }

		/// <summary>
		///     单据类型
		/// </summary>
		[JsonProperty("field_6c8Jk__c")]
		[JsonConverter(typeof(NullableEnumValueConverter), Platform.FXiaoKe)]
		public DeliveryOrderType? OrderType { get; set; }

		/// <summary>
		///     关联客户
		/// </summary>
		[JsonProperty("field_w5851__c")]
		[ForeignKey(typeof(Customer))]
		public string CustomerId { get; set; }

		/// <summary>
		///     日期
		/// </summary>
		[JsonProperty("field_q5ye0__c")]
		[JsonConverter(typeof(NullableConverter<TimestampConverter>))]
		public DateTime? Date { get; set; }

		/// <summary>
		///     关联销售订单
		/// </summary>
		[JsonProperty("field_Mf8o8__c")]
		[ForeignKey(typeof(SalesOrder))]
		public string SalesOrderId { get; set; }

		[SubModel(ReverseKeyName = nameof(LogisticsInfo.DeliveryOrderId))]
		public List<LogisticsInfo> Logistics { get; set; }

		[SubModel]
		public List<DeliveryOrderDetail> Details { get; set; }
	}
}