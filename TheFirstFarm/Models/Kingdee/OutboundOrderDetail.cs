// ReSharper disable StringLiteralTypo
using System.ComponentModel.DataAnnotations;
using Kingdee.Forms;
using Newtonsoft.Json;

namespace TheFirstFarm.Models.Kingdee {
	public class OutboundOrderDetail : ModelBase {
		/// <summary>
		/// 	物料编码
		/// </summary>
		[JsonProperty("FMATERIALID")]
		[Required]
		public NumberWrapper MaterialNumber { get; set; }

		/// <summary>
		/// 	应发数量
		/// </summary>
		[JsonProperty("FMUSTQTY")]
		[Required]
		public decimal ExpectedAmount { get; set; }

		/// <summary>
		/// 	实发数量
		/// </summary>
		[JsonProperty("FREALQTY")]
		[Required]
		public decimal ActualAmount { get; set; }
	}
}