using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Shared.Serialization {
	public class TimestampConverter : JsonConverter<DateTime> {
		public static readonly DateTime UnixTimeBeginning = new(1970, 1, 1);

		public TimestampConverter() : this(TimestampPrecision.Millisecond) { }

		public TimestampConverter(TimestampPrecision precision) => Precision = precision;

		public TimestampPrecision Precision { get; }

		public override void WriteJson(JsonWriter writer, DateTime value, JsonSerializer serializer) {
			var duration = value - UnixTimeBeginning;
			writer.WriteValue(Convert.ToInt64(Math.Max(0, Precision == TimestampPrecision.Second ? duration.TotalSeconds : duration.TotalMilliseconds)));
		}

		public override DateTime ReadJson(JsonReader reader, Type objectType, DateTime existingValue, bool hasExistingValue, JsonSerializer serializer) {
			var token = JToken.Load(reader);
			return UnixTimeBeginning +
				(Precision == TimestampPrecision.Second
					? TimeSpan.FromSeconds(token.Value<long>())
					: TimeSpan.FromMilliseconds(token.Value<long>()));
		}
	}

	public enum TimestampPrecision : byte {
		Second,

		Millisecond
	}
}