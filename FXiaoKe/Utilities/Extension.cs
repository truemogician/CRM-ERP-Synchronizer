using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FXiaoKe.Exceptions;
using FXiaoKe.Models;
using FXiaoKe.Request;
using Shared.Exceptions;

namespace FXiaoKe.Utilities {
	public static class Extension {
		private static void VerifyInheritance(Type derived, Type super) {
			if (!derived.IsSubclassOf(super))
				throw new TypeReflectionException(derived, $"Type \"{derived.FullName}\" should derive from \"{super.FullName}\"");
		}

		public static ModelAttribute GetModelAttribute(this Type type, bool verify = true) {
			if (verify)
				VerifyInheritance(type, typeof(ModelBase));
			return type.GetCustomAttribute<ModelAttribute>();
		}

		public static string GetModelName(this Type type) => type.GetModelAttribute()?.Name;

		public static bool IsCustomModel(this Type type) {
			var attr = type.GetModelAttribute();
			if (attr is null)
				throw new AttributeNotFoundException(typeof(ModelAttribute));
			return attr!.Custom;
		}

		public static MemberInfo GetPrimaryKey(this Type type, bool verify = true) {
			if (verify)
				VerifyInheritance(type, typeof(ModelBase));
			MemberInfo result = null;
			var members = (type.GetProperties() as MemberInfo[]).Concat(type.GetFields()).ToList();
			foreach (var member in members.Where(member => member.GetCustomAttribute<PrimaryKeyAttribute>() is { }))
				if (result is null)
					result = member;
				else
					throw new DuplicatePrimaryKeyException(type);
			if (result is not null)
				return result;
			result = members.FirstOrDefault(member => member.Name.Equals("Id", StringComparison.OrdinalIgnoreCase));
			if (result is not null)
				return result;
			result = members.FirstOrDefault(member => member.Name.Equals($"{type.Name}Id", StringComparison.OrdinalIgnoreCase));
			return result ?? throw new MissingPrimaryKeyException(type);
		}

		public static List<RequestAttribute> GetRequestAttributes(this Type type) => type.GetCustomAttributes<RequestAttribute>().ToList();
	}
}