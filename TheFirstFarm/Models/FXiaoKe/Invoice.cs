using System;
using System.ComponentModel.DataAnnotations;
using FXiaoKe.Models;
using Newtonsoft.Json;
using Shared.Serialization;

namespace TheFirstFarm.Models.FXiaoKe {
	[Model("object_TBoMu__c")]
	public class Invoice : CrmModelBase {
		/// <summary>
		///     单据编号
		/// </summary>
		[JsonProperty("name")]
		[MainField]
		[Required]
		public string Number { get; set; }

		/// <summary>
		///     应收金额
		/// </summary>
		[JsonProperty("field_kbe2K__c")]
		public decimal? Money { get; set; }

		/// <summary>
		///     开票日期
		/// </summary>
		[JsonProperty("field_l20lY__c")]
		public DateTime? Date { get; set; }

		/// <summary>
		///     加锁人
		/// </summary>
		[JsonProperty("lock_user")]
		[JsonConverter(typeof(ArrayWrapperConverter))]
		[ForeignKey(typeof(Staff))]
		public string Locker { get; set; }

		/// <summary>
		///     发票号
		/// </summary>
		[JsonProperty("field_7jwLt__c")]
		public string InvoiceNumber { get; set; }

		/// <summary>
		///     单据类型
		/// </summary>
		[JsonProperty("field_PzFui__c")]
		public InvoiceType? InvoiceType { get; set; }

		/// <summary>
		///     销售订单
		/// </summary>
		[JsonProperty("field_EI2oj__c")]
		[ForeignKey(typeof(SalesOrder))]
		public string SalesOrderId { get; set; }

		/// <summary>
		///     客户
		/// </summary>
		[JsonProperty("field_s15bl__c")]
		[ForeignKey(typeof(Customer))]
		public string CustomerId { get; set; }
	}

	[JsonConverter(typeof(EnumValueConverter))]
	public enum InvoiceType {
		/// <summary>
		///     标准应收单
		/// </summary>
		[EnumValue("Qj5d85aHB")]
		Standard,

		/// <summary>
		///     费用应收单
		/// </summary>
		[EnumValue("C57227swl")]
		Expense,

		/// <summary>
		///     资产应收单
		/// </summary>
		[EnumValue("aZD7X8z02")]
		Asset,

		/// <summary>
		///     转销应收单
		/// </summary>
		[EnumValue("0QY84t0z9")]
		Resale,

		/// <summary>
		///     其他
		/// </summary>
		[EnumValue("other")]
		Other
	}
}