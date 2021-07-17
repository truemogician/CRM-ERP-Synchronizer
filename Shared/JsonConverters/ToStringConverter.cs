using System;
using Newtonsoft.Json;

namespace Shared.JsonConverters {
	public class ToStringConverter<T> : JsonConverter<T> {
		public override void WriteJson(JsonWriter writer, T value, JsonSerializer serializer) => writer.WriteValue(value.ToString());

		public override T ReadJson(JsonReader reader, Type objectType, T existingValue, bool hasExistingValue, JsonSerializer serializer) => throw new NotImplementedException();
	}

	public class ToStringConverter : JsonConverter {
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) => writer.WriteValue(value.ToString());

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) => throw new NotImplementedException();

		public override bool CanConvert(Type objectType) => true;
	}
}