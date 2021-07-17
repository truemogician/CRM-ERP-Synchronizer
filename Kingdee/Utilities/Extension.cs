using System;
using System.Reflection;
using Kingdee.Forms;
using Shared.Utilities;

namespace Kingdee.Utilities {
	public static class Extension {
		public static FormAttribute GetFormAttribute(this Type type, bool verify = true) {
			if (verify)
				Utility.VerifyInheritance(type, typeof(FormBase));
			return type.GetCustomAttribute<FormAttribute>();
		}

		public static string GetFormName(this Type type) => type.GetFormAttribute()?.Name;
	}
}