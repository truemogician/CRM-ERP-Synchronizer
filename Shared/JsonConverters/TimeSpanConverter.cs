using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shared.Exceptions;

namespace Shared.JsonConverters {
	public abstract class TimeSpanConverter<T> : JsonConverter<TimeSpan> where T : unmanaged {
		protected abstract Func<TimeSpan, T> ToNumber { get; }
		protected abstract Func<T, TimeSpan> FromNumber { get; }

		public override void WriteJson(JsonWriter writer, TimeSpan value, JsonSerializer serializer) {
			writer.WriteValue(ToNumber(value));
		}

		public override TimeSpan ReadJson(JsonReader reader, Type objectType, TimeSpan existingValue, bool hasExistingValue, JsonSerializer serializer) {
			var token = JToken.Load(reader);
			if (token.Type is JTokenType.Integer or JTokenType.Float)
				return FromNumber(token.Value<T>());
			throw new JTokenTypeException(token, JTokenType.Integer, JTokenType.Float);
		}
	}

	public sealed class TimeSpanSecondIntegerConverter : TimeSpanConverter<int> {
		protected override Func<TimeSpan, int> ToNumber => timeSpan => (int)Math.Round(timeSpan.TotalSeconds);
		protected override Func<int, TimeSpan> FromNumber => seconds => TimeSpan.FromSeconds(seconds);
	}

	public sealed class TimeSpanSecondDoubleConverter : TimeSpanConverter<double> {
		protected override Func<TimeSpan, double> ToNumber => timeSpan => timeSpan.TotalSeconds;
		protected override Func<double, TimeSpan> FromNumber => TimeSpan.FromSeconds;
	}
}