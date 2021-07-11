using System.ComponentModel.DataAnnotations;
using FXiaoKe.Utilities;
using Newtonsoft.Json;

namespace FXiaoKe.Models {
	/// <summary>
	/// 客户财务信息
	/// </summary>
	[Model("AccountFinInfoObj")]
	public class FinancialInfo {
		/// <summary>
		/// 发票抬头
		/// </summary>
		[JsonProperty("name")]
		[Required]
		public string Title { get; set; }

		/// <summary>
		/// 纳税识别号
		/// </summary>
		[JsonProperty("tax_id")]
		[Required]
		public string TaxpayerId { get; set; }

		/// <summary>
		/// 开户银行
		/// </summary>
		[JsonProperty("account_bank")]
		[Required]
		public string OpeningBank { get; set; }

		/// <summary>
		/// 开户账户
		/// </summary>
		[JsonProperty("account_bank_no")]
		[Required]
		public string BankAccount { get; set; }

		/// <summary>
		/// 开票地址
		/// </summary>
		[JsonProperty("nvoice_add")]
		public string BillingAddress { get; set; }

		/// <summary>
		/// 电话
		/// </summary>
		[JsonProperty("tel")]
		public string PhoneNumber { get; set; }
	}
}