using System;
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
			if (client.Operator is null)
				throw new InvalidOperationException($"{nameof(client.Operator)} hasn't been set");
			if (string.IsNullOrEmpty(client.Operator.Id))
				throw new InvalidOperationException($"{nameof(client.Operator)}'s {nameof(client.Operator.Id)} is empty");
			OperatorId = client.Operator.Id;
		}
	}
}