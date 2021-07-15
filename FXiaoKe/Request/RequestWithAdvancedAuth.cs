﻿using Newtonsoft.Json;

namespace FXiaoKe.Request {
	public class RequestWithAdvancedAuth : RequestWithBasicAuth {
		public RequestWithAdvancedAuth() { }

		public RequestWithAdvancedAuth(Client client) : base(client) => OperatorId = client.OperatorId;

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