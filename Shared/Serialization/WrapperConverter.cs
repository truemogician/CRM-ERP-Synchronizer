using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shared.Exceptions;

namespace Shared.Serialization {
	public sealed class ObjectWrapperConverter : ObjectWrapperConverter<string> {
		public ObjectWrapperConverter(string propertyName) : base(propertyName) { }
	}

	public class ObjectWrapperConverter<T> : ObjectWrapperConverterBase<T> {
		public ObjectWrapperConverter(string propertyName) => PropertyName = propertyName;
		public override string PropertyName { get; }
	}

	public abstract class ObjectWrapperConverterBase<T> : JsonConverter<T> {
		public abstract string PropertyName { get; }

		public sealed override void WriteJson(JsonWriter writer, T value, JsonSerializer serializer) {
			writer.WriteStartObject();
			writer.WritePropertyName(PropertyName);
			writer.WriteValue(value);
			writer.WriteEndObject();
		}

		public sealed override T ReadJson(JsonReader reader, Type objectType, T existingValue, bool hasExistingValue, JsonSerializer serializer) {
			var token = JToken.Load(reader);
			if (token.Type != JTokenType.Object)
				throw new JTokenTypeException(token, JTokenType.Object);
			var obj = token as JObject;
			var prop = obj!.Property(PropertyName);
			if (prop is null)
				throw new JTokenException(token, $"Property \"{PropertyName}\" not found");
			return prop.Value.Value<T>();
		}
	}

	public sealed class ArrayWrapperConverter<T> : JsonConverter<T> {
		public override void WriteJson(JsonWriter writer, T value, JsonSerializer serializer) {
			writer.WriteStartArray();
			writer.WriteValue(value);
			writer.WriteEndArray();
		}

		public override T ReadJson(JsonReader reader, Type objectType, T existingValue, bool hasExistingValue, JsonSerializer serializer) {
			var token = JToken.Load(reader);
			if (token.Type is JTokenType.Null or JTokenType.Undefined)
				return default;
			if (token.Type != JTokenType.Array)
				throw new JTokenTypeException(token, JTokenType.Array);
			var array = token as JArray;
			if (array!.Count > 1)
				throw new JTokenException(array, $"{array.Count} items read, but only 1 expected");
			return array.Count == 0 ? default : array[0].Value<T>();
		}
	}
}