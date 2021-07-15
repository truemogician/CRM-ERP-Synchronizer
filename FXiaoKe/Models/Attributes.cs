using System;

namespace FXiaoKe.Models {
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
	public class ModelAttribute : Attribute {
		public ModelAttribute(string name = null, bool custom = false) {
			Name = name;
			Custom = custom;
		}

		public string Name { get; init; }
		public bool Custom { get; init; }
	}

	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class PrimaryKeyAttribute : Attribute { }

	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class ForeignKeyAttribute : Attribute {
		public ForeignKeyAttribute(Type type) => ForeignType = type;
		public Type ForeignType { get; set; }
	}
}