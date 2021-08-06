using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using FXiaoKe.Utilities;
using Newtonsoft.Json;
using Shared.Serialization;

namespace FXiaoKe.Models {
	public abstract class CrmModelBase : ModelBase {
		[JsonProperty("_id")]
		[Key]
		[Generated]
		public virtual string DataId { get; set; }

		/// <summary>
		///     创建人
		/// </summary>
		[JsonProperty("created_by")]
		[JsonConverter(typeof(ArrayWrapperConverter))]
		[ForeignKey(typeof(Staff))]
		public string CreatorId { get; set; }

		/// <summary>
		///     创建时间
		/// </summary>
		[JsonProperty("create_time")]
		[JsonConverter(typeof(NullableConverter<TimestampConverter>))]
		public DateTime? CreationTime { get; set; }

		/// <summary>
		///     最后修改人
		/// </summary>
		[JsonProperty("last_modified_by")]
		[JsonConverter(typeof(ArrayWrapperConverter))]
		[ForeignKey(typeof(Staff))]
		[Generated]
		public string LastModifierId { get; set; }

		/// <summary>
		///     最后修改时间
		/// </summary>
		[JsonProperty("last_modified_time")]
		[JsonConverter(typeof(NullableConverter<TimestampConverter>))]
		[Generated]
		public DateTime? LastModifiedTime { get; set; }

		/// <summary>
		///     生命状态
		/// </summary>
		[JsonProperty("life_status")]
		public LifeStatus LifeStatus { get; set; } = LifeStatus.Normal;

		/// <summary>
		///     锁定状态
		/// </summary>
		[JsonProperty("lock_status")]
		[JsonConverter(typeof(BoolConverter), "1", "0")]
		public bool Locked { get; set; }

		/// <summary>
		///     负责人
		/// </summary>
		[JsonProperty("owner")]
		[JsonConverter(typeof(ArrayWrapperConverter))]
		[Required]
		public string OwnerId { get; set; }

		[JsonIgnore]
		public IEnumerable<CrmModelBase> SubModels {
			get {
				var infos = GetType().GetSubModelMembers();
				return infos.SelectSingleOrMany<MemberInfo, CrmModelBase>(info => info.GetValue(this)).ToList();
			}
		}

		[JsonIgnore]
		public MemberInfo MainField => GetType().GetMemberWithAttribute<MainFieldAttribute>();
	}

	[JsonConverter(typeof(EnumValueConverter))]
	public enum LifeStatus {
		/// <summary>
		///     未生效
		/// </summary>
		[EnumValue("ineffective")]
		Ineffective,

		/// <summary>
		///     审核中
		/// </summary>
		[EnumValue("under_review")]
		Reviewing,

		/// <summary>
		///     正常
		/// </summary>
		[EnumValue("normal")]
		Normal,

		/// <summary>
		///     变更中
		/// </summary>
		[EnumValue("in_change")]
		Changing,

		/// <summary>
		///     作废
		/// </summary>
		[EnumValue("invalid")]
		Invalid
	}
}