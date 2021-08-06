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
	[Form("SAL_OUTSTOCK")]
	public class OutboundOrder : AuditableErpModel {
		[JsonProperty("FId")]
		public override int Id { get; set; }

		/// <summary>
		/// 	单据编号
		/// </summary>
		[JsonProperty("FBILLNO")]
		[Required]
		public string Number { get; set; }

		/// <summary>
		/// 	单据类型
		/// </summary>
		[JsonProperty("FBillTypeID")]
		[Required]
		public TypeWrapper BillType { get; set; }

		/// <summary>
		/// 	客户
		/// </summary>
		[JsonProperty("FCUSTOMERID")]
		[Required]
		public NumberWrapper CustomerNumber { get; set; }

		/// <summary>
		/// 	日期
		/// </summary>
		[JsonProperty("FDATE")]
		[Required]
		public DateTime Date { get; set; }

		/// <summary>
		/// 	销售人员
		/// </summary>
		[JsonProperty("FSalesManID")]
		[MemberRequired(nameof(NumberWrapper.Number))]
		public NumberWrapper SalesmanNumber { get; set; }

		/// <summary>
		/// 	源单单号
		/// </summary>
		[JsonProperty("FSrcBillNo")]
		[Required]
		public string SalesOrderNumber { get; set; }

		[JsonProperty("FEntity")]
		[SubForm]
		public List<OutboundOrderDetail> Details { get; set; }

		//[JsonProperty("FOutStockTrace")]
		//[SubForm]
		//public List<LogisticsInfo> Logistics { get; set; }

		public class TypeWrapper : NumberWrapper<DeliveryOrderType?> {
			[JsonConverter(typeof(NullableEnumValueConverter), Platform.Kingdee)]
			public override DeliveryOrderType? Number { get; set; }
		}
	}
}