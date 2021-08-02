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
	}
}