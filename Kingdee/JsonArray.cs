using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Kingdee {
	public class JsonArray : IEnumerable<JsonObject> {
		private readonly JArray _jArray;

		private JsonArray(string json) => _jArray = JArray.Parse(json);

		protected JsonArray() => _jArray = new JArray();

		public IEnumerator<JsonObject> GetEnumerator() => (from jToken in _jArray select JsonObject.Parse(jToken.ToString())).GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();

		public static JsonArray Parse(string json) => new(json);

		public IEnumerable<T> ConvertTo<T>() => this.Select(jsonObject => jsonObject.Deserialize<T>());

		public override string ToString() => _jArray.ToString();
	}
}