// ReSharper disable StringLiteralTypo
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Kingdee.Forms;
using Newtonsoft.Json;
using Shared.Serialization;
using Shared.Validation;
using TheFirstFarm.Models.Common;

namespace TheFirstFarm.Models.Kingdee {
	[Form("SAL_SaleOrder")]
	public class SalesOrder : AuditableErpModel {
		[JsonProperty("FId")]
		public override int Id { get; set; }

		/// <summary>
		/// 	单据类型
		/// </summary>
		[JsonProperty("FBILLTYPEID")]
		[Required]
		public TypeWrapper BillType { get; set; }

		/// <summary>
		/// 	业务类型
		/// </summary>
		[JsonProperty("FBUSINESSTYPE")]
		[Required]
		public BusinessType BusinessType { get; set; }

		/// <summary>
		/// 	单据编号
		/// </summary>
		[JsonProperty("FBILLNO")]
		[Required]
		public string Number { get; set; }

		/// <summary>
		/// 	日期
		/// </summary>
		[JsonProperty("FCREATEDATE")]
		[Required]
		public DateTime Date { get; set; }

		/// <summary>
		/// 	客户
		/// </summary>
		[JsonProperty("FCUSTID")]
		[Required]
		public NumberWrapper CustomerNumber { get; set; }

		/// <summary>
		/// 	销售员
		/// </summary>
		[JsonProperty("FSALERID")]
		[MemberRequired(nameof(NumberWrapper.Number))]
		public NumberWrapper SalesmanNumber { get; set; }

		/// <summary>
		/// 	交货地点
		/// </summary>
		[JsonProperty("FHEADLOCID")]
		public NumberWrapper DeliveryAddressNumber { get; set; }

		[JsonProperty("FSaleOrderEntry")]
		[SubForm]
		public List<SalesOrderMaterial> Materials { get; set; }

		public class TypeWrapper : NumberWrapper<SalesOrderType> {
			[JsonConverter(typeof(EnumValueConverter), Platform.Kingdee)]
			public override SalesOrderType Number {
				get => base.Number;
				set => base.Number = value;
			}
		}
	}
}