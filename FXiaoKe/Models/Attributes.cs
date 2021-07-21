using System;
using System.Linq;
using System.Reflection;
using Shared.Utilities;

namespace FXiaoKe.Models {
	public abstract class FXiaoKeAttribute : Attribute { }

	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
	public class ModelAttribute : FXiaoKeAttribute {
		public ModelAttribute(string name = null) => Name = name;

		public string Name { get; init; }

		public bool Custom { get; init; }

		public Type SubjectTo { get; init; }
	}

	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class MasterKeyAttribute : FXiaoKeAttribute {
		public MasterKeyAttribute() => KeyName = nameof(ModelBase.DataId);
		public MasterKeyAttribute(string keyName) => KeyName = keyName;
		public string KeyName { get; }

		public MemberInfo GetKey(Type declaringType) {
			var masterType = declaringType.GetCustomAttribute<ModelAttribute>()!.SubjectTo;
			if (masterType is null)
				throw new NullReferenceException("Master type not specified");
			return masterType.GetMember(KeyName, MemberTypes.Property | MemberTypes.Field);
		}
	}

	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class ForeignKeyAttribute : FXiaoKeAttribute {
		public ForeignKeyAttribute(Type type) => ForeignType = type;
		public Type ForeignType { get; set; }
	}

	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class GeneratedAttribute : FXiaoKeAttribute { }

	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class SubModelAttribute : FXiaoKeAttribute {
		public bool Eager { get; init; }
		public bool Cascade { get; init; }
	}
}