using System;
using System.Collections;

namespace Kingdee.Exceptions {
	public class ExceptionWrapper {
		public bool IsEmpty {
			get {
				if (!string.IsNullOrEmpty(HelpLink) || !string.IsNullOrEmpty(Message) || !string.IsNullOrEmpty(Message) || !string.IsNullOrEmpty(Source) || !string.IsNullOrEmpty(StackTrace) || !string.IsNullOrEmpty(ExceptionType))
					return false;
				return InnerException == null || InnerException.IsEmpty;
			}
		}

		public IDictionary Data { get; private set; }

		public string HelpLink { get; set; }

		public string Message { get; set; }

		public string Source { get; set; }

		public string StackTrace { get; set; }

		public string TargetSite { get; set; }

		public string ExceptionType { get; set; }

		public ExceptionWrapper InnerException { get; set; }

		public static ExceptionWrapper WrapEx(Exception ex) => Wrap(ex);

		private static ExceptionWrapper Wrap(Exception ex) {
			var exceptionWrapper = new ExceptionWrapper {
				Data = ex.Data,
				Message = ex.Message,
				StackTrace = ex.StackTrace,
				ExceptionType = ex.GetType().AssemblyQualifiedName
			};
			if (ex.InnerException != null)
				exceptionWrapper.InnerException = Wrap(ex.InnerException);
			return exceptionWrapper;
		}
	}
}