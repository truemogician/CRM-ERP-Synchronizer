using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shared.Exceptions;

namespace Shared.Serialization {
	public abstract class TimeSpanConverter<T> : JsonConverter<TimeSpan> where T : IConvertible {
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

	public class TimeSpanUnitConverter<T> : TimeSpanConverter<T> where T : IConvertible {
		public TimeSpanUnitConverter(TimeSpanUnit unit) => Unit = unit;
		public TimeSpanUnit Unit { get; }

		protected override Func<TimeSpan, T> ToNumber
			=> timeSpan => FromDouble(
				Unit switch {
					TimeSpanUnit.Tick        => timeSpan.Ticks,
					TimeSpanUnit.Millisecond => timeSpan.TotalMilliseconds,
					TimeSpanUnit.Second      => timeSpan.TotalSeconds,
					TimeSpanUnit.Minute      => timeSpan.TotalMinutes,
					TimeSpanUnit.Hour        => timeSpan.TotalHours,
					TimeSpanUnit.Day         => timeSpan.TotalDays,
					_                        => throw new EnumValueOutOfRangeException(typeof(TimeSpanUnit), Unit)
				}
			);

		protected override Func<T, TimeSpan> FromNumber
			=> value => {
				double db = value.ToDouble(null);
				return Unit switch {
					TimeSpanUnit.Tick        => TimeSpan.FromTicks(Convert.ToInt64(db)),
					TimeSpanUnit.Millisecond => TimeSpan.FromMilliseconds(db),
					TimeSpanUnit.Second      => TimeSpan.FromSeconds(db),
					TimeSpanUnit.Minute      => TimeSpan.FromMinutes(db),
					TimeSpanUnit.Hour        => TimeSpan.FromHours(db),
					TimeSpanUnit.Day         => TimeSpan.FromDays(db),
					_                        => throw new EnumValueOutOfRangeException(typeof(TimeSpanUnit), Unit)
				};
			};

		protected virtual T FromDouble(double time) {
			return Type.GetTypeCode(typeof(T)) switch {
				TypeCode.Double => (dynamic)time,
				TypeCode.Single => Convert.ToSingle(time),
				TypeCode.Int64  => Convert.ToInt64(time),
				TypeCode.Int32  => Convert.ToInt32(time),
				TypeCode.Int16  => Convert.ToInt16(time),
				TypeCode.UInt64 => Convert.ToUInt64(time),
				TypeCode.UInt32 => Convert.ToUInt32(time),
				TypeCode.UInt16 => Convert.ToUInt16(time),
				TypeCode.Byte   => Convert.ToByte(time),
				TypeCode.SByte  => Convert.ToSByte(time),
				TypeCode.Char   => Convert.ToChar(time),
				TypeCode.String => Convert.ToString(time),
				_               => throw new NotImplementedException()
			};
		}
	}

	public enum TimeSpanUnit {
		Tick,
		Millisecond,
		Second,
		Minute,
		Hour,
		Day
	}
}