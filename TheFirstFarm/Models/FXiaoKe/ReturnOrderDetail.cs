using System.ComponentModel.DataAnnotations;
using FXiaoKe.Models;
using Newtonsoft.Json;
using Shared.Serialization;
using TheFirstFarm.Models.Common;

namespace TheFirstFarm.Models.FXiaoKe {
	[Model("object_wVB1X__c", Custom = true)]
	public class ReturnOrderDetail : CrmModelBase {
		/// <summary>
		///     退换货明细编号
		/// </summary>
		[JsonProperty("name")]
		[MainField]
		[Required]
		public string Number { get; set; }

		[JsonProperty("field_1b2Y1__c")]
		[MasterKey(typeof(ReturnOrder))]
		[Required]
		public string ReturnOrderId { get; set; }

		/// <summary>
		///     物料编码
		/// </summary>
		[JsonProperty("field_iflIH__c")]
		[ForeignKey(typeof(Product))]
		[Required]
		public string ProductId { get; set; }

		/// <summary>
		///     物料名称
		/// </summary>
		//[JsonProperty("field_7sq4o__c")]
		//public string ProductName { get; set; }

		/// <summary>
		///     规格型号
		/// </summary>
		//[JsonProperty("field_7L1DK__c")]
		//public string Specification { get; set; }

		/// <summary>
		///     销售单位
		/// </summary>
		//[JsonProperty("field_t6f4r__c")]
		//public string SaleUnit { get; set; }

		/// <summary>
		///     实退数量
		/// </summary>
		[JsonProperty("field_88MUb__c")]
		public decimal ReturnAmount { get; set; }

		/// <summary>
		///     含税单价
		/// </summary>
		[JsonProperty("field_M180p__c")]
		public decimal UnitPrice { get; set; }

		/// <summary>
		///     税率
		/// </summary>
		[JsonProperty("field_11B09__c")]
		public decimal TaxRate { get; set; }

		/// <summary>
		///     金额
		/// </summary>
		[JsonProperty("field_LKIOp__c")]
		public decimal Volume { get; set; }

		/// <summary>
		///     退货类型
		/// </summary>
		[JsonProperty("field_0v7Wa__c")]
		[JsonConverter(typeof(EnumValueConverter), Platform.FXiaoKe)]
		[Required]
		public ReturnType ReturnType { get; set; }
	}
}