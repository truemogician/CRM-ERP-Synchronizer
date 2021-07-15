using System;

namespace Shared.Exceptions {
	public class GenericException<T> : Exception {
		public GenericException(string message = null, Exception innerException = null) : base(message, innerException) { }

		public GenericException(T value, string message = null, Exception innerException = null) : this(message, innerException) => Value = value;

		public T Value { get; }
	}
}