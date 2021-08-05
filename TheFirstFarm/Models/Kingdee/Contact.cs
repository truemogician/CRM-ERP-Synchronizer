// ReSharper disable StringLiteralTypo
using System.ComponentModel.DataAnnotations;
using Kingdee.Forms;
using Newtonsoft.Json;

namespace TheFirstFarm.Models.Kingdee {
	[Form("BD_COMMONCONTACT")]
	public class Contact : ErpModelBase {
		[JsonProperty("FContactId")]
		public override int Id { get; set; }

		/// <summary>
		///     联系人编码
		/// </summary>
		[JsonProperty("FNumber")]
		public string Number { get; set; }

		/// <summary>
		///		客户Id
		/// </summary>
		//[JsonProperty("FCustId")]
		//public int CustomerId { get; set; }

		/// <summary>
		///     姓名
		/// </summary>
		[JsonProperty("FName")]
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