// ReSharper disable InconsistentNaming
using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Shared.Validators {
	[AttributeUsage(
		AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter
	)]
	public class ArrayLengthAttribute : ValidationAttribute {
		public ArrayLengthAttribute(int minLength, int maxLength) {
			MinLength = minLength;
			MaxLength = maxLength;
		}

		public int MinLength { get; }

		public int MaxLength { get; }

		public override bool IsValid(object value) {
			if (value.GetType().GetInterface(nameof(IEnumerable)) is null)
				return false;
			var list = (value as IEnumerable)!.Cast<object>();
			int count = list.Count();
			return count >= MinLength && count <= MaxLength;
		}

		public override string FormatErrorMessage(string name) => $"The length of {name} must be between {MinLength} and {MaxLength}.";
	}

	public class ArrayMinLengthAttribute : ArrayLengthAttribute {
		public ArrayMinLengthAttribute(int minLength) : base(minLength, int.MaxValue) { }
		public override string FormatErrorMessage(string name) => $"The length of {name} must be larger than {MinLength}.";
	}

	public class ArrayMaxLengthAttribute : ArrayLengthAttribute {
		public ArrayMaxLengthAttribute(int maxLength) : base(0, maxLength) { }

		public override string FormatErrorMessage(string name) => $"The length of {name} must be smaller than {MaxLength}.";
	}

	public class NotEmptyAttribute : ArrayMinLengthAttribute {
		public NotEmptyAttribute() : base(1) { }

		public override string FormatErrorMessage(string name) => $"{name} cannot be empty";
	}
}