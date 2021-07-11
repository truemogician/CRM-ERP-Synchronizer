using Newtonsoft.Json;

namespace FXiaoKe.Api.Request {
	public class CrmBasicRequest<T> : RequestBase {
		/// <summary>
		/// 当前操作人OpenUserID
		/// </summary>
		[JsonProperty("currentOpenUserId")]
		public string OperatorId { get; set; }

		/// <summary>
		/// 数据Map
		/// </summary>
		[JsonProperty("data")]
		public T Data { get; set; }
	}
}