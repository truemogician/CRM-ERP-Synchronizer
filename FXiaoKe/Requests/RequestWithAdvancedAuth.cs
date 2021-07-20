using Newtonsoft.Json;

namespace FXiaoKe.Requests {
	public class RequestWithAdvancedAuth : RequestWithBasicAuth {
		/// <summary>
		///     当前操作人OpenUserID
		/// </summary>
		[JsonProperty("currentOpenUserId")]
		public string OperatorId { get; set; }

		public sealed override void UseClient(Client client) {
			base.UseClient(client);
			OperatorId = client.OperatorId;
		}
	}
}