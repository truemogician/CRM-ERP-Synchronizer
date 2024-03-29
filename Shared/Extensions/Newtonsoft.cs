﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Newtonsoft.Json.Serialization;
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

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string GetJsonPropertyName(this Type type, string propertyName) => type.GetProperty(propertyName).GetJsonPropertyName();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static JsonPropertyAttribute GetJsonProperty(this MemberInfo member) => member.GetCustomAttribute<JsonPropertyAttribute>();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string GetJsonPropertyName(this MemberInfo member) => member.GetJsonProperty() is { } attr ? attr.PropertyName : member.Name;

		public static MemberInfo GetMemberFromJsonPropertyName(this Type type, string jsonPropertyName) {
			var members = type.GetMembers(MemberTypes.Field | MemberTypes.Property);
			return members.SingleOrDefault(prop => prop.GetJsonPropertyName() == jsonPropertyName);
		}
	}

	public static class JsonConverterExtensions {
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void WriteValue<T>(this JsonWriter writer, T value) => writer.WriteRawValue(JsonConvert.SerializeObject(value));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void WriteValue<T>(this JsonWriter writer, T value, JsonSerializer serializer) => serializer.Serialize(writer, value, typeof(T));
	}

	public static class ContractResolverExtensions {
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsDefined<T>(this JsonProperty property) where T : Attribute => property.IsDefined(typeof(T));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsDefined(this JsonProperty property, Type attributeType) {
			var attrs = property.AttributeProvider!.GetAttributes(attributeType, true);
			return attrs.Count > 0;
		}
	}

	#nullable enable
	public static class JsonSerializerExtensions {
		public static string Serialize(this JsonSerializer serializer, object? value) {
			var builder = new StringBuilder();
			var stringWriter = new StringWriter(builder);
			serializer.Serialize(stringWriter, value);
			return builder.ToString();
		}

		public static T? Deserialize<T>(this JsonSerializer serializer, string json) {
			var stringReader = new StringReader(json);
			return serializer.Deserialize<T>(new JsonTextReader(stringReader));
		}

		public static object? Deserialize(this JsonSerializer serializer, Type objectType, string json) {
			var stringReader = new StringReader(json);
			return serializer.Deserialize(new JsonTextReader(stringReader), objectType);
		}
	}
}