using System;

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
	public class MasterKeyAttribute : FXiaoKeAttribute { }

	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class ForeignKeyAttribute : FXiaoKeAttribute {
		public ForeignKeyAttribute(Type type) => ForeignType = type;
		public Type ForeignType { get; set; }
	}

	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class GeneratedAttribute : FXiaoKeAttribute { }

	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class SubModelAttribute : FXiaoKeAttribute {
		public bool Eager { get; init; } = true;
		public bool Cascade { get; init; } = true;
	}
}