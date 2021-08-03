// ReSharper disable StringLiteralTypo
using System.ComponentModel.DataAnnotations;
using FXiaoKe.Models;
using Kingdee.Forms;
using Newtonsoft.Json;

namespace TheFirstFarm.Models.Kingdee {
	[Form("BD_COMMONCONTACT")]
	public class Contact : ErpModelBase {
		[JsonProperty("FContactId")]
		[Key]
		public int Id { get; set; }

		/// <summary>
		///     联系人编码
		/// </summary>
		[JsonProperty("FNumber")]
		public string Number { get; set; }

		/// <summary>
		///     姓名
		/// </summary>
		[JsonProperty("FName")]
		[MainField(Unique = false)]
		[Required]
		public string Name { get; set; }

		/// <summary>
		///     手机1
		/// </summary>
		[JsonProperty("FMobile")]
		[Required]
		public string PhoneNumber { get; set; }

		/// <summary>
		///     地址
		/// </summary>
		[JsonProperty("FBizAddress")]
		[Required]
		public string Address { get; set; }
	}
}