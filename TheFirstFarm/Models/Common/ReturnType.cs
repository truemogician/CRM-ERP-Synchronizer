// ReSharper disable StringLiteralTypo
using Shared.Serialization;

namespace TheFirstFarm.Models.Common {
	public enum ReturnType : byte {
		/// <summary>
		///     退货（指退货不补货）
		/// </summary>
		[EnumValue("yoC0q11kf", Platform.FXiaoKe)]
		[EnumValue("THLX01_SYS", Platform.Kingdee)]
		ReturnOnly,

		/// <summary>
		///     退货补货（指退货且补货）
		/// </summary>
		[EnumValue("07Z2px89e", Platform.FXiaoKe)]
		[EnumValue("THLX02_SYS", Platform.Kingdee)]
		ReturnAndReplenish,

		/// <summary>
		///     非法值
		/// </summary>
		[EnumDefault]
		Invalid
	}
}