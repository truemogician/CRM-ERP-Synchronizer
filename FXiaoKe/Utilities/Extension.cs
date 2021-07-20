using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using FXiaoKe.Exceptions;
using FXiaoKe.Models;
using FXiaoKe.Request;
using Shared.Exceptions;
using Shared.Utilities;

namespace FXiaoKe.Utilities {
	public static class Extension {
		public static ModelAttribute GetModelAttribute(this Type type, bool verify = true) {
			if (verify)
				Utility.VerifyInheritance(type, typeof(ModelBase));
			return type.GetCustomAttribute<ModelAttribute>();
		}

		public static string GetModelName(this Type type) => type.GetModelAttribute()?.Name;

		public static bool IsCustomModel(this Type type) {
			var attr = type.GetModelAttribute();
			if (attr is null)
				throw new AttributeNotFoundException(typeof(ModelAttribute));
			return attr!.Custom;
		}

		public static MemberInfo GetKey(this Type type, bool verify = true) {
			if (verify)
				Utility.VerifyInheritance(type, typeof(ModelBase));
			MemberInfo result = null;
			var members = (type.GetProperties() as MemberInfo[]).Concat(type.GetFields()).ToList();
			foreach (var member in members.Where(member => member.GetCustomAttribute<KeyAttribute>() is { }))
				if (result is null)
					result = member;
				else
					throw new DuplicateKeyException(type);
			if (result is not null)
				return result;
			result = members.FirstOrDefault(member => member.Name.Equals("Id", StringComparison.OrdinalIgnoreCase));
			if (result is not null)
				return result;
			result = members.FirstOrDefault(member => member.Name.Equals($"{type.Name}Id", StringComparison.OrdinalIgnoreCase));
			return result ?? throw new MissingKeyException(type);
		}

		public static List<MemberInfo> GetSubModels(this Type type, bool verify = true) {
			if (verify)
				Utility.VerifyInheritance(type, typeof(ModelBase));
			var members = (type.GetProperties() as MemberInfo[]).Concat(type.GetFields()).ToList();
			return members.Where(member => member.IsDefined(typeof(SubModelAttribute))).ToList();
		}

		public static List<MemberInfo> GetCascadeSubModels(this Type type, bool verify = true)
			=> type.GetSubModels(verify)
				.Where(member => member.GetCustomAttribute<SubModelAttribute>()!.Cascade)
				.ToList();

		public static List<MemberInfo> GetEagerSubModels(this Type type, bool verify = true)
			=> type.GetSubModels(verify)
				.Where(member => member.GetCustomAttribute<SubModelAttribute>()!.Eager)
				.ToList();
	}
}