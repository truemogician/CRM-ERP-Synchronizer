using System;
using System.ComponentModel;

namespace Shared.Exceptions {
	public class EnumException<T> : Exception where T : Enum {
		public EnumException(string message = null, Exception innerException = null) : base(message, innerException) { }
	}

	public class EnumValueOutOfRangeException<T> : EnumException<T> where T : Enum {
		public EnumValueOutOfRangeException(string message = null, Exception innerException = null) : base(message, innerException) { }
		public EnumValueOutOfRangeException(T value, string message = null, Exception innerException = null) : base(message, innerException) => Value = value;

		public T Value { get; init; }
	}
}