// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
using Shared.Serialization;

namespace TheFirstFarm.Models.Common {
	public enum Currency {
		/// <summary>
		///     人民币
		/// </summary>
		[EnumValue("cJoS42Bvj", Platform.FXiaoKe)]
		[EnumValue("PRE001", Platform.Kingdee)]
		CNY,

		/// <summary>
		///     香港元
		/// </summary>
		[EnumValue("9budk8Qg6", Platform.FXiaoKe)]
		[EnumValue("PRE002", Platform.Kingdee)]
		HKD,

		/// <summary>
		///     欧元
		/// </summary>
		[EnumValue("0bg2lP3iC", Platform.FXiaoKe)]
		[EnumValue("PRE003", Platform.Kingdee)]
		EUR,

		/// <summary>
		///     日元
		/// </summary>
		[EnumValue("oxfswvKPO", Platform.FXiaoKe)]
		[EnumValue("PRE004", Platform.Kingdee)]
		JPY,

		/// <summary>
		///     新台币
		/// </summary>
		[EnumValue("PWs5iRCd6", Platform.FXiaoKe)]
		[EnumValue("PRE005", Platform.Kingdee)]
		TWD,

		/// <summary>
		///     英镑
		/// </summary>
		[EnumValue("63b132wdN", Platform.FXiaoKe)]
		[EnumValue("PRE006", Platform.Kingdee)]
		GBP,

		/// <summary>
		///     美元
		/// </summary>
		[EnumValue("0cg1IM59y", Platform.FXiaoKe)]
		[EnumValue("PRE007", Platform.Kingdee)]
		USD,

		/// <summary>
		///     其他
		/// </summary>
		[EnumValue("other", Platform.FXiaoKe)]
		Other,

		[EnumDefault]
		Illegal
	}
}