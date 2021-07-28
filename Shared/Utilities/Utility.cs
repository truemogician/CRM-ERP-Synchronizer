using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Shared.Exceptions;
using Shared.Validation;

namespace Shared.Utilities {
	public static class Utility {
		public static void VerifyInheritance(Type derived, Type @base) {
			if (!derived.IsAssignableTo(@base))
				throw new InvariantTypeException(@base, derived);
		}

		public static List<ValidationResult> Validate<T>(T obj, bool recursive = false) {
			var results = new List<ValidationResult>();
			if (recursive)
				RecursiveValidator.TryValidateObjectRecursive(obj, results);
			else
				RecursiveValidator.TryValidateObject(obj, results);
			return results;
		}

		public static void ValidatedOrThrow<T>(T obj, bool recursive = false) {
			var results = Validate(obj, recursive);
			if (results.Count > 0)
				throw new ValidationFailedException(obj, results);
		}
	}
}