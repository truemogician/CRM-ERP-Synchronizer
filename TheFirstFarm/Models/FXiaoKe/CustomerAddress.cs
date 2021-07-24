// ReSharper disable StringLiteralTypo
using System.ComponentModel.DataAnnotations;
using FXiaoKe.Models;
using Newtonsoft.Json;

namespace TheFirstFarm.Models.FXiaoKe {
	/// <summary>
	///     客户地址
	/// </summary>
	[Model("AccountAddrObj", SubjectTo = typeof(Customer))]
	public class CustomerAddress : ModelBase {
		/// <summary>
		///     地址编号
		/// </summary>
		[JsonProperty("name")]
		[Key]
		[Required]
		[RegularExpression(@"Addr\.\d{4}-\d{2}-\d{2}_\d{6,}")]
		public string Id { get; set; }

		[JsonProperty("account_id")]
		[MasterKey]
		[Required]
		public string CustomerId { get; set; }

		/// <summary>
		///     地区定位
		/// </summary>
		[JsonProperty("regional_location")]
		public string Location { get; set; }

		/// <summary>
		///     默认收货地址
		/// </summary>
		[JsonProperty("is_ship_to_add")]
		[Required]
		public bool IsShippingAddress { get; set; }

		/// <summary>
		///     联系方式
		/// </summary>
		[JsonProperty("contact_way")]
		public string ContactWay { get; set; }
	}
}