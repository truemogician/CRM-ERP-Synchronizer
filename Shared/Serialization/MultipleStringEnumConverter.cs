using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shared.Exceptions;

namespace Shared.Serialization {
	public class MultipleStringEnumConverter : JsonConverter {
		private Type _enumType;

		private readonly Dictionary<string, string> _attrName = new();

		private readonly Dictionary<string, string> _enumName = new();

		public MultipleStringEnumConverter(object nameSelect) => NameSelect = nameSelect;

		public object NameSelect { get; }

		protected Type EnumType {
			get => _enumType;
			set {
				if (value is null)
					throw new ArgumentNullException(nameof(value));
				if (!value.IsAssignableTo(typeof(Enum)))
					throw new InvariantTypeException(typeof(Enum), value);
				if (_enumType != value) {
					_enumType = value;
					_attrName.Clear();
					_enumName.Clear();
					foreach (var field in _enumType.GetFields().Where(f => f.IsStatic)) {
						var attribute = field.GetCustomAttributes<MultipleEnumMemberAttribute>().Single(attr => attr.Index.Equals(NameSelect));
						_attrName.Add(field.Name, attribute.Name);
						_enumName.Add(attribute.Name, field.Name);
					}
				}
			}
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
			if (value == null) {
				writer.WriteNull();
				return;
			}
			EnumType = value.GetType();
			writer.WriteValue(_attrName[(value as Enum)!.ToString()]);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
			EnumType = objectType;
			var token = JToken.Load(reader);
			if (token.Type != JTokenType.String)
				throw new JTokenTypeException(token, JTokenType.String);
			string attrName = token.Value<string>();
			return Enum.Parse(EnumType, _enumName[attrName!]);
		}

		public override bool CanConvert(Type objectType) => objectType.IsEnum;
	}

	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
	public class MultipleEnumMemberAttribute : Attribute {
		public MultipleEnumMemberAttribute(string name, object index) {
			Name = name;
			Index = index;
		}

		public string Name { get; }

		public object Index { get; }
	}
}