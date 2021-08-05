// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Shared.Serialization;

namespace Kingdee.Forms {
	[Form]
	public abstract class ErpModelBase : ModelBase {
		/// <summary>
		///		单据Id
		/// </summary>
		[Key]
		[Required]
		public abstract int Id { get; set; }

		/// <summary>
		///		创建人Id
		/// </summary>
		[JsonProperty("FCreatorId")]
		public int? CreatorId { get; set; }

		/// <summary>
		///		创建时间
		/// </summary>
		[JsonProperty("FCreateDate")]
		public DateTime? CreationTime { get; set; }

		/// <summary>
		///		最后修改人Id
		/// </summary>
		[JsonProperty("FModifierId")]
		public int? ModifierId { get; set; }

		/// <summary>
		///		最后修改时间
		/// </summary>
		[JsonProperty("FModifyDate")]
		public DateTime? ModificationTime { get; set; }

		/// <summary>
		///		单据状态
		/// </summary>
		[JsonProperty("FDocumentStatus")]
		public LifeStatus LifeStatus { get; set; }
	}

	[JsonConverter(typeof(EnumValueConverter))]
	public enum LifeStatus : byte {
		/// <summary>
		///		暂存
		/// </summary>
		[EnumValue("Z")]
		Staged,

		/// <summary>
		///		已创建
		/// </summary>
		[EnumValue("A")]
		Created,

		/// <summary>
		///		审核中（已提交）
		/// </summary>
		[EnumValue("B")]
		Auditing,

		/// <summary>
		///		已审核
		/// </summary>
		[EnumValue("C")]
		Audited,

		/// <summary>
		///		重审中
		/// </summary>
		[EnumValue("D")]
		Reauditing
	}
}