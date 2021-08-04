using System;
using System.Reflection;
using Newtonsoft.Json;

namespace Shared.Serialization {
	public class NullableConverter<TConverter> : JsonConverter where TConverter : JsonConverter {
		private static Type _nullableValueType;

		private static PropertyInfo _nullableValueProperty;

		public NullableConverter() => Converter = typeof(TConverter).Construct() as TConverter;

		public NullableConverter(params object[] parameters) => Converter = typeof(TConverter).Construct(parameters) as TConverter;

		public static Type ValueType { get; } = typeof(TConverter).GetGenericType(typeof(JsonConverter<>)) is { } converterType ? converterType.GetGenericArguments()[0] : typeof(object);

		public static Type NullableValueType => _nullableValueType ??= typeof(Nullable<>).MakeGenericType(ValueType);

		private static PropertyInfo NullableValueProperty => _nullableValueProperty ??= NullableValueType.GetProperty(nameof(Nullable<int>.Value));

		public TConverter Converter { get; }

		#nullable enable
		public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer) {
			if (value is null)
				writer.WriteNull();
			else
				Converter.WriteJson(writer, NullableValueProperty.GetValue(value!), serializer);
		}

		public override object? ReadJson(
			JsonReader reader,
			Type objectType,
			object? existingValue,
			JsonSerializer serializer
		)
			=> reader.TokenType is JsonToken.Null or JsonToken.Undefined
				? null
				: Converter.ReadJson(reader, objectType.GetGenericArguments()[0], existingValue ?? default, serializer);

		public override bool CanConvert(Type objectType) => objectType == NullableValueType;
	}
}