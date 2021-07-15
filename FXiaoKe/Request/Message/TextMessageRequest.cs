using Newtonsoft.Json;
using Shared.JsonConverters;

namespace FXiaoKe.Request.Message {
	public class TextMessageRequest : MessageRequest {
		public TextMessageRequest() { }

		public TextMessageRequest(Client client) : base(client) { }

		public TextMessageRequest(string text, Client client) : this(client) => Text = text;

		public override MessageType Type => MessageType.Text;

		[JsonProperty("text")]
		[JsonConverter(typeof(ObjectWrapperConverter), "content")]
		public string Text { get; }
	}
}