using System;
using System.ComponentModel.DataAnnotations;
using Kingdee.Forms;
using Newtonsoft.Json;

namespace TheFirstFarm.Models.Kingdee {
	public class LogisticsInfo : ModelBase {
		/// <summary>
		/// 	物流公司
		/// </summary>
		[JsonProperty("FLogComId")]
		[Required]
		public CodeWrapper CompanyCode { get; set; }

		/// <summary>
		/// 	物流单号
		/// </summary>
		[JsonProperty("FCarryBillNo")]
		[Required]
		public string WaybillNumber { get; set; }

		/// <summary>
		/// 	发货时间
		/// </summary>
		[JsonProperty("FDelTime")]
		public DateTime DeliveryTime { get; set; }

		/// <summary>
		/// 	物流状态
		/// </summary>
		[JsonProperty("FTraceStatus")]
		public string Status { get; set; }

		public class CodeWrapper : WrapperBase<string> {
			[JsonProperty("FCode")]
			public string Code { get; set; }

			protected override string ValueName => nameof(Code);
		}
	}
}