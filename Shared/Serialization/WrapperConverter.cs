using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shared.Exceptions;

namespace Shared.Serialization {
	public class CustomObjectWrapperConverter<TWrapper, TTarget> : JsonConverter<TTarget> where TWrapper : IWrapper {
		public override void WriteJson(JsonWriter writer, TTarget value, JsonSerializer serializer) {
			var depth = 0;
			var wrapperType = typeof(TWrapper);
			while (true) {
				++depth;
				var prop = wrapperType.GetProperty(nameof(IWrapper.Value));
				writer.WriteStartObject();
				writer.WritePropertyName(prop.GetJsonPropertyName());
				var valueType = wrapperType.GetGenericInterfaceArguments(typeof(IWrapper<>)).Single();
				if (valueType.IsInstanceOfType(value)) {
					var converterAttr = prop!.GetCustomAttribute<JsonConverterAttribute>();
					if (converterAttr is not null) {
						var converter = converterAttr.ConverterType.Construct(converterAttr.ConverterParameters) as JsonConverter;
						serializer.Converters.Add(converter);
						writer.WriteValue(value, serializer);
						serializer.Converters.Remove(converter);
					}
					else
						writer.WriteValue(value, serializer);
					break;
				}
				if (valueType.Implements(typeof(IWrapper<>)))
					wrapperType = valueType;
				else
					throw new InterfaceNotImplementedException(typeof(IWrapper<>));
			}
			while (depth-- > 0)
				writer.WriteEndObject();
		}

		public override TTarget ReadJson(JsonReader reader, Type objectType, TTarget existingValue, bool hasExistingValue, JsonSerializer serializer) {
			var wrapperType = typeof(TWrapper);
			var jObj = JObject.Load(reader);
			while (true) {
				var prop = wrapperType.GetProperty(nameof(IWrapper.Value));
				if (!jObj!.TryGetValue(prop.GetJsonPropertyName(), out var jValue))
					throw new KeyNotFoundException();
				var valueType = wrapperType.GetGenericInterfaceArguments(typeof(IWrapper<>)).Single();
				if (valueType.IsAssignableFrom(typeof(TTarget))) {
					var converterAttr = prop!.GetCustomAttribute<JsonConverterAttribute>();
					if (converterAttr is not null) {
						var converter = converterAttr.ConverterType.Construct(converterAttr.ConverterParameters) as JsonConverter;
						serializer.Converters.Add(converter);
						var result = jValue!.ToObject<TTarget>(serializer);
						serializer.Converters.Remove(converter);
						return result;
					}
					return jValue!.ToObject<TTarget>(serializer);
				}
				if (valueType.Implements(typeof(IWrapper<>))) {
					wrapperType = valueType;
					jObj = jValue as JObject;
				}
				else
					throw new InterfaceNotImplementedException(typeof(IWrapper<>));
			}
		}
	}

	public interface IWrapper {
		public object Value { get; set; }
	}

	public interface IWrapper<T> : IWrapper {
		public new T Value { get; set; }

		object IWrapper.Value {
			get => Value;
			set {
				if (value is T v)
					Value = v;
				else
					throw new InvariantTypeException(typeof(T), value.GetType());
			}
		}
	}

	public sealed class ObjectWrapperConverter : ObjectWrapperConverter<string> {
		public ObjectWrapperConverter(string propertyName) : this(propertyName, Array.Empty<JsonConverter>()) { }

		public ObjectWrapperConverter(string propertyName, params JsonConverter[] converters) : base(propertyName, converters) { }
	}

	public class ObjectWrapperConverter<T> : ObjectWrapperConverterBase<T> {
		public ObjectWrapperConverter(string propertyName) : this(propertyName, Array.Empty<JsonConverter>()) { }

		public ObjectWrapperConverter(string propertyName, params JsonConverter[] converters) {
			PropertyName = propertyName;
			Converters = converters;
		}

		public override string PropertyName { get; }

		public override JsonConverter[] Converters { get; }
	}

	public abstract class ObjectWrapperConverterBase<T> : JsonConverter<T> {
		public abstract string PropertyName { get; }

		public abstract JsonConverter[] Converters { get; }

		public sealed override void WriteJson(JsonWriter writer, T value, JsonSerializer serializer) {
			writer.WriteStartObject();
			writer.WritePropertyName(PropertyName);
			serializer.Converters.AddRange(Converters);
			writer.WriteValue(value, serializer);
			serializer.Converters.RemoveRange(Converters);
			writer.WriteEndObject();
		}

		public sealed override T ReadJson(JsonReader reader, Type objectType, T existingValue, bool hasExistingValue, JsonSerializer serializer) {
			var token = JToken.Load(reader);
			if (token.Type != JTokenType.Object)
				throw new JTokenTypeException(token, JTokenType.Object);
			var obj = token as JObject;
			var prop = obj!.Property(PropertyName);
			if (prop is null)
				throw new JTokenException(token, $"Property \"{PropertyName}\" not found");
			serializer.Converters.AddRange(Converters);
			var result = prop.Value.ToObject<T>(serializer);
			serializer.Converters.RemoveRange(Converters);
			return result;
		}
	}

	public sealed class ArrayWrapperConverter : ArrayWrapperConverter<string> {
		public ArrayWrapperConverter() { }

		public ArrayWrapperConverter(params JsonConverter[] converters) : base(converters) { }
	}

	public class ArrayWrapperConverter<T> : JsonConverter<T> {
		public ArrayWrapperConverter() : this(Array.Empty<JsonConverter>()) { }

		public ArrayWrapperConverter(params JsonConverter[] converters) => Converters = converters;

		public JsonConverter[] Converters { get; }

		public override void WriteJson(JsonWriter writer, T value, JsonSerializer serializer) {
			writer.WriteStartArray();
			serializer.Converters.AddRange(Converters);
			writer.WriteValue(value, serializer);
			serializer.Converters.RemoveRange(Converters);
			writer.WriteEndArray();
		}

		public override T ReadJson(JsonReader reader, Type objectType, T existingValue, bool hasExistingValue, JsonSerializer serializer) {
			var token = JToken.Load(reader);
			if (token.Type is JTokenType.Null or JTokenType.Undefined)
				return default;
			if (token.Type != JTokenType.Array)
				throw new JTokenTypeException(token, JTokenType.Array);
			var array = token as JArray;
			if (array!.Count > 1)
				throw new JTokenException(array, $"{array.Count} items read, but only 1 expected");
			if (array.Count == 0)
				return default;
			serializer.Converters.AddRange(Converters);
			var result = array[0].ToObject<T>(serializer);
			serializer.Converters.RemoveRange(Converters);
			return result;
		}
	}
}