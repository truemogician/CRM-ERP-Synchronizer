using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shared.Exceptions;

namespace Shared.Serialization {
	public class StringCollectionConverter : JsonConverter<IEnumerable<string>> {
		public StringCollectionConverter(string separator) => Separator = separator;
		public StringCollectionConverter(char separator) => Separator = new string(separator, 1);
		public string Separator { get; init; }

		public override void WriteJson(JsonWriter writer, IEnumerable<string> value, JsonSerializer serializer) => writer.WriteValue(string.Join(Separator, value));

		public override IEnumerable<string> ReadJson(JsonReader reader, Type objectType, IEnumerable<string> existingValue, bool hasExistingValue, JsonSerializer serializer) {
			var token = JToken.Load(reader);
			if (token.Type is JTokenType.Null or JTokenType.Undefined)
				return null;
			if (token.Type != JTokenType.String)
				throw new JTokenTypeException(token, JTokenType.String);
			return token.Value<string>()!.Split(Separator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
		}
	}
}