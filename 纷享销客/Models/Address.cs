using System.ComponentModel.DataAnnotations;
using FXiaoKe.Utilities;
using Newtonsoft.Json;

namespace FXiaoKe.Models {
	/// <summary>
	/// 客户地址
	/// </summary>
	[Model("AccountAddrObj")]
	public class Address {
		/// <summary>
		/// 地址编号
		/// </summary>
		[JsonProperty("name")]
		[Required]
		public string Id { get; set; }

		/// <summary>
		/// 地区定位
		/// </summary>
		[JsonProperty("regional_location")]
		[Required]
		public string Location { get; set; }

		/// <summary>
		/// 默认收货地址
		/// </summary>
		[JsonProperty("is_ship_to_add")]
		[Required]
		public string IsShippingAddress { get; set; }

		/// <summary>
		/// 联系方式
		/// </summary>
		[JsonProperty("contact_way")]
		[Required]
		public string Contact { get; set; }
	}
}