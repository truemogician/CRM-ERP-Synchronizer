using System;

namespace Kingdee {
	internal static class TypeExt {
		public static bool IsSimpleType(this Type type) => type.IsValueType || type == typeof(string);
	}
}