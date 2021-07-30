using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shared.Exceptions;

namespace Shared.Serialization {
	public class EnumValueConverter : JsonConverter<Enum> {
		private readonly Dictionary<Enum, string> _stringValue = new();

		private readonly Dictionary<FieldInfo, List<EnumValueAttribute>> _attributes = new();

		private Enum _defaultEnum;

		private Type _enumType;

		public EnumValueConverter() { }

		public EnumValueConverter(object index) => Index = index;

		public object Index { get; }

		protected Type EnumType {
			get => _enumType;
			set {
				if (value is null)
					throw new ArgumentNullException(nameof(value));
				if (!value.IsAssignableTo(typeof(Enum)))
					throw new InvariantTypeException(typeof(Enum), value);
				if (_enumType != value) {
					_enumType = value;
					_stringValue.Clear();
					_attributes.Clear();
					foreach (var field in _enumType.GetFields(BindingFlags.Static | BindingFlags.Public)) {
						if (field.IsDefined(typeof(EnumDefaultAttribute)))
							_defaultEnum = _defaultEnum is null ? field.GetValue(null) as Enum : throw new DuplicateException($"One enum can only have up to 1 member with {nameof(EnumDefaultAttribute)}");
						var attrs = field.GetCustomAttributes<EnumValueAttribute>().AsList();
						//Compatibility with EnumMemberAttribute
						if (field.GetCustomAttribute<EnumMemberAttribute>() is { } enumMemberAttr)
							attrs.Add(enumMemberAttr);
						//Default attribute
						if (attrs.Count == 0)
							attrs.Add(field);
						//Verification
						attrs.AsArray().MatchIndex(Index, value);

						_attributes.Add(field, attrs);
						string stringValue = Index is null ? attrs[0].Value : attrs.SingleOrDefault(attr => Index.Equals(attr.Index))?.Value;
						if (stringValue is not null && _stringValue.ContainsValue(stringValue))
							throw new DuplicateException(stringValue);
						_stringValue.Add((field.GetValue(null) as Enum)!, stringValue);
					}
				}
			}
		}

		public override void WriteJson(JsonWriter writer, Enum value, JsonSerializer serializer) {
			if (value == null || value.GetEnumMember().IsDefined(typeof(EnumDefaultAttribute))) {
				writer.WriteNull();
				return;
			}
			EnumType = value.GetType();
			string result = _stringValue[value];
			if (result is null)
				throw new EnumException(EnumType, $"No exact value specified for {value}");
			writer.WriteValue(result);
		}

		public override Enum ReadJson(JsonReader reader, Type objectType, Enum existingValue, bool hasExistingValue, JsonSerializer serializer) {
			EnumType = objectType;
			var token = JToken.Load(reader);
			if (token.Type != JTokenType.String)
				throw new JTokenTypeException(token, JTokenType.String);
			var stringValue = token.Value<string>();
			(var key, string value) = _stringValue.FirstOrDefault(pair => pair.Value == stringValue);
			if (value is not null)
				return key;
			FieldInfo result = null;
			foreach (var (field, attrs) in _attributes.Where(pair => pair.Value.Any(attr => attr.Regex is not null))) {
				var attr = attrs.Count == 1 ? attrs[0] : attrs.FirstOrDefault(a => a.Index.Equals(Index));
				if (attr is null || !attr.Regex.IsMatch(stringValue!))
					continue;
				result = result is null ? field : throw new DuplicateException(stringValue, $"Multiple patterns matched");
			}
			return result?.GetValue(null) as Enum ?? _defaultEnum ?? throw new NotFoundException(stringValue, $"No enum member matches {stringValue}");
		}
	}

	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
	public class EnumValueAttribute : Attribute {
		private readonly string _pattern;

		private readonly RegexOptions _regexOptions = RegexOptions.None;

		public EnumValueAttribute() { }

		public EnumValueAttribute(string value) => Value = value;

		public EnumValueAttribute(string value, object index) : this(value) => Index = index;

		public string Value { get; init; }

		public object Index { get; init; }

		public string Pattern {
			get => _pattern;
			init {
				_pattern = value;
				Regex = new Regex(_pattern, _regexOptions);
			}
		}

		public RegexOptions RegexOptions {
			get => _regexOptions;
			init {
				_regexOptions = value;
				if (!string.IsNullOrEmpty(_pattern))
					Regex = new Regex(_pattern, _regexOptions);
			}
		}

		internal Regex Regex { get; private init; }

		public static implicit operator EnumValueAttribute(EnumMemberAttribute attr) => new(attr.Value);

		public static implicit operator EnumValueAttribute(FieldInfo field) => new(field.Name);

		public static explicit operator EnumValueAttribute(string value) => new(value);
	}

	[AttributeUsage(AttributeTargets.Field)]
	public class EnumDefaultAttribute : Attribute { }
}