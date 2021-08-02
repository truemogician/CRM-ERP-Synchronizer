// ReSharper disable StringLiteralTypo
using System.ComponentModel.DataAnnotations;
using FXiaoKe.Models;
using Newtonsoft.Json;

namespace TheFirstFarm.Models.FXiaoKe {
	/// <summary>
	///     联系人
	/// </summary>
	[Model("ContactObj")]
	public class Contact : CrmModelBase {
		/// <summary>
		///     联系人编码
		/// </summary>
		[JsonProperty("field_b13yj__c")]
		[RegularExpression(@"CONT\d{6,}")]
		public string Number { get; set; } = "CONT000001";

		/// <summary>
		///     客户名称
		/// </summary>
		[JsonProperty("account_id")]
		[ForeignKey(typeof(Customer))]
		public string CustomerId { get; set; }

		/// <summary>
		///     姓名
		/// </summary>
		[JsonProperty("name")]
		[MainField(Unique = false)]
		[Required]
		public string Name { get; set; }

		/// <summary>
		///     手机1
		/// </summary>
		[JsonProperty("mobile1")]
		[Required]
		public string PhoneNumber { get; set; }

		/// <summary>
		///     地址
		/// </summary>
		[JsonProperty("add")]
		[Required]
		public string Address { get; set; }
	}
}