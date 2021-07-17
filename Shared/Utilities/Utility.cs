using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Shared.Exceptions;

namespace Shared.Utilities {
	public static class Utility {
		public static void VerifyInheritance(Type derived, Type super) {
			if (!derived.IsSubclassOf(super))
				throw new TypeReflectionException(derived, $"Type \"{derived.FullName}\" should derive from \"{super.FullName}\"");
		}

		public static List<ValidationResult> Validate<T>(T obj) {
			var valContext = new ValidationContext(obj, null, null);
			var result = new List<ValidationResult>();
			Validator.TryValidateObject(obj, valContext, result, true);
			return result;
		}
	}
}