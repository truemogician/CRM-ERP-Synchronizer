// ReSharper disable StringLiteralTypo
// ReSharper disable InconsistentNaming
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FXiaoKe.Models;
using Newtonsoft.Json;
using Shared.Serialization;
using TheFirstFarm.Models.Common;

namespace TheFirstFarm.Models.FXiaoKe {
	/// <summary>
	///     客户
	/// </summary>
	[Model("AccountObj")]
	public class Customer : CrmModelBase {
		/// <summary>
		///     客户名称
		/// </summary>
		[JsonProperty("name")]
		[MainField]
		[Required]
		public string Name { get; set; }

		/// <summary>
		///     客户编码
		/// </summary>
		[JsonProperty("account_no")]
		[RegularExpression(@"CUST\d{4,}")]
		[Required]
		public string Number { get; set; }

		/// <summary>
		///     金蝶主键Id
		/// </summary>
		[JsonProperty("erp_account_id__c")]
		public int? KingdeeId { get; set; }

		/// <summary>
		///     结算币别
		/// </summary>

		[JsonProperty("field_Nx4Oo__c")]
		[JsonConverter(typeof(EnumValueConverter), Platform.FXiaoKe)]
		[Required]
		public Currency SettlementCurrency { get; set; } = Currency.CNY;

		/// <summary>
		///     创建组织编码
		/// </summary>

		[JsonProperty("field_U8k97__c")]
		[JsonConverter(typeof(EnumValueConverter), OrgSet.FCreatorOrgId)]
		[Required]
		public Organization CreatorOrgId { get; set; } = Organization.TheFirstFarm;

		/// <summary>
		///     创建组织名称
		/// </summary>

		[JsonProperty("field_Qbln5__c")]
		[JsonConverter(typeof(EnumValueConverter), OrgSet.FCreatorOrgName)]
		[Required]
		public Organization CreatorOrgName { get; set; } = Organization.TheFirstFarm;

		/// <summary>
		///     使用组织编码
		/// </summary>

		[JsonProperty("field_f267J__c")]
		[JsonConverter(typeof(EnumValueConverter), OrgSet.FUserOrgId)]
		[Required]
		public Organization UserOrgId { get; set; } = Organization.TheFirstFarm;

		/// <summary>
		///     使用组织名称
		/// </summary>

		[JsonProperty("field_7v1a2__c")]
		[JsonConverter(typeof(EnumValueConverter), OrgSet.FUserOrgName)]
		[Required]
		public Organization UserOrgName { get; set; } = Organization.TheFirstFarm;

		/// <summary>
		///     开票抬头
		/// </summary>
		[JsonProperty("field_1B7uB__c")]
		public string InvoiceTitle { get; set; }

		/// <summary>
		///     纳税识别号
		/// </summary>
		[JsonProperty("field_lW3AG__c")]
		public string TaxpayerId { get; set; }

		/// <summary>
		///     开户银行
		/// </summary>
		[JsonProperty("field_B5sv6__c")]
		public string OpeningBank { get; set; }

		/// <summary>
		///     开户账户
		/// </summary>
		[JsonProperty("field_2e11k__c")]
		public string BankAccount { get; set; }

		/// <summary>
		///     开票通讯地址
		/// </summary>
		[JsonProperty("field_ymSQx__c")]
		public string BillingAddress { get; set; }

		/// <summary>
		///     开票联系电话
		/// </summary>
		[JsonProperty("field_zdg6p__c")]
		public string InvoicePhoneNumber { get; set; }

		/// <summary>
		///     客户类型
		/// </summary>
		[JsonProperty("account_type")]
		public CustomerType CustomerType { get; set; }

		/// <summary>
		///     重要程度
		/// </summary>
		[JsonProperty("field_Wyk2a__c")]
		[JsonConverter(typeof(EnumValueConverter), "Significance")]
		public Level Significance { get; set; }

		/// <summary>
		///     客户意向
		/// </summary>
		[JsonProperty("field_4j3bg__c")]
		[JsonConverter(typeof(EnumValueConverter), "Intention")]
		public Level Intention { get; set; }

		/// <summary>
		///     是否需要同步到金蝶
		/// </summary>

