// ReSharper disable StringLiteralTypo
using Shared.Serialization;

namespace TheFirstFarm.Models.Common {
	public enum SalesOrderType {
		/// <summary>
		///     标准销售订单
		/// </summary>
		[EnumValue("m45a848H2", Platform.FXiaoKe)]
		[EnumValue("XSDD01_SYS", Platform.Kingdee)]
		Standard,

		/// <summary>
		///     寄售销售订单
		/// </summary>
		[EnumValue("DoB9s22iK", Platform.FXiaoKe)]
		[EnumValue("XSDD02_SYS", Platform.Kingdee)]
		Consignment,

		/// <summary>
		///     受拖销售订单
		/// </summary>
		[EnumValue("Cyj781h4i", Platform.FXiaoKe)]
		[EnumValue("XSDD03_SYS", Platform.Kingdee)]
		Commissioned,

		/// <summary>
		///     直运销售订单
		/// </summary>
		[EnumValue("3Isyf25u3", Platform.FXiaoKe)]
		[EnumValue("XSDD04_SYS", Platform.Kingdee)]
		DirectShipment,

		/// <summary>
		///     退货订单
		/// </summary>
		[EnumValue("Xf5f90jGJ", Platform.FXiaoKe)]
		[EnumValue("XSDD05_SYS", Platform.Kingdee)]
		Return,

		/// <summary>
		///     分销调拨订单
		/// </summary>
		[EnumValue("l4YLa197k", Platform.FXiaoKe)]
		[EnumValue("XSDD06_SYS", Platform.Kingdee)]
		DistributionTransfer,

		/// <summary>
		///     分销购销订单
		/// </summary>
		[EnumValue("860L0Uolk", Platform.FXiaoKe)]
		[EnumValue("XSDD07_SYS", Platform.Kingdee)]
		Distribution,

		/// <summary>
		///     VMI销售订单
		/// </summary>
		[EnumValue("bTg34g7n5", Platform.FXiaoKe)]
		[EnumValue("XSDD08_SYS", Platform.Kingdee)]
		Vmi,

		/// <summary>
		///     现销订单
		/// </summary>
		[EnumValue("g9pCW5lM1", Platform.FXiaoKe)]
		[EnumValue("XSDD09_SYS", Platform.Kingdee)]
		Cash,

		/// <summary>
		///     其他
		/// </summary>
		[EnumValue("other", Platform.FXiaoKe)]
		Other
	}
}