using System;

namespace Shared.Exceptions {
	public class AttributeException : ExceptionWithDefaultMessage {
		public AttributeException(string message = null, Exception innerException = null) : base(message, innerException) { }

		public AttributeException(Type attributeType, string message = null, Exception innerException = null) : this(message, innerException) => Data[nameof(AttributeType)] = attributeType;

		public Type AttributeType => Data[nameof(AttributeType)] as Type;
	}

	public class AttributeNotFoundException : AttributeException {
		public AttributeNotFoundException(string message = null, Exception innerException = null) : base(message, innerException) { }

		public AttributeNotFoundException(Type attributeType, string message = null, Exception innerException = null) : base(attributeType, message, innerException) { }

		protected override string DefaultMessage => $"Attribute {AttributeType.FullName} not found";
	}
}