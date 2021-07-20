using Newtonsoft.Json;
using Shared.Serialization;

namespace FXiaoKe.Request.Message {
	public class TextMessageRequest : MessageRequest {
		public TextMessageRequest() { }

		public TextMessageRequest(string text) => Text = text;

		public override MessageType Type => MessageType.Text;

		[JsonProperty("text")]
		[JsonConverter(typeof(ObjectWrapperConverter), "content")]
		public string Text { get; }
	}
}