// ReSharper disable StringLiteralTypo
// ReSharper disable InconsistentNaming
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using FXiaoKe.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Shared.Serialization;

namespace TheFirstFarm.Models.FXiaoKe {
	/// <summary>
	///     客户
	/// </summary>
	[Model("AccountObj")]
	public class Customer : ModelBase {
		/// <summary>
		///     客户编码
		/// </summary>

		[JsonProperty("account_no")]
		[Required]
		[RegularExpression(@"CUST\d{4,}")]
		public string Id { get; set; }

		/// <summary>
		///     金蝶主键Id
		/// </summary>
		[JsonProperty("erp_account_id__c")]
		public string KingdeeId { get; set; }

		/// <summary>
		///     客户名称
		/// </summary>
		[JsonProperty("name")]
		[Required]
		[Key]
		public string Name { get; set; }

		/// <summary>
		///     负责人
		/// </summary>

		[JsonProperty("owner")]
		[JsonConverter(typeof(ArrayWrapperConverter))]
		[Required]
		[ForeignKey(typeof(Staff))]
		public string OwnerId { get; set; }

		/// <summary>
		///     结算币别
		/// </summary>

		[JsonProperty("field_Nx4Oo__c")]
		[Required]
		public Currency SettlementCurrency { get; set; } = Currency.CNY;

		/// <summary>
		///     创建组织编码
		/// </summary>

		[JsonProperty("field_U8k97__c")]
		[JsonConverter(typeof(EnumValueConverter), OrgSet.CreatorOrgId)]
		[Required]
		public Organization CreatorOrgId { get; set; } = Organization.TheFirstFarm;

		/// <summary>
		///     创建组织名称
		/// </summary>

		[JsonProperty("field_Qbln5__c")]
		[JsonConverter(typeof(EnumValueConverter), OrgSet.CreatorOrgName)]
		[Required]
		public Organization CreatorOrgName { get; set; } = Organization.TheFirstFarm;

		/// <summary>
		///     使用组织编码
		/// </summary>

		[JsonProperty("field_f267J__c")]
		[JsonConverter(typeof(EnumValueConverter), OrgSet.UserOrgId)]
		[Required]
		public Organization UserOrgId { get; set; } = Organization.TheFirstFarm;

		/// <summary>
		///     使用组织名称
		/// </summary>

		[JsonProperty("field_7v1a2__c")]
		[JsonConverter(typeof(EnumValueConverter), OrgSet.UserOrgName)]
		[Required]
		public Organization UserOrgName { get; set; } = Organization.TheFirstFarm;

		/// <summary>
		///     是否需要同步到金蝶
		/// </summary>

		[JsonProperty("is_sync__c")]
		public string NeedSync { get; set; }

		/// <summary>
		///     同步结果
		/// </summary>

		[JsonProperty("sync_result__c")]
		public string SyncResult { get; set; }

		/// <summary>
		///     是否同步成功
		/// </summary>

		[JsonProperty("sync_TorF__c")]
		public bool SyncSuccess { get; set; }

		[JsonIgnore]
		[SubModel(Eager = true, Cascade = true)]
		public List<CustomerAddress> Addresses { get; set; }

		[JsonIgnore]
		[SubModel(Eager = true, Cascade = true)]
		public List<CustomerFinancialInfo> FinancialInfos { get; set; }
	}

	[JsonConverter(typeof(StringEnumConverter))]
	public enum Currency {
		/// <summary>
		///     PRE001
		/// </summary>
		[EnumMember(Value = "cJoS42Bvj")]
		CNY,

		/// <summary>
		///     PRE002
		/// </summary>
		[EnumMember(Value = "9budk8Qg6")]
		HKD,

		/// <summary>
		///     PRE003
		/// </summary>
		[EnumMember(Value = "0bg2lP3iC")]
		EUR,

		/// <summary>
		///     PRE004
		/// </summary>
		[EnumMember(Value = "oxfswvKPO")]
		JPY,

		/// <summary>
		///     PRE005
		/// </summary>
		[EnumMember(Value = "PWs5iRCd6")]
		TWD,

		/// <summary>
		///     PRE006
		/// </summary>
		[EnumMember(Value = "63b132wdN")]
		GBP,

		/// <summary>
		///     PRE007
		/// </summary>
		[EnumMember(Value = "0cg1IM59y")]
		USD,

		/// <summary>
		///     其他
		/// </summary>
		[EnumMember(Value = "other")]
		Other
	}

	public enum Organization : byte {
		/// <summary>
		///     江苏一号农场科技股份有限公司
		/// </summary>
		[EnumValue("823li72l1", OrgSet.CreatorOrgId)]
		[EnumValue("jO1vcYL3g", OrgSet.CreatorOrgName)]
		[EnumValue("al70nWs58", OrgSet.UserOrgId)]
		[EnumValue("PsdFo1W03", OrgSet.UserOrgName)]
		TheFirstFarm,

		/// <summary>
		///     江苏海威科网络科技有限公司
		/// </summary>
		[EnumValue("0461V10u4", OrgSet.CreatorOrgId)]
		[EnumValue("g5ySNf9e5", OrgSet.CreatorOrgName)]
		[EnumValue("x52p2dk70", OrgSet.UserOrgId)]
		[EnumValue("c52sR5sy9", OrgSet.UserOrgName)]
		Hiwico,

		/// <summary>
		///     旅游酒店BD
		/// </summary>
		[EnumValue("option1", OrgSet.CreatorOrgId)]
		[EnumValue("option1", OrgSet.CreatorOrgName)]
		[EnumValue("option1", OrgSet.UserOrgId)]
		[EnumValue("option1", OrgSet.UserOrgName)]
		TourHotelBD,

		/// <summary>
		///     其他
		/// </summary>
		[EnumValue("other", OrgSet.CreatorOrgId)]
		[EnumValue("other", OrgSet.CreatorOrgName)]
		[EnumValue("other", OrgSet.UserOrgId)]
		[EnumValue("other", OrgSet.UserOrgName)]
		Other
	}

	internal enum OrgSet : byte {
		CreatorOrgId,
		CreatorOrgName,
		UserOrgId,
		UserOrgName
	}
}