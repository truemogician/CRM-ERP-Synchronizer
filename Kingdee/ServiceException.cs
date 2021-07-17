using System;
using System.Collections;
using System.Runtime.Serialization;

namespace Kingdee {
	public class ServiceException : Exception {
		private const int ApiClientErrorCode = -1;

		public ServiceException() { }

		public ServiceException(int code, string message, Exception ex = null)
			: base(message, ex) {
			HttpCode = code;
			ErrorCode = code;
			Message = message;
		}

		public ServiceException(string message)
			: this(ApiClientErrorCode, message) { }

		public ServiceException(Exception ex)
			: this(ApiClientErrorCode, ex.Message, ex) { }

		private ServiceException(SerializationInfo info, StreamingContext context) {
			InnerExWrapper = info.GetValue(nameof(InnerExWrapper), typeof(ExceptionWrapper)) as ExceptionWrapper;
			HttpCode = info.GetInt32(nameof(HttpCode));
			ErrorCode = info.GetInt32("HResult");
			HelpLink = info.GetString("HelpURL");
			ClassName = info.GetString(nameof(ClassName));
			ExceptionMethod = info.GetString(nameof(ExceptionMethod));
			Message = info.GetString(nameof(Message));
			Source = info.GetString(nameof(Source));
			StackTrace = info.GetString("StackTraceString");
			try {
				info.GetValue(nameof(Data), typeof(IDictionary));
			}
			catch {
				// ignored
			}
		}

		public string ClassName { get; private set; }

		public string ExceptionMethod { get; private set; }

		public int ErrorCode { get; }

		public sealed override string HelpLink { get; set; }

		public override string Message { get; }

		public sealed override string Source { get; set; }

		public override string StackTrace { get; }

		public bool Handled { get; set; }

		public ExceptionWrapper InnerExWrapper { get; set; }

		public int HttpCode { get; set; }

		public ExceptionWrapper GetLastInnerEx() {
			if (InnerExWrapper == null)
				return null;
			var exceptionWrapper = InnerExWrapper;
			while (exceptionWrapper.InnerException is {
				IsEmpty: false
			})
				exceptionWrapper = exceptionWrapper.InnerException;
			return exceptionWrapper;
		}
	}
}