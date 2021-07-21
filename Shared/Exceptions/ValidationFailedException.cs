using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Shared.Exceptions {
	public class ValidationFailedException : ExceptionWithDefaultMessage {
		public ValidationFailedException(string message = null, Exception innerException = null) : base(message, innerException) { }

		public ValidationFailedException(object obj, IEnumerable<ValidationResult> results, string message = null, Exception innerException = null) : this(message, innerException) {
			this[nameof(SourceObject)] = obj;
			this[nameof(Results)] = results.AsList();
		}

		public object SourceObject => Get<object>(nameof(SourceObject));

		public List<ValidationResult> Results => Get<List<ValidationResult>>(nameof(Results));

		protected override string DefaultMessage => $"Validation on {SourceObject} failed with {Results.Count} errors";
	}
}