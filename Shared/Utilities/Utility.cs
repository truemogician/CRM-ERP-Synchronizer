using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Shared.Exceptions;

namespace Shared.Utilities {
	public static class Utility {
		public static void VerifyInheritance(Type derived, Type super) {
			if (!derived.IsSubclassOf(super))
				throw new TypeException(derived, $"Type \"{derived.FullName}\" should derive from \"{super.FullName}\"");
		}

		public static List<ValidationResult> Validate<T>(T obj) {
			var valContext = new ValidationContext(obj, null, null);
			var result = new List<ValidationResult>();
			Validator.TryValidateObject(obj, valContext, result, true);
			return result;
		}

		public static void ValidatedOrThrow<T>(T obj) {
			var results = Validate(obj);
			if (results.Count > 0)
				throw new ValidationFailedException(results);
		}

		public static T Construct<T>(params object[] parameters) => (T)typeof(T).Construct(parameters);
	}
}