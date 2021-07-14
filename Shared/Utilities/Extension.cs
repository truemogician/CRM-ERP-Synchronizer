using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Shared.Exceptions;

namespace Shared.Utilities {
	public static class Extension {
		public static List<T> GetAttributes<T>(this PropertyInfo property) where T : Attribute => property.GetCustomAttributes(typeof(T), true).Cast<T>().ToList();

		public static T GetAttribute<T>(this PropertyInfo property) where T : Attribute => property.GetAttributes<T>().FirstOrDefault();

		public static List<TResult> GetAttributeValues<TAttribute, TResult>(this Type type, Func<TAttribute, TResult> selector) where TAttribute : Attribute {
			var properties = type.GetProperties();
			return properties.Select(p => selector(p.GetAttribute<TAttribute>())).ToList();
		}

		public static List<string> GetJsonPropertyNames(this Type type) {
			var properties = type.GetProperties(BindingFlags.Public);
			return properties.Select(
					prop => {
						var attr = prop.GetJsonProperty();
						return attr is null ? prop.Name : attr.PropertyName;
					}
				)
				.ToList();
		}

		public static JsonPropertyAttribute GetJsonProperty(this Type type, string propertyName) {
			var prop = type.GetProperty(propertyName);
			if (prop is null)
				throw new TypeReflectionException(type, $"Property \"{propertyName}\" doesn't exist in type \"{type.Name}\"");
			return prop.GetCustomAttribute<JsonPropertyAttribute>();
		}

		public static JsonPropertyAttribute GetJsonProperty(this PropertyInfo property) => property.GetCustomAttribute<JsonPropertyAttribute>();

		public static string GetJsonPropertyName(this Type type, string propertyName) {
			var attr = type.GetJsonProperty(propertyName);
			return attr is null ? propertyName : attr.PropertyName;
		}

		public static PropertyInfo GetPropertyFromJsonPropertyName(this Type type, string jsonPropertyName) {
			var props = type.GetProperties(BindingFlags.Public);
			return props.FirstOrDefault(
				prop => {
					var attr = prop.GetJsonProperty();
					return (attr is null ? prop.Name : attr.PropertyName) == jsonPropertyName;
				}
			);
		}
	}
}