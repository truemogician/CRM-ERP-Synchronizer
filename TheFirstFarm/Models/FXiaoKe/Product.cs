// ReSharper disable StringLiteralTypo
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FXiaoKe.Models;
using Newtonsoft.Json;
using Shared.Serialization;
using TheFirstFarm.Models.Common;

namespace TheFirstFarm.Models.FXiaoKe {
	/// <summary>
	///     产品
	/// </summary>
	[Model("ProductObj")]
	public class Product : CrmModelBase {
		/// <summary>
		///     产品编码
		/// </summary>
		[JsonProperty("name")]
		[MainField]
		[Required]
		public string Number { get; set; }

		/// <summary>
		///     产品名称
		/// </summary>
		[JsonProperty("product_code")]
		public string Name { get; set; }

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
		[Required]
		public string Category { get; set; }//Enum too large

		/// <summary>
		///     物料属性
		/// </summary>
		[JsonProperty("field_WzGGe__c")]
		[Required]
		public ProductProperty ProductProperty { get; set; }

		/// <summary>
		///     计量单位
		/// </summary>
		[JsonProperty("field_2gj87__c")]
		public string MeasurementUnit { get; set; }

		/// <summary>
		///     长
		/// </summary>
		[JsonProperty("field_9M1ng__c")]
		public decimal? Length { get; set; }

		/// <summary>
		///     宽
		/// </summary>
		[JsonProperty("field_kKc10__c")]
		public decimal? Width { get; set; }

		/// <summary>
		///     高
		/// </summary>
		[JsonProperty("field_G2e0j__c")]
		public decimal? Height { get; set; }

		/// <summary>
		///     保质期单位
		/// </summary>
		[JsonProperty("field_a6e61__c")]
		[JsonConverter(typeof(EnumValueConverter), Platform.FXiaoKe)]
		public ShelfLifeUnit ShelfLifeUnit { get; set; }

		/// <summary>
		///     保质期
		/// </summary>
		[JsonProperty("field_rwK57__c")]
		public decimal? ShelfLife { get; set; }

		/// <summary>
		///     起订量
		/// </summary>
		[JsonProperty("field_BN728__c")]
		public decimal? MinOrderQuantity { get; set; }

		/// <summary>
		///     允许退货
		/// </summary>
		[JsonProperty("field_15ox1__c")]
		[JsonConverter(typeof(BoolConverter), "hSv12h4jJ", "option1")]
		public bool AllowReturn { get; set; }

		/// <summary>
		///     产品图片
		/// </summary>
		[JsonProperty("picture_path")]
		public List<MediaInfo> Images { get; set; }
	}
}