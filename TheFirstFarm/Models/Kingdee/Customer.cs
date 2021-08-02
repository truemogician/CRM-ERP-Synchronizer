// ReSharper disable StringLiteralTypo
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Kingdee.Forms;
using Newtonsoft.Json;

namespace TheFirstFarm.Models.Kingdee {
	[Form("BD_Customer")]
	public class Customer : FormBase {
		/// <summary>
		///     客户Id
		/// </summary>
		[JsonProperty("FCustId")]
		[Key]
		[Required]
		public int Id { get; set; }

		/// <summary>
		///     客户名称
		/// </summary>
		[JsonProperty("FName")]
		[Required]
		public string Name { get; set; }

		/// <summary>
		///     客户编码
		/// </summary>
		[JsonProperty("FNumber")]
		[Required]
		public string Number { get; set; }

		/// <summary>
		///     销售员#名称
		/// </summary>
		[JsonProperty("FSeller")]
		public NumberWrapper SalesmanId { get; set; }

		/// <summary>
		///     结算币别#编码
		/// </summary>
		[JsonProperty("FReceiveCurrId")]
		[Required]
		public NumberWrapper CurrencyId { get; set; }

		/// <summary>
		///     创建组织#编码
		/// </summary>
		[JsonProperty("FCreateOrgId")]
		[Required]
		public NumberWrapper CreatorOrgId { get; set; }

		/// <summary>
		///     使用组织#编码
		/// </summary>
		[JsonProperty("FUseOrgId")]
		[Required]
		public NumberWrapper UserOrgId { get; set; }

		/// <summary>
		///     发票抬头
		/// </summary>
		[JsonProperty("FInvoiceTitle")]
		public string InvoiceTitle { get; set; }

		/// <summary>
		///     纳税登记号
		/// </summary>
		[JsonProperty("FTaxRegisterCode")]
		public string TaxpayerId { get; set; }

		/// <summary>
		///     开户银行
		/// </summary>
		[JsonProperty("FInvoiceBankName")]
		public string OpeningBank { get; set; }

		/// <summary>
		///     银行账户
		/// </summary>
		[JsonProperty("FInvoiceBankAccount")]
		public string BankAccount { get; set; }

		/// <summary>
		///     开票通讯地址
		/// </summary>
		[JsonProperty("FInvoiceAddress")]
		public string BillingAddress { get; set; }

		/// <summary>
		///     开票联系电话
		/// </summary>
		[JsonProperty("FINVOICETEL")]
		public string PhoneNumber { get; set; }

		[JsonProperty("FT_BD_CUSTCONTACT")]
		[SubForm]
		public List<CustomerAddress> Addresses { get; set; }
	}
}