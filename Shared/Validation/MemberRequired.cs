using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Shared.Exceptions;

namespace Shared.Validation {
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public class MemberRequiredAttribute : ValidationAttribute {
		private enum ErrorType : byte {
			NullObject,

			WrongObjectType,

			MissingMember,

			WrongMemberType,

			EmptyMemberValue
		}

		private ErrorType _errorType;

		private string _errorMember;

		private static readonly RequiredAttribute Required = new();

		public MemberRequiredAttribute(params string[] names) => MemberNames = names;

		public string[] MemberNames { get; }

		public override bool IsValid(object value) {
			if (value is null) {
				_errorType = ErrorType.NullObject;
				return false;
			}
			var type = value.GetType();
			if (Type.GetTypeCode(type) != TypeCode.Object) {
				_errorType = ErrorType.WrongObjectType;
				return false;
			}
			foreach (string name in MemberNames) {
				var member = type.GetMostDerivedMember(name);
				if (member is null) {
					_errorMember = name;
					_errorType = ErrorType.MissingMember;
					return false;
				}
				if (member.MemberType is not (MemberTypes.Field or MemberTypes.Property)) {
					_errorMember = name;
					_errorType = ErrorType.WrongMemberType;
					return false;
				}
				object v = member.GetValue(value);
				if (!Required.IsValid(v)) {
					_errorMember = name;
					_errorType = ErrorType.EmptyMemberValue;
					return false;
				}
			}
			return true;
		}

		public override string FormatErrorMessage(string name)
			=> _errorType switch {
				ErrorType.NullObject       => $"{name} is null",
				ErrorType.WrongObjectType  => $"{name} is not an object",
				ErrorType.MissingMember    => $"{_errorMember} is missing on {name}",
				ErrorType.WrongMemberType  => $"{name}.{_errorMember} is neither field nor property",
				ErrorType.EmptyMemberValue => $"{name}.{_errorMember} is required",
				_                          => throw new EnumValueOutOfRangeException()
			};
	}
}