using System;
using FXiaoKe.Request;
using FXiaoKe.Response;
using Shared.Exceptions;

namespace FXiaoKe.Exceptions {
	public class RequestException : ExceptionWithDefaultMessage {
		public RequestException(string message = null, Exception innerException = null) : base(message, innerException) { }

		public RequestException(RequestBase request, string message = null, Exception innerException = null) : base(message, innerException) => Data[nameof(Request)] = request;

		public RequestBase Request => Data[nameof(Request)] as RequestBase;

		protected override string DefaultMessage => $"Request(Method = {Request.Attribute.Method.Method}, Url = {Request.Attribute.Url})";
	}

	public class RequestFailedException : RequestException {
		public RequestFailedException(string message = null, Exception innerException = null) : base(message, innerException) { }

		public RequestFailedException(RequestBase request, ResponseBase response = null, string message = null, Exception innerException = null) : base(request, message, innerException) => Data[nameof(Response)] = response;

		public ResponseBase Response => Data[nameof(Response)] as ResponseBase;

		protected override string DefaultMessage => $"{base.DefaultMessage} Failed";
	}
}