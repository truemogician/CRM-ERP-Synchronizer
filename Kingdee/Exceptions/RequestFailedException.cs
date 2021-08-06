#nullable enable
using System;
using Kingdee.Responses;
using Shared.Exceptions;

namespace Kingdee.Exceptions {
	public class RequestFailedException : ExceptionWithDefaultMessage {
		public RequestFailedException(string? message = null, Exception? innerException = null) : base(message, innerException) { }

		public RequestFailedException(ResponseBase response, string? message = null, Exception? innerException = null) : base(message, innerException) => Set(nameof(ResponseBase), response);

		public ResponseBase? Response => Get<ResponseBase>(nameof(ResponseBase));

		protected override string DefaultMessage => Response is BasicResponse resp ? resp.ResponseStatus.ToString() : Response?.ToString() ?? "Request failed";
	}
}