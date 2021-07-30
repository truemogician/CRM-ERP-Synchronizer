using System;
using System.Reflection;
using Newtonsoft.Json;

namespace Shared.Serialization {
	public class NullableConverter<TConverter, TValue> : JsonConverter<TValue?> where TConverter : JsonConverter<TValue> where TValue : struct {
		public NullableConverter() => Converter = typeof(TConverter).Construct() as TConverter;

		public NullableConverter(params object[] parameters) => Converter = typeof(TConverter).Construct(parameters) as TConverter;

		public TConverter Converter { get; }

		public override void WriteJson(JsonWriter writer, TValue? value, JsonSerializer serializer) {
			if (!value.HasValue)
				writer.WriteNull();
			else
				Converter.WriteJson(writer, value.Value, serializer);
		}

		public override TValue? ReadJson(JsonReader reader, Type objectType, TValue? existingValue, bool hasExistingValue, JsonSerializer serializer) {
			if (reader.TokenType is JsonToken.Null or JsonToken.Undefined)
				return null;
			return Converter.ReadJson(reader, objectType, existingValue ?? default, hasExistingValue, serializer);
		}
	}
}