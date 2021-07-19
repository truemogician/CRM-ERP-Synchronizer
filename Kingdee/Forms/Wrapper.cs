using Newtonsoft.Json;

namespace Kingdee.Forms {
	public class NumberWrapper : NumberWrapper<string> { }

	public class NumberWrapper<T> {
		[JsonProperty("FNumber")]
		public T Number { get; set; }

		public static implicit operator T(NumberWrapper<T> self) => self.Number;
		public static implicit operator NumberWrapper<T>(T number) => new() {Number = number};
	}

	public class NameWrapper : NameWrapper<string> { }

	public class NameWrapper<T> {
		[JsonProperty("FName")]
		public T Name { get; set; }

		public static implicit operator T(NameWrapper<T> self) => self.Name;
		public static implicit operator NameWrapper<T>(T number) => new() {Name = number};
	}
}