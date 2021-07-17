using System.Collections.Generic;
using System.Linq;
using Kingdee.Requests;
using Kingdee.Utilities;

namespace Kingdee.Forms {
	public static class FormMeta<T> where T : FormBase {
		public static string Name => typeof(T).GetFormName();
		public static List<Field<T>> QueryFields => typeof(T).GetQueryFields(false).Select(field => new Field<T>(field)).ToList();
	}
}