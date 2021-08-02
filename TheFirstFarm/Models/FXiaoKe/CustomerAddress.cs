// ReSharper disable StringLiteralTypo
using System.ComponentModel.DataAnnotations;
using FXiaoKe.Models;
using FXiaoKe.Models.Region;
using Newtonsoft.Json;

namespace TheFirstFarm.Models.FXiaoKe {
	/// <summary>
	///     客户地址
	/// </summary>
	[Model("AccountAddrObj")]
	public class CustomerAddress : CrmModelBase {
		/// <summary>
		///     地址编号
		/// </summary>
		[JsonProperty("name")]
		[MainField]
		[RegularExpression(@"Addr\.\d{4}-\d{2}-\d{2}_\d{6,}")]
		[Required]
		public string Number { get; set; }

		[JsonProperty("account_id")]
		[MasterKey(typeof(Customer))]
		[Required]
		public string CustomerId { get; set; }

		/// <summary>
		///     国家
		/// </summary>
		[JsonProperty("country")]
		public Country? Country { get; set; }

		/// <summary>
		///     省
		/// </summary>
		[JsonProperty("province")]
		public Province? Province { get; set; }

		/// <summary>
		///     市
		/// </summary>
		[JsonProperty("city")]
		public City? City { get; set; }

		/// <summary>
		///     区
		/// </summary>
		[JsonProperty("district")]
		public int? District { get; set; }

		/// <summary>
		///     详细地址
		/// </summary>
		[JsonProperty("address")]
		public string Address { get; set; }

		/// <summary>
		///     定位
		/// </summary>
		[JsonProperty("location")]
		public Location Location { get; set; }

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