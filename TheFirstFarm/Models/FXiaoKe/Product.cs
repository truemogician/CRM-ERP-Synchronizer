// ReSharper disable StringLiteralTypo
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using FXiaoKe.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
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
		public string Image { get; set; }

		/// <summary>
		///     负责人
		/// </summary>
		[JsonProperty("owner")]
		[Required]
		public string OwnerId { get; set; }
	}

	[JsonConverter(typeof(StringEnumConverter))]
	public enum ShelfLifeUnit : byte {
		/// <summary>
		///     日
		/// </summary>
		[EnumMember(Value = "2n51d7uyh")]
		Day,

		/// <summary>
		///     月
		/// </summary>
		[EnumMember(Value = "Cg0qbSi63")]
		Month,

		/// <summary>
		///     年
		/// </summary>
		[EnumMember(Value = "option1")]
		Year,

		/// <summary>
		///     其他
		/// </summary>
		[EnumMember(Value = "other")]
		Other
	}

	[JsonConverter(typeof(StringEnumConverter))]
	public enum ProductProperty {
		/// <summary>
		///     外购
		/// </summary>
		[EnumMember(Value = "1")]
		OutPurchased,

		/// <summary>
		///     自制
		/// </summary>
		[EnumMember(Value = "2")]
		SelfMade,

		/// <summary>
		///     委外
		/// </summary>
		[EnumMember(Value = "3")]
		Outsource,

		/// <summary>
		///     配置
		/// </summary>
		[EnumMember(Value = "9")]
		Configuration,

		/// <summary>
		///     资产
		/// </summary>
		[EnumMember(Value = "10")]
		Asset,

		/// <summary>
		///     特征
		/// </summary>
		[EnumMember(Value = "4")]
		Characteristic,

		/// <summary>
		///     费用
		/// </summary>
		[EnumMember(Value = "11")]
		Expense,

		/// <summary>
		///     虚拟
		/// </summary>
		[EnumMember(Value = "5")]
		Virtual,

		/// <summary>
		///     服务
		/// </summary>
		[EnumMember(Value = "6")]
		Service,

		/// <summary>
		///     一次性
		/// </summary>
		[EnumMember(Value = "7")]
		Disposable,

		/// <summary>
		///     模型
		/// </summary>
		[EnumMember(Value = "12")]
		Model,

		/// <summary>
		///     产品系列
		/// </summary>
		[EnumMember(Value = "13")]
		ProductSeries,

		/// <summary>
		///     其他
		/// </summary>
		[EnumMember(Value = "other")]
		Other
	}
}