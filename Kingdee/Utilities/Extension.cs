using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using Kingdee.Forms;
using Kingdee.Requests;
using Shared.Exceptions;
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

		private static object SetQueryFields(object obj, Type type, List<Field> fields, ArraySegment<object> data) {
			if (fields.Count != data.Count)
				throw new Exception($"Count of {nameof(fields)} doesn't equal to that of {nameof(data)}");
			obj ??= type.Construct();
			//fields.Sort((a,b)=>string.Compare(a.ToString(), b.ToString(), StringComparison.Ordinal));
			for (int i = 0, delta; i < fields.Count; i += delta) {
				delta = 1;
				(var field, object datum) = (fields[i], data[i]);
				if (field.Length == 1)
					field.SetValue(obj, datum);
				else {
					for (int j = i + 1; j < fields.Count && fields[j].PropertyNames[0] == field.PropertyNames[0]; ++j, ++delta) { }
					var stProp = field.StartingProperty;
					if (stProp.GetValue(obj) is null)
						stProp.SetValue(obj, stProp.PropertyType.Construct());
					SetQueryFields(
						stProp.GetValue(obj),
						stProp.PropertyType,
						fields.GetRange(i, delta).Select(f => f[1..]).ToList(),
						data[i..(i + delta)]
					);
				}
			}
			return obj;
		}

		public static object SetQueryFields(this object obj, IEnumerable<Field> fields, IEnumerable<object> data) => SetQueryFields(obj, obj.GetType(), fields.ToList(), data.ToArray());

		public static object CreateFromQueryFields(this Type type, IEnumerable<Field> fields, IEnumerable<object> data) => SetQueryFields(null, type, fields.ToList(), data.ToArray());

		public static List<Field> GetQueryFields(this Type type, bool verify = true) {
			if (verify)
				Utility.VerifyInheritance(type, typeof(FormBase));
			return GetQueryFields(type, null);
		}
	}
}