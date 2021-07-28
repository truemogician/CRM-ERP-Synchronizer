using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Shared.Validation {
	public static class RecursiveValidator {
		public static bool TryValidateObject(object obj, ICollection<ValidationResult> results, IDictionary<object, object> validationContextItems = null) => Validator.TryValidateObject(obj, new ValidationContext(obj, null, validationContextItems), results, true);

		public static bool TryValidateObjectRecursive<T>(T obj, ICollection<ValidationResult> results, IDictionary<object, object> validationContextItems = null) => TryValidateObjectRecursive(obj, results, new HashSet<object>(), validationContextItems);

		private static bool TryValidateObjectRecursive<T>(T obj, ICollection<ValidationResult> results, ISet<object> validatedObjects, IDictionary<object, object> validationContextItems = null) {
			//short-circuit to avoid infinite loops on cyclical object graphs
			if (validatedObjects.Contains(obj))
				return true;

			validatedObjects.Add(obj);
			bool result = TryValidateObject(obj, results, validationContextItems);
			var type = obj.GetType();
			if (type.Module.ScopeName.StartsWith("System."))
				return result;
			var properties = type.GetProperties()
				.Where(
					prop => prop.CanRead &&
						!prop.PropertyType.Module.ScopeName.StartsWith("System.") &&
						!prop.IsDefined(typeof(ValidationIgnoreAttribute)) &&
						!prop.PropertyType.IsDefined(typeof(ValidationIgnoreAttribute)) &&
						!prop.IsIndexer()
				)
				.ToList();
			foreach (var property in properties) {
				if (property.PropertyType == typeof(string) || property.PropertyType.IsPrimitive)
					continue;
				object value = property.GetValue(obj);
				if (value is null)
					continue;
				if (value.GetType().Implements(typeof(ICollection<>))) {
					var enumerable = value as IEnumerable;
					foreach (object item in enumerable!)
						if (item is not null) {
							var nestedResults = new List<ValidationResult>();
							if (!TryValidateObjectRecursive(item, nestedResults, validatedObjects, validationContextItems)) {
								result = false;
								results.AddRange(
									from validationResult in nestedResults
									let prop = property
									select new ValidationResult(
										validationResult.ErrorMessage,
										validationResult.MemberNames.Select(x => prop.Name + '.' + x)
									)
								);
							}
						}
				}
				else {
					var nestedResults = new List<ValidationResult>();
					if (!TryValidateObjectRecursive(value, nestedResults, validatedObjects, validationContextItems)) {
						result = false;
						results.AddRange(
							from validationResult in nestedResults
							let prop = property
							select new ValidationResult(
								validationResult.ErrorMessage,
								validationResult.MemberNames.Select(x => prop.Name + '.' + x)
							)
						);
					}
				}
			}
			return result;
		}
	}
}