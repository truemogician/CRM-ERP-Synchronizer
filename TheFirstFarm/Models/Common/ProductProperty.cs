using Newtonsoft.Json;
using Shared.Serialization;

namespace TheFirstFarm.Models.Common {
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