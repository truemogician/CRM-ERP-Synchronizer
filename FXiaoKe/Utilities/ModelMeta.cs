using System.Collections.Generic;
using System.Reflection;
using FXiaoKe.Models;
using Shared.Utilities;

namespace FXiaoKe.Utilities {
	public static class ModelMeta<T> where T : ModelBase {
		public static ModelAttribute ModelAttribute => typeof(T).GetModelAttribute(false);

		public static string ModelName => ModelAttribute.Name;

		public static bool IsCustomModel => ModelAttribute.Custom;

		public static MemberInfo Key => typeof(T).GetKey(false);

		public static MemberInfo MasterKey => typeof(T).GetMemberWithAttribute<MasterKeyAttribute>();

		public static List<MemberInfo> SubModels => typeof(T).GetSubModels(false);

		public static List<MemberInfo> CascadeSubModels => typeof(T).GetCascadeSubModels(false);

		public static List<MemberInfo> EagerSubModels => typeof(T).GetEagerSubModels(false);
	}
}