// ReSharper disable StringLiteralTypo
// ReSharper disable InconsistentNaming
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using FXiaoKe.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

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
		///     客户名称
		/// </summary>
		[JsonProperty("name")]
		[Required]
		[PrimaryKey]
		public string Name { get; set; }

		/// <summary>
		///     负责人
		/// </summary>

		[JsonProperty("owner")]
		[Required]
		[ForeignKey(typeof(Staff))]
		public string Contact { get; set; }

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
		[Required]
		public Organization CreatorOrg { get; set; } = Organization.TheFirstFarm;

		/// <summary>
		///     创建组织名称
		/// </summary>

		[JsonProperty("field_Qbln5__c")]
		[Required]
		public OrganizationName CreatorOrgName { get; set; } = OrganizationName.TheFirstFarm;

		/// <summary>
		///     使用组织编码
		/// </summary>

		[JsonProperty("field_f267J__c")]
		[Required]
		public Organization UserOrg { get; set; } = Organization.TheFirstFarm;

		/// <summary>
		///     使用组织名称
		/// </summary>

		[JsonProperty("field_7v1a2__c")]
		[Required]
		public OrganizationName UserOrgName { get; set; } = OrganizationName.TheFirstFarm;

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

	[JsonConverter(typeof(StringEnumConverter))]
	public enum Organization {
		/// <summary>
		///     江苏一号农场科技股份有限公司
		/// </summary>
		[EnumMember(Value = "823li72l1")]
		TheFirstFarm,

		/// <summary>
		///     旅游酒店BD
		/// </summary>
		[EnumMember(Value = "0461V10u4")]
		TourHotelBD,

		/// <summary>
		///     江苏海威科网络科技有限公司
		/// </summary>
		[EnumMember(Value = "option1")]
		Hiwico,

		/// <summary>
		///     其他
		/// </summary>
		[EnumMember(Value = "other")]
		Other
	}

	[JsonConverter(typeof(StringEnumConverter))]
	public enum OrganizationName {
		/// <summary>
		///     江苏一号农场科技股份有限公司
		/// </summary>
		[EnumMember(Value = "jO1vcYL3g")]
		TheFirstFarm,

		/// <summary>
		///     江苏海威科网络科技有限公司
		/// </summary>
		[EnumMember(Value = "g5ySNf9e5")]
		Hiwico,

		/// <summary>
		///     旅游酒店BD
		/// </summary>
		[EnumMember(Value = "option1")]
		TourHotelBD,

		/// <summary>
		///     其他
		/// </summary>
		[EnumMember(Value = "other")]
		Other
	}
}