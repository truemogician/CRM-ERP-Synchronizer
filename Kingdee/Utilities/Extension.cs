using System;
using System.Reflection;
using Kingdee.Forms;
using static Shared.Utilities.Extension;

namespace Kingdee.Utilities {
	public static class Extension {
		public static FormAttribute GetFormAttribute(this Type type, bool verify = true) {
			if (verify)
				VerifyInheritance(type, typeof(FormBase));
			return type.GetCustomAttribute<FormAttribute>();
		}

		public static string GetFormName(this Type type) => type.GetFormAttribute()?.Name;
	}
}