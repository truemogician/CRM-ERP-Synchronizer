using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Shared.Exceptions;

// ReSharper disable once CheckNamespace
namespace Newtonsoft.Json {
	public static class ReflectionExtensions {
		public static List<string> GetJsonPropertyNames(this Type type) {
			var properties = type.GetProperties(BindingFlags.Public);
			return properties.Select(
					prop => prop.GetJsonPropertyName()
				)
				.ToList();
		}

		public static JsonPropertyAttribute GetJsonProperty(this Type type, string propertyName) {
			var prop = type.GetProperty(propertyName);
			if (prop is null)
				throw new TypeException(type, $"Property \"{propertyName}\" doesn't exist in type \"{type.Name}\"");
			return prop.GetCustomAttribute<JsonPropertyAttribute>();
		}

		public static string GetJsonPropertyName(this Type type, string propertyName) => type.GetProperty(propertyName).GetJsonPropertyName();

		public static JsonPropertyAttribute GetJsonProperty(this MemberInfo member) => member.GetCustomAttribute<JsonPropertyAttribute>();

		public static string GetJsonPropertyName(this MemberInfo member) => member.GetJsonProperty() is { } attr ? attr.PropertyName : member.Name;

		public static MemberInfo GetMemberFromJsonPropertyName(this Type type, string jsonPropertyName) {
			var members = type.GetMembers(MemberTypes.Field | MemberTypes.Property);
			return members.SingleOrDefault(prop => prop.GetJsonPropertyName() == jsonPropertyName);
		}
	}
}