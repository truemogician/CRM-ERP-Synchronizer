using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Kingdee.Utilities {
	public abstract class StringIdConverter : JsonConverter<string> {
		protected abstract string IdName { get; }

		public sealed override void WriteJson(JsonWriter writer, string value, JsonSerializer serializer) {
			writer.WriteStartObject();
			writer.WritePropertyName(IdName);
			writer.WriteValue(value);
			writer.WriteEndObject();
		}

		public sealed override string ReadJson(JsonReader reader, Type objectType, string existingValue, bool hasExistingValue, JsonSerializer serializer) {
			var obj = JObject.Load(reader);
			return obj.Property(IdName)?.Value.Value<string>();
		}
	}

	public class BoolConverter : JsonConverter<bool?> {
		protected virtual string TrueString => "true";
		protected virtual string FalseString => "false";

		public sealed override void WriteJson(JsonWriter writer, bool? value, JsonSerializer serializer) {
			if (value.HasValue)
				writer.WriteValue(value.Value ? TrueString : FalseString);
			else
				writer.WriteNull();
		}

		public sealed override bool? ReadJson(JsonReader reader, Type objectType, bool? existingValue, bool hasExistingValue, JsonSerializer serializer) {
			var token = JToken.Load(reader);
			if (token.Type is JTokenType.Null or JTokenType.Undefined)
				return null;
			if (token.Type != JTokenType.String)
				throw new JsonReaderException($"Wrong token type: {nameof(JTokenType.String)} expected, but {token.Type} received");
			string value = token.Value<string>();
			if (string.IsNullOrEmpty(value))
				return null;
			if (value == TrueString)
				return true;
			if (value == FalseString)
				return false;
			throw new JsonReaderException($"Value is neither true nor false string: {value}");
		}
	}
}