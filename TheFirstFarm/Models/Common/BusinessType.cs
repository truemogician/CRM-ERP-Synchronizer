using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TheFirstFarm.Models.Common {
	[JsonConverter(typeof(StringEnumConverter))]
	public enum BusinessType {
		/// <summary>
		/// 分销调拨
		/// </summary>
		[EnumMember(Value = "DRPTRANS")]
		DrpTrans,

		/// <summary>
		/// 受托加工销售
		/// </summary>
		[EnumMember(Value = "COMMISSIONED")]
		Commissioned,

		/// <summary>
		/// 分销购销
		/// </summary>
		[EnumMember(Value = "DRPSALE")]
		DrpSale,

		/// <summary>
		/// 寄售
		/// </summary>
		[EnumMember(Value = "CONSIGNMENT")]
		Consignment,

		/// <summary>
		/// VMI业务
		/// </summary>
		[EnumMember(Value = "VMI")]
		VMI,

		/// <summary>
		/// 普通销售
		/// </summary>
		[EnumMember(Value = "NORMAL")]
		Normal,

		/// <summary>
		/// 其他
		/// </summary>
		[EnumMember(Value = "other")]
		Other
	}
}