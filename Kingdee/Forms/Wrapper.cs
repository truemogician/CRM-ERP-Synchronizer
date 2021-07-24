using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace Kingdee.Forms {
	public class NumberWrapper : NumberWrapper<string> { }

	public class NumberWrapper<T> : WrapperBase<T> {
		[JsonProperty("FNumber")]
		public T Number { get; set; }

		//public static implicit operator T(NumberWrapper<T> self) => self.Number;
		public static implicit operator NumberWrapper<T>(T number) => new() {Number = number};
		protected override string ValueName => nameof(Number);
	}

	public class NameWrapper : NameWrapper<string> { }

	public class NameWrapper<T> : WrapperBase<T> {
		[JsonProperty("FName")]
		public T Name { get; set; }

		//public static implicit operator T(NameWrapper<T> self) => self.Name;
		public static implicit operator NameWrapper<T>(T number) => new() {Name = number};
		protected override string ValueName => nameof(Name);
	}

	public abstract class WrapperBase<T> {
		protected abstract string ValueName { get; }

		public static implicit operator T(WrapperBase<T> self) => (T)self.GetType().GetMember(self.ValueName).Single().GetValue(self);
	}
}