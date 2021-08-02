using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Shared.Serialization {
	public class AdditionalPropertyConverter<T> : JsonConverter<T> {
		public Dictionary<string, Func<T, object>> AdditionalProperties { get; } = new();

		public List<JsonConverter> Converters = new();

		public sealed override void WriteJson(JsonWriter writer, T value, JsonSerializer serializer) {
			serializer.Converters.AddRange(Converters);
			var obj = JObject.FromObject(value, serializer);
			foreach ((string name, var func) in AdditionalProperties)
				obj.Add(name, JToken.FromObject(func(value), serializer));
			serializer.Converters.RemoveRange(Converters);
			writer.WriteValue(obj);
		}

		public sealed override T ReadJson(JsonReader reader, Type objectType, T existingValue, bool hasExistingValue, JsonSerializer serializer) {
			var obj = JObject.Load(reader);
			foreach (string name in AdditionalProperties.Keys)
				obj.Remove(name);
			serializer.Converters.AddRange(Converters);
			var result = obj.ToObject<T>(serializer);
			serializer.Converters.RemoveRange(Converters);
			return result;
		}
	}
}