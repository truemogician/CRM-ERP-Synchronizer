using System;
using System.Reflection;

namespace Shared.Exceptions {
	public class TypeException : ExceptionWithDefaultMessage {
		public TypeException(string message = null, Exception innerException = null) : base(message, innerException) { }
		public TypeException(Type type, string message = null, Exception innerException = null) : this(message, innerException) => this[nameof(Type)] = type;
		public Type Type => Get<Type>(nameof(Type));
	}

	public class TypeNotMatchException : ExceptionWithDefaultMessage {
		public TypeNotMatchException(string message = null, Exception innerException = null) : base(message, innerException) { }

		public TypeNotMatchException(Type dstType, Type srcType, string message = null, Exception innerException = null) : this(message, innerException) {
			this[nameof(TargetType)] = dstType;
			this[nameof(SourceType)] = srcType;
		}

		public Type TargetType => Get<Type>(nameof(TargetType));
		public Type SourceType => Get<Type>(nameof(SourceType));

		protected override string DefaultMessage => $"Source type {SourceType.FullName} doesn't match target type {TargetType.FullName}";
	}

	public class InvariantTypeException : TypeNotMatchException {
		public InvariantTypeException(string message = null, Exception innerException = null) : base(message, innerException) { }
		public InvariantTypeException(Type dstType, Type srcType, string message = null, Exception innerException = null) : base(dstType, srcType, message, innerException) { }
		protected override string DefaultMessage => $"Source type {SourceType.FullName} is not covariant with target type {TargetType.FullName}";
	}

	public class InterfaceNotImplementedException : ExceptionWithDefaultMessage {
		public InterfaceNotImplementedException(string message = null, Exception innerException = null) : base(message, innerException) { }
		public InterfaceNotImplementedException(Type interfaceType, string message = null, Exception innerException = null) : this(message, innerException) => this[nameof(InterfaceType)] = interfaceType;
		public Type InterfaceType => Get<Type>(nameof(InterfaceType));
		protected override string DefaultMessage => $"Interface {InterfaceType.FullName} not implemented";
	}

	public class MemberException : ExceptionWithDefaultMessage {
		public MemberException(string message = null, Exception innerException = null) : base(message, innerException) { }
		public MemberException(MemberInfo member, string message = null, Exception innerException = null) : this(message, innerException) => this[nameof(Member)] = member;
		public MemberInfo Member => Get<MemberInfo>(nameof(Member));
	}

	public class MemberTypeException : MemberException {
		public MemberTypeException(string message = null, Exception innerException = null) : base(message, innerException) { }
		public MemberTypeException(MemberInfo member, MemberTypes requiredTypes, string message = null, Exception innerException = null) : base(member, message, innerException) => this[nameof(RequiredTypes)] = requiredTypes;
		public MemberTypes? RequiredTypes => Get<MemberTypes?>(nameof(RequiredTypes));
		protected override string DefaultMessage => $"Member {Member.Name} is {Member.MemberType}, but {RequiredTypes} required";
	}
}