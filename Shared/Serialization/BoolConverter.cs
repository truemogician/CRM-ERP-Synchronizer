using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shared.Exceptions;

namespace Shared.Serialization {
	public class BoolConverter : BoolConverterBase {
		public BoolConverter() : this("true", "false") { }

		public BoolConverter(string trueString, string falseString) : this(trueString, falseString, StringComparison.OrdinalIgnoreCase) { }

		public BoolConverter(string trueString, string falseString, StringComparison comparisonOption) {
			TrueString = trueString;
			FalseString = falseString;
			ComparisonOption = comparisonOption;
		}

		protected override string TrueString { get; }

		protected override string FalseString { get; }

		protected override StringComparison ComparisonOption { get; }
	}

	public abstract class BoolConverterBase : JsonConverter<bool?> {
		protected abstract string TrueString { get; }
		protected abstract string FalseString { get; }
		protected virtual StringComparison ComparisonOption => StringComparison.OrdinalIgnoreCase;

		public sealed override void WriteJson(JsonWriter writer, bool? value, JsonSerializer serializer) {
			if (value.HasValue)
				writer.WriteValue(value.Value ? TrueString : FalseString);
			else
				writer.WriteNull();
		}

		public sealed override bool? ReadJson(JsonReader reader, Type objectType, bool? existingValue, bool hasExistingValue, JsonSerializer serializer) {
			var token = JToken.Load(reader);
			if (token.Type is JTokenType.Null or JTokenType.Undefined)
				return null;
			if (token.Type != JTokenType.String)
				throw new JTokenTypeException(token, JTokenType.String);
			string value = token.Value<string>();
			if (string.IsNullOrEmpty(value))
				return null;
			if (value.Equals(TrueString, ComparisonOption))
				return true;
			if (value.Equals(TrueString, ComparisonOption))
				return false;
			throw new JTokenException(token, $"Value is neither true nor false string: {value}");
		}
	}
}