using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace Shared.Utilities {
	public static class Extension {
		public static List<T> GetAttributes<T>(this PropertyInfo property) where T : Attribute => property.GetCustomAttributes(typeof(T), true).Cast<T>().ToList();

		public static T GetAttribute<T>(this PropertyInfo property) where T : Attribute => property.GetAttributes<T>().FirstOrDefault();

		public static List<TResult> GetAttributeValues<TAttribute, TResult>(this Type type, Func<TAttribute, TResult> selector) where TAttribute : Attribute {
			var properties = type.GetProperties();
			return properties.Select(p => selector(p.GetAttribute<TAttribute>())).ToList();
		}

		public static List<string> GetJsonFields(this Type type) => type.GetAttributeValues<JsonPropertyAttribute, string>(p => p.PropertyName);
	}
}