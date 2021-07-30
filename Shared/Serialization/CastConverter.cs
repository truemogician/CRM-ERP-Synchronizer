using System;
using System.Reflection;
using Newtonsoft.Json;

namespace Shared.Serialization {
	public class CastConverter<TSource, TTarget> : JsonConverter<TSource> {
		public CastConverter() { }

		public CastConverter(JsonConverter converter) => TargetConverter = converter;

		public CastConverter(Type converterType, params object[] parameters) => TargetConverter = (JsonConverter)converterType.Construct(parameters);

		public JsonConverter TargetConverter { get; set; }

		public override void WriteJson(JsonWriter writer, TSource value, JsonSerializer serializer) {
			var target = (TTarget)(dynamic)value;
			if (TargetConverter is null)
				writer.WriteValue(target);
			else {
				serializer.Converters.Add(TargetConverter);
				writer.WriteValue(target, serializer);
				serializer.Converters.Remove(TargetConverter);
			}
		}

		public override TSource ReadJson(JsonReader reader, Type objectType, TSource existingValue, bool hasExistingValue, JsonSerializer serializer) {
			if (TargetConverter is not null)
				serializer.Converters.Add(TargetConverter);
			var target = serializer.Deserialize<TTarget>(reader);
			if (TargetConverter is not null)
				serializer.Converters.Remove(TargetConverter);
			return (TSource)(dynamic)target;
		}
	}
}