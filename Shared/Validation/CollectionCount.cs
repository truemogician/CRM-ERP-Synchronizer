// ReSharper disable InconsistentNaming
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Shared.Validation {
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
	public class CollectionCountAttribute : ValidationAttribute {
		public CollectionCountAttribute(int minCount, int maxCount) {
			MinCount = minCount;
			MaxCount = maxCount;
		}

		public int MinCount { get; }

		public int MaxCount { get; }

		public override bool IsValid(object value) {
			if (!value.GetType().Implements(typeof(ICollection<>)))
				return false;
			var collectionType = value.GetType().GetGenericInterface(typeof(ICollection<>));
			var count = (int)collectionType.GetProperty(nameof(ICollection<object>.Count))!.GetValue(value)!;
			return count >= MinCount && count <= MaxCount;
		}

		public override string FormatErrorMessage(string name) => $"The length of {name} must be between {MinCount} and {MaxCount}.";
	}

	public class CollectionMinCountAttribute : CollectionCountAttribute {
		public CollectionMinCountAttribute(int minCount) : base(minCount, int.MaxValue) { }

		public override string FormatErrorMessage(string name) => $"The length of {name} must be larger than {MinCount}.";
	}

	public class CollectionMaxCountAttribute : CollectionCountAttribute {
		public CollectionMaxCountAttribute(int maxCount) : base(0, maxCount) { }

		public override string FormatErrorMessage(string name) => $"The length of {name} must be smaller than {MaxCount}.";
	}

	public class NotEmptyAttribute : CollectionMinCountAttribute {
		public NotEmptyAttribute() : base(1) { }

		public override string FormatErrorMessage(string name) => $"{name} cannot be empty";
	}
}