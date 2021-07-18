using System;

namespace Shared.Exceptions {
	public class TypeException : Exception {
		public TypeException(string message = null, Exception innerException = null) : base(message, innerException) { }
		public TypeException(Type type, string message = null, Exception innerException = null) : this(message, innerException) => Type = type;
		public Type Type { get; set; }
	}

	public class TypeNotMatchException : Exception {
		public TypeNotMatchException(string message = null, Exception innerException = null) : base(message, innerException) { }

		public TypeNotMatchException(Type dstType, Type srcType, string message = null, Exception innerException = null) : this(message, innerException) {
			DstType = dstType;
			SrcType = srcType;
		}

		public Type DstType { get; set; }
		public Type SrcType { get; set; }
	}

	public class InvariantTypeException : TypeNotMatchException {
		public InvariantTypeException(string message = null, Exception innerException = null) : base(message, innerException) { }

		public InvariantTypeException(Type dstType, Type srcType, string message = null, Exception innerException = null) : base(dstType, srcType, message, innerException) { }
	}
}