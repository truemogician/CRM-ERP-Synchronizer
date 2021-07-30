using Shared.Serialization;

namespace TheFirstFarm.Models.Common {
	public enum ShelfLifeUnit : byte {
		/// <summary>
		///     日
		/// </summary>
		[EnumValue("2n51d7uyh", Platform.FXiaoKe)]
		[EnumValue("D", Platform.Kingdee)]
		Day,

		/// <summary>
		///     月
		/// </summary>
		[EnumValue("Cg0qbSi63", Platform.FXiaoKe)]
		[EnumValue("M", Platform.Kingdee)]
		Month,

		/// <summary>
		///     年
		/// </summary>
		[EnumValue("option1", Platform.FXiaoKe)]
		[EnumValue("Y", Platform.Kingdee)]
		Year,

		/// <summary>
		///     其他
		/// </summary>
		[EnumValue("other", Platform.FXiaoKe)]
		Other,

		[EnumDefault]
		Invalid
	}
}