// ReSharper disable StringLiteralTypo
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Kingdee.Forms;
using Newtonsoft.Json;
using Shared.Serialization;
using Shared.Validation;
using TheFirstFarm.Models.Common;

namespace TheFirstFarm.Models.Kingdee {
	[Form("AR_RECEIVABLE")]
	public class ReceivableBill : AuditableErpModel {
		[JsonProperty("FId")]
		public override int Id { get; set; }

		/// <summary>
		///     单据编号
		/// </summary>
		[JsonProperty("FBillNo")]
		[Required]
		public string Number { get; set; }

		/// <summary>
		///     单据类型
		/// </summary>
		[JsonProperty("FBillTypeId")]
		[Required]
		public TypeWrapper BillTypeNumber { get; set; }

		/// <summary>
		///     客户
		/// </summary>
		[JsonProperty("FCustomerId")]
		[Required]
		public NumberWrapper CustomerNumber { get; set; }

		/// <summary>
		///     销售员
		/// </summary>
		[JsonProperty("FSaleerId")]
		[MemberRequired(nameof(NumberWrapper.Number))]
		public NumberWrapper SalesmanNumber { get; set; }

		/// <summary>
		///     应收金额
		/// </summary>
		[JsonProperty("FPayAmountFor")]
		public decimal ExpectedMoney { get; set; }

		[JsonProperty("FEntityDetail")]
		[SubForm]
		[CollectionMinCount(1)]
		public List<ReceivableBillDetail> Details { get; set; }

		public class TypeWrapper : NumberWrapper<InvoiceType> {
			[JsonConverter(typeof(EnumValueConverter), Platform.Kingdee)]
			public override InvoiceType Number {
				get => base.Number;
				set => base.Number = value;
			}
		}
	}
}