// ReSharper disable StringLiteralTypo
using System.ComponentModel.DataAnnotations;
using Kingdee.Forms;
using Newtonsoft.Json;

namespace TheFirstFarm.Models.Kingdee {
	[Form("BD_CUSTCONTACT")]
	public class CustomerAddress : FormBase {
		/// <summary>
		///     地点编码
		/// </summary>
		[JsonProperty("FNumber1")]
		[Required]
		public string Number { get; set; }

		/// <summary>
		///     详细地址
		/// </summary>
		[JsonProperty("FAddress1")]
		public string Location { get; set; }

		/// <summary>
		///     默认收货地址
		/// </summary>
		[JsonProperty("FIsDefaultConsignee")]
		[Required]
		public bool IsShippingAddress { get; set; }

		/// <summary>
		///     移动电话
		/// </summary>
		[JsonProperty("FMobile")]
		public string ContactWay { get; set; }
	}
}