using System;
using System.Linq;
using System.Net.Http;
using System.Reflection;

namespace FXiaoKe.Utilities {
	public static class Extension {
		public static string GetModelName(this Type type) => type.GetCustomAttribute<ModelAttribute>()?.Name;
	}
}