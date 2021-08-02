using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Shared.Exceptions;
using Shared.Validation;

namespace Shared.Extensions {
	public static class ValidationExtensions {
		public static List<ValidationResult> Validate<T>(this T obj, bool recursive = false) {
			var results = new List<ValidationResult>();
			if (recursive)
				RecursiveValidator.TryValidateObjectRecursive(obj, results);
			else
				RecursiveValidator.TryValidateObject(obj, results);
			return results;
		}

		public static void ValidatedOrThrow<T>(this T obj, bool recursive = false) {
			var results = Validate(obj, recursive);
			if (results.Count > 0)
				throw new ValidationFailedException(obj, results);
		}
	}
}