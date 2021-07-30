// ReSharper disable StringLiteralTypo
using Newtonsoft.Json;
using Shared.Serialization;

namespace TheFirstFarm.Models.Common {
	[JsonConverter(typeof(EnumValueConverter))]
	public enum BusinessType {
		/// <summary>
		///     分销调拨
		/// </summary>
		[EnumValue("DRPTRANS")]
		DistributionTransfer,

		/// <summary>
		///     受托加工销售
		/// </summary>
		[EnumValue("COMMISSIONED")]
		Commissioned,

		/// <summary>
		///     分销购销
		/// </summary>
		[EnumValue("DRPSALE")]
		Distribution,

		/// <summary>
		///     寄售
		/// </summary>
		[EnumValue("CONSIGNMENT")]
		Consignment,

		/// <summary>
		///     VMI业务
		/// </summary>
		[EnumValue("VMI")]
		Vmi,

		/// <summary>
		///     普通销售
		/// </summary>
		[EnumValue("NORMAL")]
		Normal,

		/// <summary>
		///     其他
		/// </summary>
		[EnumValue("other")]
		Other,

		/// <summary>
		///		非法值
		/// </summary>
		[EnumDefault]
		Invalid
	}
}