		[JsonProperty("is_sync__c")]
		[JsonConverter(typeof(NullableConverter<BoolConverter>), new object[] {new[] {"是", "否"}})]
		public bool? NeedSync { get; set; }

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

		[SubModel(ReverseKeyName = nameof(FXiaoKe.Contact.CustomerId))]
		[Required]
		public Contact Contact { get; set; }

		[SubModel]
		public List<CustomerAddress> Addresses { get; set; }

		[JsonIgnore]
		[SubModel(Eager = false, Cascade = false)]
		public List<CustomerFinancialInfo> FinancialInfos { get; set; }
	}

	[JsonConverter(typeof(EnumValueConverter))]
	public enum CustomerType {
		/// <summary>
		///     客户
		/// </summary>
		[EnumValue("1")]
		Customer,

		/// <summary>
		///     工厂
		/// </summary>
		[EnumValue("TJ0kf42xv")]
		Factory,

		/// <summary>
		///     商超
		/// </summary>
		[EnumValue("FKV3UdBg4")]
		Supermarket,

		/// <summary>
		///     平台
		/// </summary>
		[EnumValue("h71as17P7")]
		Platform,

		/// <summary>
		///     团膳食堂
		/// </summary>
		[EnumValue("z36PyrYv9")]
		Canteen,

		/// <summary>
		///     商超+平台
		/// </summary>
		[EnumValue("2SMDgxe6A")]
		SupermarketPlatform,

		/// <summary>
		///     休食
		/// </summary>
		[EnumValue("4V0s1Ii9j")]
		LeisureFood,

		/// <summary>
		///     母婴
		/// </summary>
		[EnumValue("j3h3znF1T")]
		Mothering,

		/// <summary>
		///     水果生鲜店
		/// </summary>
		[EnumValue("QABzE2ett")]
		FreshStore,

		/// <summary>
		///     LKA 社区社团
		/// </summary>
		[EnumValue("1x50y7aw4")]
		Lka,

		/// <summary>
		///     GKA
		/// </summary>
		[EnumValue("c9Wh26Tvz")]
		Gka,

		/// <summary>
		///     NKA
		/// </summary>
		[EnumValue("htacg89sv")]
		Nka,

		/// <summary>
		///     BC超
		/// </summary>
		[EnumValue("1mI3c9k7k")]
		BcSupermarket,

		/// <summary>
		///     校超
		/// </summary>
		[EnumValue("lDUYl4371")]
		SchoolSupermarket,

		/// <summary>
		///     微商
		/// </summary>
		[EnumValue("is2Jxj7aP")]
		WeChat,

		/// <summary>
		///     CVS
		/// </summary>
		[EnumValue("KZD25krTv")]
		Cvs,

		/// <summary>
		///     特渠
		/// </summary>
		[EnumValue("q4WGXm339")]
		Special,

		/// <summary>
		///     学校
		/// </summary>
		[EnumValue("6cxgSXPC1")]
		School,

		/// <summary>
		///     企业
		/// </summary>
		[EnumValue("vh31Ln19l")]
		Enterprise,

		/// <summary>
		///     代销
		/// </summary>
		[EnumValue("R1EoD4wmM")]
		Proxy,

		/// <summary>
		///     餐饮
		/// </summary>
		[EnumValue("f02b67vVM")]
		Catering,

		/// <summary>
		///     其他
		/// </summary>
		[EnumValue("other")]
		Other
	}

	public enum Level {
		/// <summary>
		///     一星
		/// </summary>
		[EnumValue("2cJ4zkgoi", "Significance")]
		[EnumValue("option1", "Intention")]
		Low,

		/// <summary>
		///     二星
		/// </summary>
		[EnumValue("ko8Z09Kwb", "Significance")]
		[EnumValue("C5bKq4b3v", "Intention")]
		Middle,

		/// <summary>
		///     三星
		/// </summary>
		[EnumValue("option1", "Significance")]
		[EnumValue("pw4N2ivN3", "Intention")]
		High,

		/// <summary>
		///     其他
		/// </summary>
		[EnumValue("other")]
		Other,

		/// <summary>
		///     非法值
		/// </summary>
		[EnumDefault]
		Illegal
	}
}