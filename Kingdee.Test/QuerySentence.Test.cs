using System.Collections.Generic;
using Kingdee.Requests.Query;
using Newtonsoft.Json;
using NUnit.Framework;
using Shared.Serialization;

namespace Kingdee.Test {
	public class QuerySentenceTests {
		public List<Field> Columns { get; } = new();

		[SetUp]
		public void SetUp() {
			Columns.Add(new Field(nameof(Foo.Prop1)));
			Columns.Add(new Field(nameof(Foo.Prop2)));
			Columns.Add(new Field(nameof(Foo.Prop3), nameof(Foo.Prop3.Prop1)));
			Columns.Add(new Field(nameof(Foo.Prop3), nameof(Foo.Prop3.Prop2)));
		}

		[Test]
		public void OperatorTest() {
			Sentence<Foo> sentence = Columns[3] * 2 > 4;
			Assert.AreEqual("((C.E * 2) > 4)", sentence.ToString());
		}

		[Test]
		public void BetweenTest() {
			Sentence<Foo> sentence = (Columns[0] * 2).Between(3, 4);
			Assert.AreEqual("((A * 2) BETWEEN 3 AND 4)", sentence.ToString());
		}

		[Test]
		public void InTest() {
			Sentence<Foo> sentence = (Columns[0] ^ 8).In("a", "b", Columns[2]);
			Assert.AreEqual("((A ^ 8) IN ('a', 'b', C.D))", sentence.ToString());
		}

		[Test]
		public void SerializeTest() {
			var foo = new FooFoo<Foo> {
				Sentence = Columns[3] * 2 > 4
			};
			Assert.AreEqual("{\"sentence\":\"((C.E * 2) > 4)\"}", JsonConvert.SerializeObject(foo));
		}
	}

	public class FooFoo<T> {
		[JsonProperty("sentence")]
		[JsonConverter(typeof(ToStringConverter<Sentence>))]
		public Sentence<T> Sentence { get; set; }
	}

	public class Foo {
		[JsonProperty("A")]
		public string Prop1 { get; set; }

		[JsonProperty("B")]
		public int Prop2 { get; set; }

		[JsonProperty("C")]
		public Bar Prop3 { get; set; }
	}

	public class Bar {
		[JsonProperty("D")]
		public bool Prop1 { get; set; }

		[JsonProperty("E")]
		public List<object> Prop2 { get; set; }
	}
}