using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using FXiaoKe.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Shared.Validators;

namespace FXiaoKe.Request.Message {
	[Request("/cgi/message/send", typeof(BasicResponse))]
	public abstract class MessageRequest : RequestWithBasicAuth {
		protected MessageRequest() { }
		protected MessageRequest(Client client) : base(client) { }

		/// <summary>
		///     开放平台员工ID列表（消息接收者，目前最多支持500人）
		/// </summary>
		[JsonProperty("toUser")]
		[IEnumerableLength(1, 500)]
		public List<string> ReceiversIds { get; set; } = new();

		/// <summary>
		///     消息类型
		/// </summary>
		[JsonProperty("msgType")]
		[Required]
		public abstract MessageType Type { get; }
	}

	[JsonConverter(typeof(StringEnumConverter))]
	public enum MessageType : byte {
		[EnumMember(Value = "text")]
		Text,

		[EnumMember(Value = "composite")]
		Composite,

		[EnumMember(Value = "articles")]
		Articles
	}
}