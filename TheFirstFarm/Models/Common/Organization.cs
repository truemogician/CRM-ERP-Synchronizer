// ReSharper disable InconsistentNaming
using Shared.Serialization;

namespace TheFirstFarm.Models.Common {
	public enum Organization : byte {
		/// <summary>
		///     江苏一号农场科技股份有限公司
		/// </summary>
		[EnumValue("823li72l1", OrgSet.FCreatorOrgId)]
		[EnumValue("jO1vcYL3g", OrgSet.FCreatorOrgName)]
		[EnumValue("al70nWs58", OrgSet.FUserOrgId)]
		[EnumValue("PsdFo1W03", OrgSet.FUserOrgName)]
		[EnumValue("100", OrgSet.KOrg)]
		TheFirstFarm,

		/// <summary>
		///     江苏海威科网络科技有限公司
		/// </summary>
		[EnumValue("0461V10u4", OrgSet.FCreatorOrgId)]
		[EnumValue("g5ySNf9e5", OrgSet.FCreatorOrgName)]
		[EnumValue("x52p2dk70", OrgSet.FUserOrgId)]
		[EnumValue("c52sR5sy9", OrgSet.FUserOrgName)]
		[EnumValue("101", OrgSet.KOrg)]
		Hiwico,

		/// <summary>
		///     旅游酒店BD
		/// </summary>
		[EnumValue("option1", OrgSet.FCreatorOrgId)]
		[EnumValue("option1", OrgSet.FCreatorOrgName)]
		[EnumValue("option1", OrgSet.FUserOrgId)]
		[EnumValue("option1", OrgSet.FUserOrgName)]
		[EnumValue("10001", OrgSet.KOrg)]
		TourHotelBD,

		/// <summary>
		///     其他
		/// </summary>
		[EnumValue("other", OrgSet.FCreatorOrgId)]
		[EnumValue("other", OrgSet.FCreatorOrgName)]
		[EnumValue("other", OrgSet.FUserOrgId)]
		[EnumValue("other", OrgSet.FUserOrgName)]
		Other,

		[EnumDefault]
		Illegal
	}

	internal enum OrgSet : byte {
		FCreatorOrgId,

		FCreatorOrgName,

		FUserOrgId,

		FUserOrgName,

		KOrg
	}
}