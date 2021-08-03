// ReSharper disable StringLiteralTypo
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Kingdee.Forms;
using Newtonsoft.Json;
using Shared.Serialization;
using TheFirstFarm.Models.Common;

namespace TheFirstFarm.Models.Kingdee {
	[Form("BD_Customer")]
	public class Customer : ErpModelBase {
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
		public NumberWrapper SalesmanNumber { get; set; }

		/// <summary>
		///     结算币别#编码
		/// </summary>
		[JsonProperty("FReceiveCurrId")]
		[Required]
		public CurrencyWrapper CurrencyNumber { get; set; }

		/// <summary>
		///     创建组织#编码
		/// </summary>
		[JsonProperty("FCreateOrgId")]
		[Required]
		public OrganizationWrapper CreatorOrgNumber { get; set; }

		/// <summary>
		///     使用组织#编码
		/// </summary>
		[JsonProperty("FUseOrgId")]
		[Required]
		public OrganizationWrapper UserOrgNumber { get; set; }

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

		[JsonProperty("FT_BD_CUSTLOCATION")]
		public List<ContactRef> Contacts { get; set; }

		[JsonProperty("FT_BD_CUSTCONTACT")]
		[SubForm]
		public List<CustomerAddress> Addresses { get; set; }

		public class CurrencyWrapper : NumberWrapper<Currency> {
			[JsonConverter(typeof(EnumValueConverter), Platform.Kingdee)]
			public override Currency Number {
				get => base.Number;
				set => base.Number = value;
			}

			public static implicit operator CurrencyWrapper(Currency value) => new() {Number = value};
		}

		public class OrganizationWrapper : NumberWrapper<Organization> {
			[JsonConverter(typeof(EnumValueConverter), OrgSet.KOrg)]
			public override Organization Number {
				get => base.Number;
				set => base.Number = value;
			}

			public static implicit operator OrganizationWrapper(Organization value) => new() {Number = value};
		}

		public class ContactRef {
			[JsonProperty("FContactId")]
			public NumberWrapper Number { get; set; }
		}
	}
}