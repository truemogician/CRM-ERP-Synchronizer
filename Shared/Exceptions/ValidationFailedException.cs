using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shared.Exceptions {
	public class ValidationFailedException : Exception {
		public ValidationFailedException(string message = null, Exception innerException = null) : base(message, innerException) { }

		public ValidationFailedException(IEnumerable<ValidationResult> results, string message = null, Exception innerException = null) : this(message, innerException) => ValidationResults = results;

		public IEnumerable<ValidationResult> ValidationResults { get; }
	}
}