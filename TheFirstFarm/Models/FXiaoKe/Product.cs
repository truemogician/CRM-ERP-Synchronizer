// ReSharper disable StringLiteralTypo
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FXiaoKe.Models;
using Newtonsoft.Json;
using Shared.Serialization;

namespace TheFirstFarm.Models.FXiaoKe {
	/// <summary>
	///     产品
	/// </summary>
	[Model("ProductObj")]
	public class Product : ModelBase {
		/// <summary>
		///     产品编码
		/// </summary>
		[JsonProperty("name")]
		[Required]
		public string Id { get; set; }

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
		public ShelfLifeUnit ShelfLifeUnit { get; set; }

		/// <summary>
		///     保质期
		/// </summary>
		[JsonProperty("field_rwK57__c")]
		public string ShelfLife { get; set; }

		/// <summary>
		///     起订量
		/// </summary>
		[JsonProperty("field_BN728__c")]
		public string MinOrderQuantity { get; set; }

		/// <summary>
		///     允许退货
		/// </summary>
		[JsonProperty("field_15ox1__c")]
		[JsonConverter(typeof(BoolConverter), "hSv12h4jJ", "option1")]
		public bool? AllowReturn { get; set; }

		/// <summary>
		///     产品图片
		/// </summary>
		[JsonProperty("picture_path")]
		public List<ImageInfo> Images { get; set; }

		/// <summary>
		///     负责人
		/// </summary>
		[JsonProperty("owner")]
		[JsonConverter(typeof(ArrayWrapperConverter))]
		[Required]
		public string OwnerId { get; set; }
	}

	[JsonConverter(typeof(EnumValueConverter))]
	public enum ShelfLifeUnit : byte {
		/// <summary>
		///     日
		/// </summary>
		[EnumValue("2n51d7uyh")]
		Day,

		/// <summary>
		///     月
		/// </summary>
		[EnumValue("Cg0qbSi63")]
		Month,

		/// <summary>
		///     年
		/// </summary>
		[EnumValue("option1")]
		Year,

		/// <summary>
		///     其他
		/// </summary>
		[EnumValue("other")]
		Other,

		[EnumDefault]
		Invalid
	}

	[JsonConverter(typeof(EnumValueConverter))]
	public enum ProductProperty {
		/// <summary>
		///     外购
		/// </summary>
		[EnumValue("1")]
		OutPurchased,

		/// <summary>
		///     自制
		/// </summary>
		[EnumValue("2")]
		SelfMade,

		/// <summary>
		///     委外
		/// </summary>
		[EnumValue("3")]
		Outsource,

		/// <summary>
		///     配置
		/// </summary>
		[EnumValue("9")]
		Configuration,

		/// <summary>
		///     资产
		/// </summary>
		[EnumValue("10")]
		Asset,

		/// <summary>
		///     特征
		/// </summary>
		[EnumValue("4")]
		Characteristic,

		/// <summary>
		///     费用
		/// </summary>
		[EnumValue("11")]
		Expense,

		/// <summary>
		///     虚拟
		/// </summary>
		[EnumValue("5")]
		Virtual,

		/// <summary>
		///     服务
		/// </summary>
		[EnumValue("6")]
		Service,

		/// <summary>
		///     一次性
		/// </summary>
		[EnumValue("7")]
		Disposable,

		/// <summary>
		///     模型
		/// </summary>
		[EnumValue("12")]
		Model,

		/// <summary>
		///     产品系列
		/// </summary>
		[EnumValue("13")]
		ProductSeries,

		/// <summary>
		///     其他
		/// </summary>
		[EnumValue("other")]
		Other,

		[EnumDefault]
		Invalid
	}
}