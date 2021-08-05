using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Kingdee.Requests.Query;
using Kingdee.Utilities;

namespace Kingdee.Forms {
	public static class FormMeta<T> where T : ErpModelBase {
		public static string Name => typeof(T).GetFormName();

		public static PropertyInfo Key => typeof(T).GetMostDerivedProperty(nameof(ErpModelBase.Id));

		public static List<Field<T>> QueryFields => typeof(T).GetQueryFields(false).Select(field => new Field<T>(field)).ToList();

		public static T CreateFromQueryFields(IEnumerable<Field> fields, IEnumerable<object> data) => (T)typeof(T).CreateFromQueryFields(fields, data);
	}
}