using System;

namespace Shared.Exceptions {
	public class TypeReflectionException : Exception {
		public TypeReflectionException() { }
		public TypeReflectionException(string message, Exception innerException = null) : base(message, innerException) { }
		public TypeReflectionException(Type type, string message, Exception innerException = null) : this(message, innerException) => Type = type;
		public Type Type { get; set; }
	}
}