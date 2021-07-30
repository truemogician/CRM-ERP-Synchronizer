// ReSharper disable StringLiteralTypo
using System.ComponentModel.DataAnnotations;
using FXiaoKe.Models;
using Newtonsoft.Json;

namespace TheFirstFarm.Models.FXiaoKe {
	/// <summary>
	///		发货单明细
	/// </summary>
	[Model("object_y7oXT__c")]
	public class DeliveryOrderDetail : CrmModelBase {
		/// <summary>
		/// 	发货单明细号
		/// </summary>
		[JsonProperty("name")]
		[MainField]
		[RegularExpression(@"FHMX-\d{8}-\d{3,}")]
		[Required]
		public string Number { get; set; }

		/// <summary>
		/// 	金蝶分录ID
		/// </summary>
		[JsonProperty("field_flnm__c")]
		public string KingdeeId { get; set; }

		/// <summary>
		/// 	物料编码
		/// </summary>
		[JsonProperty("field_7iuKb__c")]
		[ForeignKey(typeof(Product))]
		public string ProductId { get; set; }

		/// <summary>
		/// 	发货单
		/// </summary>
		[JsonProperty("field_Q1hWk__c")]
		[MasterKey(typeof(DeliveryOrder))]
		[Required]
		public string DeliveryOrderId { get; set; }

		/// <summary>
		/// 	应发数量
		/// </summary>
		[JsonProperty("field_3aR0e__c")]
		public decimal? ExpectedAmount { get; set; }

		/// <summary>
		/// 	实发数量
		/// </summary>
		[JsonProperty("field_c8JM4__c")]
		public decimal? ActualAmount { get; set; }
	}
}