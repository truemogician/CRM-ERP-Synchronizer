using System;
using Shared.Exceptions;

namespace FXiaoKe.Models {
	public abstract class FXiaoKeAttribute : Attribute { }

	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
	public class ModelAttribute : FXiaoKeAttribute {
		public ModelAttribute(string name = null) => Name = name;

		public string Name { get; }

		public bool Custom { get; init; }
	}

	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class MainFieldAttribute : FXiaoKeAttribute {
		public bool Unique { get; init; } = true;
	}

	public class MasterKeyAttribute : ReferenceAttributeBase {
		public MasterKeyAttribute(Type type) : base(type) { }

		public Type MasterType => ReferenceType;
	}

	public class ForeignKeyAttribute : ReferenceAttributeBase {
		public ForeignKeyAttribute(Type type) : base(type) { }

		public Type ForeignType => ReferenceType;
	}

	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public abstract class ReferenceAttributeBase : FXiaoKeAttribute {
		internal readonly Type ReferenceType;

		protected ReferenceAttributeBase(Type type) => ReferenceType = type;
	}

	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class GeneratedAttribute : FXiaoKeAttribute { }

	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class DefaultConstructorAttribute : FXiaoKeAttribute {
		public DefaultConstructorAttribute(Type constructingType) {
			if (constructingType.IsAbstract || constructingType.IsInterface || constructingType.IsGenericTypeDefinition)
				throw new TypeException(constructingType, $"{constructingType.Name} can't be instantiated");
			ConstructingType = constructingType;
		}

		public Type ConstructingType { get; }
	}

	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class SubModelAttribute : FXiaoKeAttribute {
		public bool Eager { get; init; } = true;

		public bool Cascade { get; init; } = true;

		public string ReverseKeyName { get; init; }
	}
}