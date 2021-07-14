// ReSharper disable StringLiteralTypo
using FXiaoKe.Models;
using Newtonsoft.Json;

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
		public string Id { get; set; }

		/// <summary>
		///     客户名称
		/// </summary>
		[JsonProperty("name")]
		public string Name { get; set; }

		/// <summary>
		///     负责人
		/// </summary>

		[JsonProperty("owner")]
		public string Contact { get; set; }

		/// <summary>
		///     结算币别
		/// </summary>

		[JsonProperty("field_Nx4Oo__c")]
		public string Currency { get; set; } = "人民币";

		/// <summary>
		///     创建组织编码
		/// </summary>

		[JsonProperty("field_U8k97__c")]
		public string CreatorOrgId { get; set; } = "100";

		/// <summary>
		///     创建组织名称
		/// </summary>

		[JsonProperty("field_Qbln5__c")]
		public string CreatorOrgName { get; set; } = "江苏一号农场科技股份有限公司";

		/// <summary>
		///     使用组织编码
		/// </summary>

		[JsonProperty("field_f267J__c")]
		public string UserOrgId { get; set; } = "100";

		/// <summary>
		///     使用组织名称
		/// </summary>

		[JsonProperty("field_7v1a2__c")]
		public string UserOrgName { get; set; } = "江苏一号农场科技股份有限公司";

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
}