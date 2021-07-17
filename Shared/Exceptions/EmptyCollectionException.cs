using System;

namespace Shared.Exceptions {
	public class EmptyCollectionException : Exception {
		public EmptyCollectionException(string message = null, Exception innerException = null) : base(message, innerException) { }
	}
}