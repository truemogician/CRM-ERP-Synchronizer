using System;

namespace Shared.Exceptions {
	public class AttributeNotFoundException : ExceptionWithDefaultMessage {
		public AttributeNotFoundException(string message = null, Exception innerException = null) : base(message, innerException) { }
		public AttributeNotFoundException(Type attributeType, string message = null, Exception innerException = null) : this(message, innerException) => Data[nameof(AttributeType)] = attributeType;
		public Type AttributeType => Data[nameof(AttributeType)] as Type;
		protected override string DefaultMessage => $"Attribute {AttributeType.FullName} not found";
	}
}