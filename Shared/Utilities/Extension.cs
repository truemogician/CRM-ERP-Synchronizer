using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Shared.Exceptions;

namespace Shared.Utilities {
	public static class Extension {
		public static object GetValue(this MemberInfo info, object obj)
			=> info switch {
				FieldInfo field       => field.GetValue(obj),
				PropertyInfo property => property.GetValue(obj),
				_                     => throw new Exception($"Cannot get value from {info.Name}")
			};

		public static void SetValue(this MemberInfo info, object obj, object value) {
			switch (info) {
				case FieldInfo field:
					field.SetValue(obj, value);
					break;
				case PropertyInfo property:
					property.SetValue(obj, value);
					break;
				default: throw new Exception($"Cannot get value from {info.Name}");
			}
		}

		public static bool Implements(this Type type, Type interfaceType)
			=> (interfaceType.IsGenericTypeDefinition
				? type.GetGenericInterface(interfaceType)
				: type.GetInterface(interfaceType.Name)) is not null;

		public static Type GetGenericInterface(this Type type, Type genericTypeDefinition)
			=> type.GetInterfaces()
				.SingleOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == genericTypeDefinition);

		public static Type[] GetGenericInterfaceArguments(this Type type, Type genericTypeDefinition) => type.GetGenericInterface(genericTypeDefinition)?.GetGenericArguments();

		public static List<PropertyInfo> GetIndexers(this Type type) => type.GetProperties().Where(p => p.GetIndexParameters().Length > 0).ToList();

		public static PropertyInfo GetIndexer(this Type type, params Type[] parameterTypes)
			=> type.GetProperties()
				.SingleOrDefault(
					p => p.GetIndexParameters() is var args &&
						args.Length == parameterTypes.Length &&
						parameterTypes.Select((t, index) => t == args[index].ParameterType).All(x => x)
				);

		public static List<MemberInfo> GetMembersWithAttribute(this Type type, Type attributeType) => type.GetMembers().Where(member => member.IsDefined(attributeType)).ToList();

		public static List<MemberInfo> GetMembersWithAttribute<T>(this Type type) where T : Attribute => type.GetMembersWithAttribute(typeof(T));

		public static MemberInfo GetMemberWithAttribute(this Type type, Type attributeType) => type.GetMembersWithAttribute(attributeType).SingleOrDefault();

		public static MemberInfo GetMemberWithAttribute<T>(this Type type) where T : Attribute => type.GetMembersWithAttribute<T>().SingleOrDefault();

		public static List<T> GetAttributes<T>(this PropertyInfo property) where T : Attribute => property.GetCustomAttributes(typeof(T), true).Cast<T>().ToList();

		public static T GetAttribute<T>(this PropertyInfo property) where T : Attribute => property.GetAttributes<T>().FirstOrDefault();

		public static List<TResult> GetAttributeValues<TAttribute, TResult>(this Type type, Func<TAttribute, TResult> selector) where TAttribute : Attribute {
			var properties = type.GetProperties();
			return properties.Select(p => selector(p.GetAttribute<TAttribute>())).ToList();
		}

		public static Type GetPropertyType(this Type type, string propertyName) => type.GetProperty(propertyName)?.PropertyType;

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

		public static JsonPropertyAttribute GetJsonProperty(this PropertyInfo property) => property.GetCustomAttribute<JsonPropertyAttribute>();

		public static string GetJsonPropertyName(this PropertyInfo property) => property.GetJsonProperty() is { } attr ? attr.PropertyName : property.Name;

		public static PropertyInfo GetPropertyFromJsonPropertyName(this Type type, string jsonPropertyName) {
			var props = type.GetProperties(BindingFlags.Public);
			return props.FirstOrDefault(prop => prop.GetJsonPropertyName() == jsonPropertyName);
		}

		public static object Construct(this Type type, params object[] parameters) {
			var constructor = type.GetConstructor(parameters.Select(p => p.GetType()).ToArray());
			if (constructor is null)
				throw new TypeException(type, $"{type.FullName} can't be constructed with such parameters");
			return constructor.Invoke(parameters);
		}

		public static void SetValueWithConversion(this MemberInfo info, object obj, object value) {
			(Action<object, object> setValue, Type type) = info switch {
				FieldInfo field   => ((Action<object, object>)field.SetValue, field.FieldType),
				PropertyInfo prop => (prop.SetValue, prop.PropertyType),
				_                 => throw new Exception("Property or field expected")
			};
			if (type.IsInstanceOfType(value) || value is null)
				setValue(obj, value);
			else if (type.Implements(typeof(IConvertible)) && value.GetType().Implements(typeof(IConvertible)))
				setValue(
					obj,
					Type.GetTypeCode(type) switch {
						TypeCode.Boolean  => Convert.ToBoolean(value),
						TypeCode.SByte    => Convert.ToSByte(value),
						TypeCode.Byte     => Convert.ToByte(value),
						TypeCode.Int16    => Convert.ToInt16(value),
						TypeCode.UInt16   => Convert.ToUInt16(value),
						TypeCode.Int32    => Convert.ToInt32(value),
						TypeCode.UInt32   => Convert.ToUInt32(value),
						TypeCode.Int64    => Convert.ToInt64(value),
						TypeCode.UInt64   => Convert.ToUInt64(value),
						TypeCode.Single   => Convert.ToSingle(value),
						TypeCode.Double   => Convert.ToDouble(value),
						TypeCode.Decimal  => Convert.ToDecimal(value),
						TypeCode.DateTime => Convert.ToDateTime(value),
						TypeCode.Char     => Convert.ToChar(value),
						TypeCode.String   => Convert.ToString(value),
						_                 => throw new EnumValueOutOfRangeException<TypeCode>(Type.GetTypeCode(type))
					}
				);
			else
				throw new InterfaceNotImplementedException(typeof(IConvertible));
		}

		public static List<T> AsList<T>(this IEnumerable<T> enumerable) => enumerable is List<T> list ? list : enumerable.ToList();

		public static T[] AsArray<T>(this IEnumerable<T> enumerable) => enumerable is T[] array ? array : enumerable.ToArray();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="type"></param>
		/// <param name="genericTypeDefinition">The generic interface <paramref name="type"/> is required to implement. Default is <see cref="IEnumerable{}"/></param>
		/// <returns></returns>
		public static Type GetItemType(this Type type, Type genericTypeDefinition = null) {
			genericTypeDefinition ??= typeof(IEnumerable<>);
			if (!genericTypeDefinition.IsGenericTypeDefinition)
				throw new TypeException(genericTypeDefinition, "Required to be a generic type definition");
			if (!type.Implements(genericTypeDefinition))
				return null;
			return type.HasElementType ? type.GetElementType() : type.GetGenericInterfaceArguments(genericTypeDefinition).Single();
		}
	}
}