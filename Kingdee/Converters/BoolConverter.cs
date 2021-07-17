using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Kingdee.Converters {
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