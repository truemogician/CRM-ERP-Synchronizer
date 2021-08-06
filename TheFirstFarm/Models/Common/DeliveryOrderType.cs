// ReSharper disable StringLiteralTypo
using Shared.Serialization;

namespace TheFirstFarm.Models.Common {
	public enum DeliveryOrderType {
		/// <summary>
		///     标准销售出库单
		/// </summary>
		[EnumValue("9u11w22Xu", Platform.FXiaoKe)]
		[EnumValue("XSCKD01_SYS", Platform.Kingdee)]
		Standard,

		/// <summary>
		///     寄售出库单
		/// </summary>
		[EnumValue("2xJNsq93O", Platform.FXiaoKe)]
		[EnumValue("XSCKD02_SYS", Platform.Kingdee)]
		Consignment,

		/// <summary>
		///     零售出库单
		/// </summary>
		[EnumValue("3lZ48u49w", Platform.FXiaoKe)]
		[EnumValue("XSCKD03_SYS", Platform.Kingdee)]
		Retail,

		/// <summary>
		///     分销购销销售出库单
		/// </summary>
		[EnumValue("sE5zk16oe", Platform.FXiaoKe)]
		[EnumValue("XSCKD04_SYS", Platform.Kingdee)]
		Distribution,

		/// <summary>
		///     VMI出库单
		/// </summary>
		[EnumValue("9CcWD41O3", Platform.FXiaoKe)]
		[EnumValue("XSCKD05_SYS", Platform.Kingdee)]
		Vmi,

		/// <summary>
		///     现销出库单
		/// </summary>
		[EnumValue("Fp2pooreH", Platform.FXiaoKe)]
		[EnumValue("XSCKD06_SYS", Platform.Kingdee)]
		Cash,

		/// <summary>
		///     B2C销售出库单
		/// </summary>
		[EnumValue("wgXQPX32z", Platform.FXiaoKe)]
		[EnumValue("XSCKD07_SYS", Platform.Kingdee)]
		B2C,

		/// <summary>
		///     其他
		/// </summary>
		[EnumValue("other", Platform.FXiaoKe)]
		Other
	}
}