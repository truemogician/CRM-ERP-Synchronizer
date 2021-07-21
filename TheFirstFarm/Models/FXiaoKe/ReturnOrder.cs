using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FXiaoKe.Models;
using Newtonsoft.Json;
using Shared.Serialization;
using TheFirstFarm.Models.Common;

namespace TheFirstFarm.Models.FXiaoKe {
	[Model("object_4qZkc__c", Custom = true)]
	public class ReturnOrder : ModelBase {
		/// <summary>
		///     单据编号
		/// </summary>
		[JsonProperty("name")]
		[Key]
		[Required]
		public string Id { get; set; }

		/// <summary>
		///     日期
		/// </summary>
		[JsonProperty("field_6V5Ls__c")]
		[JsonConverter(typeof(TimestampConverter))]
		public DateTime Date { get; set; }

		/// <summary>
		///     退货客户
		/// </summary>
		[JsonProperty("field_nlvAs__c")]
		[ForeignKey(typeof(Customer))]
		[Required]
		public string CustomerId { get; set; }

		/// <summary>
		///     负责人
		/// </summary>
		[JsonProperty("owner")]
		[JsonConverter(typeof(ArrayWrapperConverter<string>))]
		[ForeignKey(typeof(Staff))]
		[Required]
		public string OwnerId { get; set; }

		/// <summary>
		///     业务类型（erp）
		/// </summary>
		[JsonProperty("field_2imfP__c")]
		public BusinessType BusinessType { get; set; }

		/// <summary>
		///     退货原因
		/// </summary>
		[JsonProperty("field_S1QH2__c")]
		public string Reason { get; set; }

		[JsonIgnore]
		[SubModel(Eager = true, Cascade = true)]
		public List<ReturnOrderDetail> Details { get; set; }
	}
}