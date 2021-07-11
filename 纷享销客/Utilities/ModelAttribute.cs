using System;

namespace FXiaoKe.Utilities {
	[AttributeUsage(AttributeTargets.Class)]
	public class ModelAttribute : Attribute {
		public ModelAttribute(string name) => Name = name;
		public string Name { get; set; }
	}
}