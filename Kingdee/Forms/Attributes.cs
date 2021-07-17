using System;

namespace Kingdee.Forms {
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
	public class FormAttribute : Attribute {
		public FormAttribute(string name = null) => Name = name;

		public string Name { get; init; }
	}

	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
	public class SubFormAttribute : Attribute { }

	[AttributeUsage(AttributeTargets.Property)]
	public class QueryIgnoreAttribute : Attribute { }
}