// ReSharper disable StringLiteralTypo
using Shared.Serialization;

namespace TheFirstFarm.Models.Common {
	public enum ReturnType {
		/// <summary>
		///     退货（指退货不补货）
		/// </summary>
		[MultipleEnumMember("yoC0q11kf", Platform.FXiaoKe)]
		[MultipleEnumMember("THLX01_SYS", Platform.Kingdee)]
		ReturnOnly,

		/// <summary>
		///     退货补货（指退货且补货）
		/// </summary>
		[MultipleEnumMember("07Z2px89e", Platform.FXiaoKe)]
		[MultipleEnumMember("THLX02_SYS", Platform.Kingdee)]
		ReturnAndReplenish
	}
}