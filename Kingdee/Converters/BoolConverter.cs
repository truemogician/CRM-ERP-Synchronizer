using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shared.Exceptions;

namespace Kingdee.Converters {
	public class BoolConverter : JsonConverter<bool?> {
		protected virtual string TrueString => "true";
		protected virtual string FalseString => "false";
		protected virtual StringComparison ComparisonOption => StringComparison.OrdinalIgnoreCase;

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
				throw new JTokenTypeException(token, JTokenType.String);
			string value = token.Value<string>();
			if (string.IsNullOrEmpty(value))
				return null;
			if (value.Equals(TrueString, ComparisonOption))
				return true;
			if (value.Equals(TrueString, ComparisonOption))
				return false;
			throw new JTokenException(token, $"Value is neither true nor false string: {value}");
		}
	}
}