using System;
using System.Collections.Generic;
using System.Reflection;
using Kingdee.Forms;
using Kingdee.Requests;
using Shared.Utilities;

namespace Kingdee.Utilities {
	public static class Extension {
		public static FormAttribute GetFormAttribute(this Type type, bool verify = true) {
			if (verify)
				Utility.VerifyInheritance(type, typeof(FormBase));
			return type.GetCustomAttribute<FormAttribute>();
		}

		public static string GetFormName(this Type type, bool verify = true) => type.GetFormAttribute(verify)?.Name;

		private static List<Field> GetQueryFields(Type type, Field field) {
			var result = new List<Field>();
			foreach (var prop in type.GetProperties()) {
				if (prop.GetCustomAttribute<QueryIgnoreAttribute>() is not null)
					continue;
				var fld = field is null
					? new Field(prop.Name) {
						FormType = type
					}
					: field.Concat(prop.Name);
				if (Type.GetTypeCode(prop.PropertyType) == TypeCode.Object) {
					var propType = prop.PropertyType;
					if (propType.GetCustomAttribute<SubFormAttribute>() is null && propType.GetCustomAttribute<FormAttribute>() is null)
						continue;
					result.AddRange(GetQueryFields(propType, fld));
				}
				else
					result.Add(fld);
			}
			return result;
		}

		public static List<Field> GetQueryFields(this Type type, bool verify = true) {
			if (verify)
				Utility.VerifyInheritance(type, typeof(FormBase));
			return GetQueryFields(type, null);
		}
	}
}