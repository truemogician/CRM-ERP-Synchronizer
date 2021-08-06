using Kingdee.Forms;
using Newtonsoft.Json;

namespace TheFirstFarm.Models.Kingdee {
	public class ReceivableBillDetail : ModelBase {
		/// <summary>
		///     销售订单号
		/// </summary>
		[JsonProperty("FOrderNumber")]
		public string SalesOrderNumber { get; set; }
	}
}