using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;
using Kingdee.Forms;
using Kingdee.Requests;
using Shared;
using Shared.Exceptions;
using Shared.Utilities;
using JsonIgnoreAttribute = Newtonsoft.Json.JsonIgnoreAttribute;

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
			var props = type.GetProperties();
			bool blackList = props.Any(prop => prop.IsDefined(typeof(JsonIgnoreAttribute)));
			bool whiteList = props.Any(prop => prop.IsDefined(typeof(JsonIncludeAttribute)));
			if (blackList && whiteList)
				throw new Exception($"Mixed use of {nameof(JsonIgnoreAttribute)} and {nameof(JsonIncludeAttribute)}");
			foreach (var prop in props.Select(p => new EnumerablePropertyInfo(p))) {
				if (blackList && prop.IsDefined(typeof(JsonIgnoreAttribute)) || whiteList && !prop.IsDefined(typeof(JsonIncludeAttribute)))
					continue;
				var fld = field is null
					? new Field(prop.Name) {
						FormType = type
					}
					: field.Concat(prop.Name);
				if (prop.PropertyType.IsValueType || prop.PropertyType == typeof(string))
					result.Add(fld);
				else
					result.AddRange(GetQueryFields(prop.ElementType, fld));
			}
			return result;
		}

		private static object SetQueryFields(object obj, Type type, List<Field> fields, ArraySegment<object> data) {
			if (fields.Count != data.Count)
				throw new Exception($"Count of {nameof(fields)} doesn't equal to that of {nameof(data)}");
			obj ??= type.Construct();
			for (int i = 0, delta; i < fields.Count; i += delta) {
				delta = 1;
				(var field, object datum) = (fields[i], data[i]);
				var stProp = field.StartingInfo;
				object targetObj = obj;
				if (field.Length > 1) {
					if (stProp.GetValue(targetObj) is null)
						stProp.SetValue(targetObj, stProp.PropertyType.Construct());//ToDo: interface construction
					targetObj = stProp.GetValue(targetObj);
				}
				for (int k = 0; k < stProp.Rank; ++k) {
					if (targetObj?.GetType().Implements(typeof(ICollection<>)) != true)
						throw new InterfaceNotImplementedException(typeof(ICollection<>));
					var collectionType = targetObj.GetType().GetGenericInterface(typeof(ICollection<>));
					if ((int)collectionType.GetProperty(nameof(ICollection<object>.Count))!.GetValue(targetObj)! > 0) {
						dynamic enumerator = collectionType.GetMethod(nameof(ICollection<object>.GetEnumerator)).Invoke(targetObj);
						enumerator.MoveNext();
						targetObj = enumerator.Current;
					}
					else {
						var itemType = targetObj.GetType().GetItemType();
						object newItem = itemType.Construct();
						collectionType.GetMethod(nameof(ICollection<object>.Add)).Invoke(targetObj, newItem);
						targetObj = newItem;
					}
				}
				if (field.Length == 1) {
					if (stProp.Rank > 0)
						((dynamic)targetObj)!.Add(datum);
					else
						field.EndingInfo.SetValueWithConversion(targetObj, datum);
				}
				else {
					for (int j = i + 1; j < fields.Count && fields[j].Names[0] == field.Names[0]; ++j, ++delta) { }
					SetQueryFields(
						targetObj,
						stProp.ElementType,
						fields.GetRange(i, delta).Select(f => f[1..]).AsList(),
						data[i..(i + delta)]
					);
				}
			}
			return obj;
		}

		public static object SetQueryFields(this object obj, IEnumerable<Field> fields, IEnumerable<object> data) => SetQueryFields(obj, obj.GetType(), fields.AsList(), data.AsArray());

		public static object CreateFromQueryFields(this Type type, IEnumerable<Field> fields, IEnumerable<object> data) => SetQueryFields(null, type, fields.AsList(), data.AsArray());

		public static List<Field> GetQueryFields(this Type type, bool verify = true) {
			if (verify)
				Utility.VerifyInheritance(type, typeof(FormBase));
			return GetQueryFields(type, null);
		}
	}
}