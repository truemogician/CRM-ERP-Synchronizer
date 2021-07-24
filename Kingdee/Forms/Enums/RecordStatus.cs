using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Kingdee.Forms.Enums {
	[JsonConverter(typeof(StringEnumConverter))]
	public enum RecordStatus : byte {
		/// <summary>
		///     暂存
		/// </summary>
		[EnumMember(Value = "Z")]
		Drafted,

		/// <summary>
		///     创建
		/// </summary>
		[EnumMember(Value = "A")]
		Created,

		/// <summary>
		///     审核中
		/// </summary>
		[EnumMember(Value = "B")]
		Auditing,

		/// <summary>
		///     已审核
		/// </summary>
		[EnumMember(Value = "C")]
		Audited,

		/// <summary>
		///     重新审核
		/// </summary>
		[EnumMember(Value = "D")]
		Reauditing
	}
}