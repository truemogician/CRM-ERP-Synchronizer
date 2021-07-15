using System.Reflection;
using FXiaoKe.Models;

namespace FXiaoKe.Utilities {
	public static class ModelMeta<T> where T : ModelBase {
		public static ModelAttribute ModelAttribute => typeof(T).GetModelAttribute(false);

		public static string ModelName => ModelAttribute.Name;

		public static bool IsCustomModel => ModelAttribute.Custom;

		public static MemberInfo PrimaryKey => typeof(T).GetPrimaryKey(false);
	}
}