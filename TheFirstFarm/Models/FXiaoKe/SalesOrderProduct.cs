﻿// ReSharper disable StringLiteralTypo
using System;
using System.ComponentModel.DataAnnotations;
using FXiaoKe.Models;
using Newtonsoft.Json;
using Shared.Serialization;

namespace TheFirstFarm.Models.FXiaoKe {
	/// <summary>
	///     订单产品
	/// </summary>
	[Model("SalesOrderProductObj")]
	public class SalesOrderProduct : CrmModelBase {
		/// <summary>
		///		订单产品编号
		/// </summary>
		[JsonProperty("name")]
		[MainField]
		[RegularExpression(@"\d{12,}")]
		[Required]
		public string Number { get; set; }

		/// <summary>
		///     物料编码
		/// </summary>
		[JsonProperty("product_id")]
		[ForeignKey(typeof(Product))]
		[Required]
		public string ProductId { get; set; }

		[JsonProperty("order_id")]
		[MasterKey(typeof(SalesOrder))]
		[Required]
		public string SalesOrderId { get; set; }

		/// <summary>
		///     数量
		/// </summary>
		[JsonProperty("quantity")]
		public decimal Quantity { get; set; }

		/// <summary>
		///     含税单价
		/// </summary>
		[JsonProperty("sales_price")]
		public decimal Price { get; set; }

		/// <summary>
		///     税率
		/// </summary>
		[JsonProperty("field_q9GkX__c")]
		public decimal TaxRate { get; set; }

		/// <summary>
		///     金额
		/// </summary>
		[JsonProperty("field_y66kY__c")]
		public decimal Money { get; set; }

		/// <summary>
		///     要货日期
		/// </summary>
		[JsonProperty("field_hvdcm__c")]
		[JsonConverter(typeof(NullableConverter<TimestampConverter>))]
		public DateTime? RequestDate { get; set; }

		/// <summary>
		///     备注
		/// </summary>
		[JsonProperty("remark")]
		public string Remark { get; set; }
	}
}