using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using Kingdee.Requests.Query;
using Shared.Exceptions;
using Shared.Utilities;
using Shared.Validators;
using DataType = Kingdee.Requests.Query.DataType;

namespace Kingdee.Requests {
	public class Field : IFormType, IFormattable {
		public Field(string propertyName) => PropertyNameChain.Add(propertyName);

		public Field(params string[] propertyNameChain) => PropertyNameChain.AddRange(propertyNameChain);

		public Field(Field src) {
			FormType = src.FormType;
			PropertyNameChain = new List<string>(src.PropertyNameChain);
		}

		public PropertyInfo StartingProperty => PropertyChain.First();

		public PropertyInfo EndingProperty => PropertyChain.Last();

		public Type Type => EndingProperty.PropertyType;

		public DataType DataType
			=> Type.GetTypeCode(Type) switch {
				TypeCode.String                                                                                                                                => DataType.String,
				TypeCode.Single or TypeCode.Double or TypeCode.Decimal                                                                                         => DataType.Float,
				TypeCode.Byte or TypeCode.SByte or TypeCode.Int16 or TypeCode.UInt16 or TypeCode.Int32 or TypeCode.UInt32 or TypeCode.Int64 or TypeCode.UInt64 => DataType.Integer,
				TypeCode.Object                                                                                                                                => Type.IsSubclassOf(typeof(IEnumerable<byte>)) ? DataType.Bit : throw new TypeException(Type, $"Can't be mapped to {nameof(DataType)}"),
				_                                                                                                                                              => throw new TypeException(Type, $"Can't be mapped to {nameof(DataType)}")
			};

		public int Length => PropertyNameChain.Count;

		public IReadOnlyList<string> PropertyNames => PropertyNameChain;

		[NotEmpty]
		protected List<string> PropertyNameChain { get; } = new();

		protected IEnumerable<PropertyInfo> PropertyChain {
			get {
				Utility.ValidatedOrThrow(this);
				var type = FormType;
				foreach (string propName in PropertyNameChain) {
					var prop = type.GetProperty(propName);
					if (prop is null)
						throw new TypeException(type, $"Property \"{propName}\" doesn't exist");
					yield return prop;
					type = prop.PropertyType;
				}
			}
		}

		public Field this[Range range] {
			get {
				int start = range.Start.GetOffset(PropertyNameChain.Count);
				int end = range.End.GetOffset(PropertyNameChain.Count);
				if (start < 0 || end >= PropertyNameChain.Count)
					throw new IndexOutOfRangeException();
				var result = new Field(PropertyNameChain.GetRange(start, end - start).ToArray());
				using var enumerator = PropertyChain.GetEnumerator();
				enumerator.MoveNext();
				for (int i = 0; i < start; ++i, enumerator.MoveNext()) { }
				result.FormType = enumerator.Current?.PropertyType;
				return result;
			}
		}

		public string ToString(string format = null, IFormatProvider formatProvider = null) {
			Func<PropertyInfo, string> getName = format?.ToLowerInvariant() switch {
				"json" or "j" => prop => prop.GetJsonPropertyName(),
				_             => prop => prop.Name
			};
			return PropertyChain.Aggregate(
				new StringBuilder(),
				(builder, prop) => builder.Append($"{getName(prop)}."),
				builder => builder.ToString(0, builder.Length - 1)
			);
		}

		[Required]
		public Type FormType { get; set; }

		public static Field FromString(Type type, string chain, string format = null) {
			Func<string, string> getPropName = format?.ToLowerInvariant() switch {
				"json" or "j" => name => type.GetPropertyFromJsonPropertyName(name).Name,
				_             => name => name
			};
			string[] propertyNameChain = chain.Split('.')
				.Aggregate(
					new List<string>(),
					(list, name) => {
						list.Add(getPropName(name));
						return list;
					},
					list => list.ToArray()
				);
			return new Field(propertyNameChain) {
				FormType = type
			};
		}

		public Field Concat(Field field) {
			if (Type != field.FormType)
				throw new Exception($"{nameof(Type)} of the preceding field must equals to {nameof(FormType)} of the succeeding field");
			var result = new Field(this);
			result.PropertyNameChain.AddRange(field.PropertyNameChain);
			return result;
		}

		public Field Concat(string propertyName) {
			var result = new Field(this);
			result.PropertyNameChain.Add(propertyName);
			return result;
		}

		public object GetValue(object obj) {
			Utility.ValidatedOrThrow(this);
			(var field, object cur) = (this, obj);
			while (field.Length > 1 && field.FormType.IsInstanceOfType(cur)) {
				cur = field.StartingProperty.GetValue(cur);
				field = field[1..];
			}
			if (field.Length > 1 || !field.FormType.IsInstanceOfType(cur))
				throw new InvariantTypeException(field.FormType, cur?.GetType());
			return field.StartingProperty.GetValue(cur);
		}

		public void SetValue(object obj, object value) {
			Utility.ValidatedOrThrow(this);
			(var field, object cur) = (this, obj);
			while (field.Length > 1 && field.FormType.IsInstanceOfType(cur)) {
				cur = field.StartingProperty.GetValue(cur);
				field = field[1..];
			}
			if (field.Length > 1 || !field.FormType.IsInstanceOfType(cur))
				throw new InvariantTypeException(field.FormType, cur?.GetType());
			field.StartingProperty.SetValue(cur, value);
		}

		public override int GetHashCode() => PropertyNameChain != null ? PropertyNameChain.GetHashCode() : 0;

		public override bool Equals(object obj) {
			if (ReferenceEquals(this, obj))
				return true;
			if (obj is null)
				return false;
			return obj.GetType().IsSubclassOf(typeof(Field)) && Equals(obj as Field);
		}

		protected bool Equals(Field other) => Equals(PropertyNameChain, other.PropertyNameChain) && FormType == other.FormType;

		#region Arithmetic Operators
		public static Expression operator +(Field left, Expression right) => (Expression)left + right;
		public static Expression operator -(Field left, Expression right) => (Expression)left - right;
		public static Expression operator *(Field left, Expression right) => (Expression)left * right;
		public static Expression operator /(Field left, Expression right) => (Expression)left / right;
		public static Expression operator %(Field left, Expression right) => (Expression)left % right;
		public static Expression operator &(Field left, Expression right) => (Expression)left & right;
		public static Expression operator |(Field left, Expression right) => (Expression)left | right;
		public static Expression operator ^(Field left, Expression right) => (Expression)left ^ right;
		#endregion

		#region Comparison Operators
		public static Clause operator ==(Field left, Expression right) => (Expression)left == right;
		public static Clause operator !=(Field left, Expression right) => (Expression)left != right;
		public static Clause operator >(Field left, Expression right) => (Expression)left > right;
		public static Clause operator <=(Field left, Expression right) => (Expression)left <= right;
		public static Clause operator <(Field left, Expression right) => (Expression)left < right;
		public static Clause operator >=(Field left, Expression right) => (Expression)left >= right;
		#endregion
	}

	public class Field<T> : Field {
		public Field(string propertyName) : base(propertyName) => FormType = typeof(T);

		public Field(params string[] propertyNameChain) : base(propertyNameChain) => FormType = typeof(T);

		public Field(Field field) : base(field.PropertyNames.ToArray()) => FormType = typeof(T);
	}

	public interface IFormType {
		public Type FormType { get; set; }
	}
}