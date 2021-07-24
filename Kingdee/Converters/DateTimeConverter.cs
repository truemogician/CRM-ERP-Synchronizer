using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shared.Exceptions;

namespace Kingdee.Converters {
	public class DateTimeConverter : JsonConverter<DateTime?> {
		public override void WriteJson(JsonWriter writer, DateTime? value, JsonSerializer serializer) {
			if (value.HasValue)
				writer.WriteValue(value.Value.ToString("yyyy-MM-dd HH:mm:ss"));
			else
				writer.WriteNull();
		}

		public override DateTime? ReadJson(JsonReader reader, Type objectType, DateTime? existingValue, bool hasExistingValue, JsonSerializer serializer) {
			var token = JToken.Load(reader);
			if (token.Type is JTokenType.Null or JTokenType.Undefined)
				return null;
			if (token.Type != JTokenType.String)
				throw new JTokenTypeException(token, JTokenType.String);
			var dateString = token.Value<string>();
			if (string.IsNullOrEmpty(dateString))
				return null;
			return Convert.ToDateTime(dateString);
		}
	}
}