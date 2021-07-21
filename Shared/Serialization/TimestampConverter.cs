using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Shared.Serialization {
	public class TimestampConverter : JsonConverter<DateTime> {
		public TimestampConverter() : this(TimestampPrecision.Millisecond) { }
		public TimestampConverter(TimestampPrecision precision) => Precision = precision;

		public TimestampPrecision Precision { get; }

		public override void WriteJson(JsonWriter writer, DateTime value, JsonSerializer serializer) {
			var offset = new DateTimeOffset(value);
			writer.WriteValue(Precision == TimestampPrecision.Second ? offset.ToUnixTimeSeconds() : offset.ToUnixTimeMilliseconds());
		}

		public override DateTime ReadJson(JsonReader reader, Type objectType, DateTime existingValue, bool hasExistingValue, JsonSerializer serializer) {
			var token = JToken.Load(reader);
			return Precision == TimestampPrecision.Second
				? DateTimeOffset.FromUnixTimeSeconds(token.Value<long>()).DateTime
				: DateTimeOffset.FromUnixTimeMilliseconds(token.Value<long>()).DateTime;
		}
	}

	public enum TimestampPrecision : byte {
		Second,
		Millisecond
	}
}