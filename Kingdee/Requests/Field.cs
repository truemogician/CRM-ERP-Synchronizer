using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Kingdee.Forms;
using Kingdee.Requests.Query;
using Newtonsoft.Json;
using Shared;
using Shared.Exceptions;

namespace Kingdee.Requests {
	public class Field : PropertyChain, IFormType {
		public Field(string propertyName) : base(propertyName) { }

		public Field(params string[] propertyNameChain) : base(propertyNameChain) { }

		public Field(PropertyChain src) : base(src) { }

		public DataType DataType
			=> Type.GetTypeCode(EndingType) switch {
				TypeCode.String                                                                                                                                => DataType.String,
				TypeCode.Single or TypeCode.Double or TypeCode.Decimal                                                                                         => DataType.Float,
				TypeCode.Byte or TypeCode.SByte or TypeCode.Int16 or TypeCode.UInt16 or TypeCode.Int32 or TypeCode.UInt32 or TypeCode.Int64 or TypeCode.UInt64 => DataType.Integer,
				TypeCode.Object                                                                                                                                => EndingType.IsAssignableTo(typeof(IEnumerable<byte>)) ? DataType.Bit : throw new TypeException(EndingType, $"Can't be mapped to {nameof(DataType)}"),
				_                                                                                                                                              => throw new TypeException(EndingType, $"Can't be mapped to {nameof(DataType)}")
			};

		public override Field this[Range range] => new(base[range]);

		public Type FormType {
			get => StartingType;
			set => StartingType = value;
		}

		public override bool Equals(object obj) {
			if (obj is null)
				return false;
			if (ReferenceEquals(this, obj))
				return true;
			return obj.GetType() == GetType() && base.Equals(obj);
		}

		public override int GetHashCode() => base.GetHashCode();

		public override string ToString(string format = null, IFormatProvider formatProvider = null)
			=> InfoChain.Aggregate(
				new StringBuilder(),
				format?.ToLowerInvariant() switch {
					"json" or "j" => (builder, prop) => prop.IsDefined(typeof(SubFormAttribute)) ? builder : builder.Append($"{prop.GetJsonPropertyName()}."),
					_             => (builder, prop) => builder.Append($"{prop.Name}.")
				},
				builder => builder.ToString(0, builder.Length - 1)
			);

		public new static Field FromString(Type type, string chain, string format = null) => throw new NotImplementedException();

		public override Field Concat(PropertyChain field) => new(base.Concat(field));

		public override Field Concat(string propertyName) => new(base.Concat(propertyName));

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

		public static implicit operator Field(PropertyInfo prop) => new(prop);
	}

	public class Field<T> : Field {
		public Field(string propertyName) : base(propertyName) => FormType = typeof(T);

		public Field(params string[] propertyNameChain) : base(propertyNameChain) => FormType = typeof(T);

		public Field(PropertyChain field) : base(field.Names.ToArray()) => FormType = typeof(T);
	}

	public interface IFormType {
		public Type FormType { get; set; }
	}
}