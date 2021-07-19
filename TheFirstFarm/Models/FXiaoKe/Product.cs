// ReSharper disable StringLiteralTypo
using FXiaoKe.Models;
using Newtonsoft.Json;

namespace TheFirstFarm.Models.FXiaoKe {
	/// <summary>
	///     产品
	/// </summary>
	[Model("ProductObj")]
	public class Product : ModelBase {
		/// <summary>
		///     产品名称
		/// </summary>
		[JsonProperty("product_code")]
		public string Name { get; set; }

		/// <summary>
		///     产品编码
		/// </summary>
		[JsonProperty("name")]
		public string Id { get; set; }

		/// <summary>
		///     规格属性
		/// </summary>
		[JsonProperty("product_spec")]
		public string Specification { get; set; }

		/// <summary>
		///     条形码
		/// </summary>
		[JsonProperty("barcode")]
		public string BarCode { get; set; }

		/// <summary>
		///     分类
		/// </summary>
		[JsonProperty("category")]
		public string Category { get; set; }

		/// <summary>
		///     物料属性
		/// </summary>
		[JsonProperty("field_WzGGe__c")]
		public string Property { get; set; }

		/// <summary>
		///     计量单位
		/// </summary>
		[JsonProperty("field_2gj87__c")]
		public string Unit { get; set; }

		/// <summary>
		///     长
		/// </summary>
		[JsonProperty("field_9M1ng__c")]
		public string Length { get; set; }

		/// <summary>
		///     宽
		/// </summary>
		[JsonProperty("field_kKc10__c")]
		public string Width { get; set; }

		/// <summary>
		///     高
		/// </summary>
		[JsonProperty("field_G2e0j__c")]
		public string Height { get; set; }

		/// <summary>
		///     保质期单位
		/// </summary>
		[JsonProperty("field_a6e61__c")]
		public string ShelfLifeUnit { get; set; }

		/// <summary>
		///     保质期
		/// </summary>
		[JsonProperty("field_rwK57__c")]
		public string ShelfLife { get; set; }

		/// <summary>
		///     起订量
		/// </summary>
		[JsonProperty("field_BN728__c")]
		public string MinimumOrderQuantity { get; set; }

		/// <summary>
		///     允许退货
		/// </summary>
		[JsonProperty("field_15ox1__c")]
		public string AllowReturn { get; set; }

		/// <summary>
		///     产品图片
		/// </summary>
		[JsonProperty("picture_path")]
		public string Picture { get; set; }

		/// <summary>
		///     负责人
		/// </summary>
		[JsonProperty("owner")]
		[ForeignKey(typeof(Staff))]
		public string OwnerId { get; set; }
	}
}