using System;

namespace Shared.Exceptions {
	public class AttributeNotFoundException : Exception {
		public AttributeNotFoundException(string message = null, Exception innerException = null) : base(message, innerException) { }
		public AttributeNotFoundException(Type attributeType, string message = null, Exception innerException = null) : this(message, innerException) => AttributeType = attributeType;
		public Type AttributeType { get; set; }
	}
}