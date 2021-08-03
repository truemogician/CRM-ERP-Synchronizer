using System;
using Shared.Exceptions;

namespace Shared.Utilities {
	public static class Utility {
		public static void VerifyInheritance(Type derived, Type @base) {
			if (!derived.IsAssignableTo(@base))
				throw new InvariantTypeException(@base, derived);
		}
	}
}