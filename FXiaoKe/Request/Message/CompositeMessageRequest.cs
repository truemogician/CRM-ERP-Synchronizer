using System.Collections.Generic;
using Newtonsoft.Json;
using Shared.JsonConverters;

namespace FXiaoKe.Request.Message {
	public class CompositeMessageRequest : MessageRequest {
		public CompositeMessageRequest() { }
		public CompositeMessageRequest(Client client) : base(client) { }
		public override MessageType Type => MessageType.Composite;

		[JsonProperty("composite")]
		public CompositeMessage Composite { get; set; }
	}

	public class CompositeMessage {
		[JsonProperty("head")]
		[JsonConverter(typeof(ObjectWrapperConverter), "head")]
		public string Head { get; set; }

		[JsonProperty("first")]
		[JsonConverter(typeof(ObjectWrapperConverter), "content")]
		public string First { get; set; }

		[JsonProperty("form")]
		public List<Form> Form { get; set; }

		[JsonProperty("remark")]
		[JsonConverter(typeof(ObjectWrapperConverter), "content")]
		public string Remark { get; set; }

		[JsonProperty("link")]
		public Link Link { get; set; }
	}

	public class Form {
		[JsonProperty("label")]
		public string Label { get; set; }

		[JsonProperty("value")]
		public string Value { get; set; }
	}

	public class Link {
		[JsonProperty("title")]
		public string Title { get; set; }

		[JsonProperty("url")]
		public string Url { get; set; }
	}
}