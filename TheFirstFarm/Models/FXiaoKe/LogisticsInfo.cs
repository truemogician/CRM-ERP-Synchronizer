// ReSharper disable StringLiteralTypo
using System;
using System.ComponentModel.DataAnnotations;
using FXiaoKe.Models;
using Newtonsoft.Json;
using Shared.Serialization;

namespace TheFirstFarm.Models.FXiaoKe {
	[Model("object_S6U1J__c", Custom = true)]
	public class LogisticsInfo : CrmModelBase {
		/// <summary>
		/// 	单据编号
		/// </summary>
		[JsonProperty("name")]
		[MainField]
		[RegularExpression(@"WL\d{8}-\d{3,}")]
		[Generated]
		public string Number { get; set; }

		/// <summary>
		/// 	金蝶物流 ID
		/// </summary>
		[JsonProperty("field_flnm__c")]
		public int KingdeeId { get; set; }

		/// <summary>
		/// 	关联发货单
		/// </summary>
		[JsonProperty("field_359fr__c")]
		[MasterKey(typeof(DeliveryOrder))]
		[Required]
		public string DeliveryOrderId { get; set; }

		/// <summary>
		/// 	发货时间
		/// </summary>
		[JsonProperty("field_qRi4J__c")]
		[JsonConverter(typeof(NullableConverter<TimestampConverter>))]
		public DateTime? DeliveryTime { get; set; }

		/// <summary>
		/// 	物流公司
		/// </summary>
		[JsonProperty("field_Fo22X__c")]
		public string Company { get; set; }

		/// <summary>
		/// 	物流单号
		/// </summary>
		[JsonProperty("field_B2pcm__c")]
		public string WaybillNumber { get; set; }

		/// <summary>
		/// 	物流状态
		/// </summary>
		[JsonProperty("field_ssac9__c")]
		public string Status { get; set; }
	}
}