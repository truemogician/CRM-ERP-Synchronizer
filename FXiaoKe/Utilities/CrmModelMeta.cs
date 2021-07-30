using System.Collections.Generic;
using System.Reflection;
using FXiaoKe.Models;

namespace FXiaoKe.Utilities {
	public static class CrmModelMeta<T> where T : CrmModelBase {
		public static ModelAttribute ModelAttribute => typeof(T).GetModelAttribute(false);

		public static string ModelName => ModelAttribute.Name;

		public static bool IsCustomModel => ModelAttribute.Custom;

		public static MemberInfo Key => typeof(T).GetKey(false);

		public static MemberInfo MainField => typeof(T).GetMemberWithAttribute<MainFieldAttribute>();

		public static MemberInfo MasterKey => typeof(T).GetMemberWithAttribute<MasterKeyAttribute>();

		public static List<MemberInfo> SubModelMembers => typeof(T).GetSubModelMembers(false);
	}
}