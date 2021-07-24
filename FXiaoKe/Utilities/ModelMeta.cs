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

		public static List<MemberInfo> SubModelInfos => typeof(T).GetSubModelInfos(false);
	}
}