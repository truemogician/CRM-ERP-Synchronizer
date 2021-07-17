using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using FXiaoKe.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TheFirstFarm.Models.FXiaoKe {
	[Model("object_4qZkc__c", true)]
	public class ReturnOrder {
		/// <summary>
		/// 业务类型（erp）
		/// </summary>
		[JsonProperty("field_2imfP__c")]
		public BusinessType BusinessType { get; set; }

		/// <summary>
		/// 单据编号
		/// </summary>
		[JsonProperty("name")]
		[PrimaryKey]
		[Required]
		public string Id { get; set; }

		/// <summary>
		/// 日期
		/// </summary>
		[JsonProperty("field_6V5Ls__c")]
		public DateTime Date { get; set; }

		/// <summary>
		/// 退货客户
		/// </summary>
		[JsonProperty("field_nlvAs__c")]
		[ForeignKey(typeof(Customer))]
		[Required]
		public string CustomerId { get; set; }

		/// <summary>
		/// 负责人
		/// </summary>
		[JsonProperty("owner")]
		[ForeignKey(typeof(Staff))]
		[Required]
		public string ContactId { get; set; }

		/// <summary>
		/// 退货原因
		/// </summary>
		[JsonProperty("field_S1QH2__c")]
		public string Reason { get; set; }
	}

	[JsonConverter(typeof(StringEnumConverter))]
	public enum BusinessType {
		/// <summary>
		/// 普通销售
		/// </summary>
		[EnumMember(Value = "5n22nRQ2s")]
		General,

		/// <summary>
		/// 寄售
		/// </summary>
		[EnumMember(Value = "cruFbEeF0")]
		Consignment,

		/// <summary>
		/// 直运
		/// </summary>
		[EnumMember(Value = "1xt283wXn")]
		DirectShipment,

		/// <summary>
		/// 其他
		/// </summary>
		[EnumMember(Value = "other")]
		Other
	}
}