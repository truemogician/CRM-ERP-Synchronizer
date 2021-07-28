using System;

namespace Shared.Validation {
	[AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
	public class ValidationIgnoreAttribute : Attribute { }
}