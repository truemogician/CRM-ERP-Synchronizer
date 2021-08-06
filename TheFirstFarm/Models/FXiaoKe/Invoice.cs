using System;
using System.ComponentModel.DataAnnotations;
using FXiaoKe.Models;
using Newtonsoft.Json;
using Shared.Serialization;
using TheFirstFarm.Models.Common;

namespace TheFirstFarm.Models.FXiaoKe {
	[Model("object_TBoMu__c", Custom = true)]
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
		[JsonConverter(typeof(NullableConverter<TimestampConverter>))]
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
		[JsonConverter(typeof(EnumValueConverter), Platform.FXiaoKe)]
		public InvoiceType BillType { get; set; }

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
}