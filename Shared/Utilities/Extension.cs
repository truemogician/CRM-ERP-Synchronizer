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
				_                     => throw new MemberTypeException(info, MemberTypes.Property | MemberTypes.Field)
			};

		public static void SetValue(this MemberInfo info, object obj, object value) {
			switch (info) {
				case FieldInfo field:
					field.SetValue(obj, value);
					break;
				case PropertyInfo property:
					property.SetValue(obj, value);
					break;
				default: throw new MemberTypeException(info, MemberTypes.Property | MemberTypes.Field);
			}
		}

		public static Type GetValueType(this MemberInfo info)
			=> info switch {
				FieldInfo field             => field.FieldType,
				PropertyInfo property       => property.PropertyType,
				MethodInfo method           => method.ReturnType,
				ConstructorInfo constructor => constructor.DeclaringType,
				EventInfo @event            => @event.EventHandlerType,
				_                           => info.ReflectedType
			};

		public static bool Implements(this Type type, Type interfaceType)
			=> (interfaceType.IsGenericTypeDefinition
				? type.GetGenericInterface(interfaceType)
				: type.GetInterface(interfaceType.Name)) is not null;

		public static Type GetGenericInterface(this Type type, Type genericTypeDefinition)
			=> type.GetInterfaces()
				.SingleOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == genericTypeDefinition);

		public static Type[] GetGenericInterfaceArguments(this Type type, Type genericTypeDefinition) => type.GetGenericInterface(genericTypeDefinition)?.GetGenericArguments();

		public static bool IsAssignableToGeneric(this Type type, Type genericTypeDeclaration) {
			while (type != null && type != typeof(object)) {
				var cur = type.IsGenericType ? type.GetGenericTypeDefinition() : type;
				if (genericTypeDeclaration == cur)
					return true;
				type = type.BaseType;
			}
			return false;
		}

		public static List<PropertyInfo> GetIndexers(this Type type) => type.GetProperties().Where(p => p.GetIndexParameters().Length > 0).ToList();

		public static PropertyInfo GetIndexer(this Type type, params Type[] parameterTypes)
			=> type.GetProperties()
				.SingleOrDefault(
					p => p.GetIndexParameters() is var args &&
						args.Length == parameterTypes.Length &&
						parameterTypes.Select((t, index) => t == args[index].ParameterType).All(x => x)
				);

		public static List<MemberInfo> GetMembersWithAttributes(this Type type, params Type[] attributeTypes) => type.GetMembers().Where(member => attributeTypes.All(member.IsDefined)).ToList();

		public static List<MemberInfo> GetMembersWithAttribute<T>(this Type type) where T : Attribute => type.GetMembersWithAttributes(typeof(T));

		public static MemberInfo GetMemberWithAttributes(this Type type, params Type[] attributeTypes) => type.GetMembersWithAttributes(attributeTypes).SingleOrDefault();

		public static MemberInfo GetMemberWithAttribute<T>(this Type type) where T : Attribute => type.GetMembersWithAttribute<T>().SingleOrDefault();

		public static List<(MemberInfo Member, List<Attribute> Attributes)> GetMembersAndAttributes(this Type type, params Type[] attributeTypes) => type.GetMembers().Where(member => attributeTypes.All(member.IsDefined)).Select(member => (member, attributeTypes.Select(member.GetCustomAttribute).AsList())).AsList();

		public static List<(MemberInfo Member, T Attribute)> GetMembersAndAttribute<T>(this Type type) where T : Attribute => type.GetMembersAndAttributes(typeof(T)).Select(result => (result.Member, result.Attributes.Single() as T)).AsList();

		public static (MemberInfo Member, List<Attribute> Attributes) GetMemberAndAttributes(this Type type, params Type[] attributeTypes) => type.GetMembersAndAttributes(attributeTypes).SingleOrDefault();

		public static (MemberInfo Member, T Attribute) GetMemberAndAttribute<T>(this Type type) where T : Attribute => type.GetMembersAndAttribute<T>().SingleOrDefault();

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
				setValue(obj, Convert.ChangeType(value, type));
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

		public static List<MemberInfo> GetMembers(this Type type, MemberTypes memberTypes) {
			var result = new List<MemberInfo>();
			if (memberTypes.HasFlag(MemberTypes.Property))
				result.AddRange(type.GetProperties());
			if (memberTypes.HasFlag(MemberTypes.Field))
				result.AddRange(type.GetFields());
			if (memberTypes.HasFlag(MemberTypes.Constructor))
				result.AddRange(type.GetConstructors());
			if (memberTypes.HasFlag(MemberTypes.Event))
				result.AddRange(type.GetEvents());
			if (memberTypes.HasFlag(MemberTypes.Method))
				result.AddRange(type.GetMethods());
			if (memberTypes.HasFlag(MemberTypes.NestedType))
				result.AddRange(type.GetNestedTypes());
			return result;
		}
	}
}