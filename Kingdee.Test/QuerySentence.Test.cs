using System;
using System.Collections.Generic;
using NUnit.Framework;
using Kingdee.Requests.Query;
using Newtonsoft.Json;
using Shared.JsonConverters;

namespace Kingdee.Test {
	public class QuerySentenceTests {
		public Sentence<Foo> Sentence { get; set; }

		[SetUp]
		public void SetUp() {
			var column = new Column(nameof(Foo.Prop3), nameof(Foo.Prop3.Prop2));
			Sentence = column * 2 > 4;
		}

		[Test]
		public void ToStringTest() {
			Assert.AreEqual("((C.E * 2) > 4)", Sentence.ToString());
		}

		[Test]
		public void ConverterTest() {
			var foo = new FooFoo<Foo>() {
				Sentence = Sentence
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