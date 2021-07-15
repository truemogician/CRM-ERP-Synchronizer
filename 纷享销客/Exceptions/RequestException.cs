using System;
using FXiaoKe.Request;
using Shared.Exceptions;

namespace FXiaoKe.Exceptions {
	public class RequestException<T> : GenericException<T> where T : RequestBase {
		public RequestException(string message = null, Exception innerException = null) : base(message, innerException) { }

		public RequestException(T value, string message = null, Exception innerException = null) : base(value, message, innerException) { }
	}
}