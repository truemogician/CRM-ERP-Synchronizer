using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Shared.Exceptions {
	public class ExceptionWithDefaultMessage : Exception {
		[JsonIgnore]
		private readonly bool _useDefaultMessage;

		protected ExceptionWithDefaultMessage(string message = null, Exception innerException = null) : base(message, innerException) => _useDefaultMessage = message is null;

		protected ExceptionWithDefaultMessage([NotNull] SerializationInfo info, StreamingContext context) : base(info, context) { }

		[JsonIgnore]
		protected virtual string DefaultMessage { get; } = null;

		public sealed override string Message => _useDefaultMessage && DefaultMessage is not null ? DefaultMessage : base.Message;

		protected object this[object key] {
			get => Data.Contains(key) ? Data[key] : null;
			set => Data[key] = value;
		}

		protected T Get<T>(object key) => this[key] is T result ? result : default;

		protected void Set<T>(object key, T value) => this[key] = value;
	}
}