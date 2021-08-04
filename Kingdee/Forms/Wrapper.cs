using System.Reflection;
using Newtonsoft.Json;

namespace Kingdee.Forms {
	public class NumberWrapper : NumberWrapper<string> {
		public static implicit operator NumberWrapper(string number) => new() {Number = number};
	}

	public class NumberWrapper<T> : WrapperBase<T> {
		[JsonProperty("FNumber")]
		public virtual T Number { get; set; }

		protected override string ValueName => nameof(Number);

		public static implicit operator NumberWrapper<T>(T number) => new() {Number = number};
	}

	public class NameWrapper : NameWrapper<string> {
		public static implicit operator NameWrapper(string number) => new() {Name = number};
	}

	public class NameWrapper<T> : WrapperBase<T> {
		[JsonProperty("FName")]
		public virtual T Name { get; set; }

		protected override string ValueName => nameof(Name);

		public static implicit operator NameWrapper<T>(T number) => new() {Name = number};
	}

	public abstract class WrapperBase<T> {
		private readonly MemberInfo _member;

		protected WrapperBase() => _member = GetType().GetMostDerivedMember(ValueName);

		protected abstract string ValueName { get; }

		public override string ToString() => ((T)_member.GetValue(this))?.ToString();

		public static implicit operator T(WrapperBase<T> self) => (T)self._member.GetValue(self);
	}
}