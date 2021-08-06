using Shared.Serialization;

namespace TheFirstFarm.Models.Common {
	public enum InvoiceType {
		/// <summary>
		///     标准应收单
		/// </summary>
		[EnumValue("Qj5d85aHB", Platform.FXiaoKe)]
		[EnumValue("YSD01_SYS", Platform.Kingdee)]
		Standard,

		/// <summary>
		///     费用应收单
		/// </summary>
		[EnumValue("C57227swl", Platform.FXiaoKe)]
		[EnumValue("YSD02_SYS", Platform.Kingdee)]
		Expense,

		/// <summary>
		///     资产应收单
		/// </summary>
		[EnumValue("aZD7X8z02", Platform.FXiaoKe)]
		[EnumValue("YSD03_SYS", Platform.Kingdee)]
		Asset,

		/// <summary>
		///     转销应收单
		/// </summary>
		[EnumValue("0QY84t0z9", Platform.FXiaoKe)]
		[EnumValue("YSD04_SYS", Platform.Kingdee)]
		Resale,

		/// <summary>
		///     其他
		/// </summary>
		[EnumValue("other", Platform.FXiaoKe)]
		Other
	}
}