using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Shared.Exceptions {
	public class JTokenException : ExceptionWithDefaultMessage {
		public JTokenException(string message = null, Exception innerException = null) : base(message, innerException) { }

		public JTokenException(JToken token, string message = null, Exception innerException = null) : this(message, innerException) => Token = token;

		public JToken Token { get; set; }
	}

	public class JTokenTypeException : JTokenException {
		public JTokenTypeException(string message = null, Exception innerException = null) : base(message, innerException) { }

		public JTokenTypeException(JToken actual, JTokenType expected, string message = null, Exception innerException = null) : base(actual, message, innerException) => Expected.Add(expected);

		public JTokenTypeException(JToken actual, params JTokenType[] expected) : base(actual) => Expected.AddRange(expected);

		public List<JTokenType> Expected { get; } = new();

		protected override string DefaultMessage => $"Expecting {string.Join(", ", Expected)}, but {Token?.Type} received";
	}
}