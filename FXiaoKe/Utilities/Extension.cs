using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using FXiaoKe.Exceptions;
using FXiaoKe.Models;
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

		#nullable enable
		public static MemberInfo? GetKey(this Type type, bool verify = true) {
			if (verify)
				Utility.VerifyInheritance(type, typeof(ModelBase));
			var members = type.GetMembers(MemberTypes.Property | MemberTypes.Field);
			var result = members.SingleOrDefault(member => member.IsDefined(typeof(KeyAttribute)));
			if (result is not null)
				return result;
			result = members.FirstOrDefault(member => member.Name.Equals("Id", StringComparison.OrdinalIgnoreCase));
			if (result is not null)
				return result;
			result = members.FirstOrDefault(member => member.Name.Equals($"{type.Name}Id", StringComparison.OrdinalIgnoreCase));
			return result;
		}
		#nullable disable

		public static List<MemberInfo> GetSubModelMembers(this Type type, bool? eager = null, bool? cascade = null) {
			Utility.VerifyInheritance(type, typeof(CrmModelBase));
			var members = type.GetMembers(MemberTypes.Field | MemberTypes.Property);
			return members.Where(
					member => {
						var attr = member.GetCustomAttribute<SubModelAttribute>();
						return attr is not null &&
							(!eager.HasValue || eager.Value == attr.Eager) &&
							(!cascade.HasValue || cascade.Value == attr.Cascade);
					}
				)
				.AsList();
		}
	}
}