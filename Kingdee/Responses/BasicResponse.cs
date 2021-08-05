// ReSharper disable StringLiteralTypo
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Shared.Serialization;

namespace Kingdee.Responses {
	[JsonConverter(typeof(ObjectWrapperConverter<BasicResponse>), "Result")]
	public class BasicResponse : ResponseBase {
		[JsonProperty("ResponseStatus")]
		public ResponseStatus ResponseStatus { get; set; }

		public static implicit operator bool(BasicResponse resp) => resp.ResponseStatus;
	}

	public class ResponseStatus {
		[JsonProperty("ErrorCode")]
		public int ErrorCode { get; set; }

		[JsonProperty("MsgCode")]
		public int MessageCode { get; set; }

		[JsonProperty("IsSuccess")]
		public bool Success { get; set; }

		[JsonProperty("Errors")]
		public List<FieldMessage> FailureDetails { get; set; }

		[JsonProperty("SuccessMessages")]
		public List<FieldMessage> SuccessDetails { get; set; }

		[JsonProperty("SuccessEntitys")]
		public List<SuccessEntity> SuccessEntities { get; set; }

		public static implicit operator bool(ResponseStatus status) => status.Success;

		public override string ToString() => string.Join(", ", (Success ? SuccessDetails : FailureDetails).Select(msg => msg.ToString()));
	}

	public class FieldMessage {
		[JsonProperty("FieldName")]
		public string FieldName { get; set; }

		[JsonProperty("Message")]
		public string Message { get; set; }

		[JsonProperty("DIndex")]
		public int DIndex { get; set; }

		public override string ToString() => string.IsNullOrEmpty(FieldName) ? Message : $"{FieldName}: {Message}";
	}

	public class SuccessEntity {
		[JsonProperty("Id")]
		public int? Id { get; set; }

		[JsonProperty("Number")]
		public string Number { get; set; }

		[JsonProperty("DIndex")]
		public int DIndex { get; set; }
	}
}