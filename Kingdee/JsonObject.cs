using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Kingdee {
	public class JsonObject : IEnumerable<KeyValuePair<string, string>> {
		private readonly JObject _jObject;

		private JsonObject(string json) => _jObject = JObject.Parse(json);

		protected JsonObject() => _jObject = new JObject();

		public IEnumerator<KeyValuePair<string, string>> GetEnumerator() {
			foreach (var keyValuePair in _jObject)
				yield return new KeyValuePair<string, string>(keyValuePair.Key, keyValuePair.Value?.ToString());
		}

		IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();

		public static JsonObject Parse(string json) => new(json);

		public void SetValue(string prop, object v) {
			if (v != null && v.GetType().IsSimpleType())
				AddOrUpdate(prop, new JValue(v));
			else
				AddOrUpdate(prop, JObject.FromObject(v!));
		}

		private void AddOrUpdate(string prop, JToken v) {
			if (_jObject.Property(prop) == null)
				_jObject.Add(prop, v);
			else
				_jObject[prop] = v;
		}

		public T GetValue<T>(string prop) {
			if (_jObject.TryGetValue(prop, out var jToken))
				return jToken.Value<T>();
			throw new ServiceException("对象不包括属性" + prop);
		}

		public override string ToString() => _jObject.ToString();

		internal T Deserialize<T>() => (T)JsonConvert.DeserializeObject(ToString(), typeof(T));
	}
}