using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Shared.Serialization;
using Shared.Validation;

namespace FXiaoKe.Requests.Message {
	public class CompositeMessageRequest : MessageRequest {
		public CompositeMessageRequest() { }
		public CompositeMessageRequest(CompositeMessage message) => Composite = message;
		public override MessageType Type => MessageType.Composite;

		[JsonProperty("composite")]
		[Required]
		public CompositeMessage Composite { get; set; }
	}

	public class CompositeMessage {
		public CompositeMessage() { }

		public CompositeMessage(string title, string url) {
			Title = title;
			Link.Url = url;
		}

		[JsonProperty("head")]
		[JsonConverter(typeof(ObjectWrapperConverter), "title")]
		[Required]
		public string Title { get; set; }

		[JsonProperty("first")]
		[JsonConverter(typeof(ObjectWrapperConverter), "content")]
		public string Head { get; set; }

		[JsonProperty("form")]
		[CollectionMaxCount(6)]
		public List<LabelAndValue> Form { get; init; } = new();

		[JsonProperty("remark")]
		[JsonConverter(typeof(ObjectWrapperConverter), "content")]
		public string Tail { get; set; }

		[JsonProperty("link")]
		[Required]
		public Link Link { get; set; } = new();

		public override string ToString() {
			var builder = new StringBuilder();
			builder.AppendLine(Title);
			if (!string.IsNullOrEmpty(Head))
				builder.AppendLine(Head);
			int labelWidth = Form.Max(form => form.Label.Length);
			foreach (var row in Form)
				builder.AppendLine(row.ToString(labelWidth));
			if (!string.IsNullOrEmpty(Tail))
				builder.AppendLine(Tail);
			builder.AppendLine().AppendLine(Link.ToString());
			return builder.ToString();
		}
	}

	public class LabelAndValue {
		public LabelAndValue() { }

		public LabelAndValue(string label, string value) {
			Label = label;
			Value = value;
		}

		[JsonProperty("label")]
		public string Label { get; set; }

		[JsonProperty("value")]
		[Required]
		public string Value { get; set; }

		public static implicit operator LabelAndValue((string, string) tuple) => new(tuple.Item1, tuple.Item2);
		public static implicit operator (string, string)(LabelAndValue self) => (self.Label, self.Value);

		public override string ToString() => ToString(0);

		public string ToString(int width) => $"{(Label ?? string.Empty).PadRight(width)}: {Value}";
	}

	public class Link {
		[JsonProperty("title")]
		[Required]
		public string Title { get; set; } = "详情";

		[JsonProperty("url")]
		[Required]
		public string Url { get; set; }

		public override string ToString() => ToString(0);

		public string ToString(int width) => $"{(Title ?? string.Empty).PadRight(width)}: {Url}";
	}
}