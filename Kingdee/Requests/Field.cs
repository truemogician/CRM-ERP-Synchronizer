using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Kingdee.Requests.Query;
using Shared.Exceptions;
using Shared.Utilities;

namespace Kingdee.Requests {
	public class Field : IFormType {
		public Field(string propertyName) => PropertyNameChain.Add(propertyName);

		public Field(params string[] propertyNameChain) => PropertyNameChain.AddRange(propertyNameChain);

		public List<string> PropertyNameChain { get; } = new();

		protected IEnumerable<PropertyInfo> PropertyChain {
			get {
				if (PropertyNameChain.Count == 0)
					throw new EmptyCollectionException($"{nameof(PropertyNameChain)} is empty");
				if (FormType is null)
					throw new NullReferenceException($"{nameof(FormType)} hasn't been set");
				var type = FormType;
				foreach (string propName in PropertyNameChain) {
					var prop = type.GetProperty(propName);
					if (prop is null)
						throw new TypeReflectionException(type, $"Property \"{propName}\" doesn't exist");
					yield return prop;
					type = prop.PropertyType;
				}
			}
		}

		public Type Type => PropertyChain.Last().PropertyType;

		public DataType DataType
			=> Type.GetTypeCode(Type) switch {
				TypeCode.String                                                                                                                                => DataType.String,
				TypeCode.Single or TypeCode.Double or TypeCode.Decimal                                                                                         => DataType.Float,
				TypeCode.Byte or TypeCode.SByte or TypeCode.Int16 or TypeCode.UInt16 or TypeCode.Int32 or TypeCode.UInt32 or TypeCode.Int64 or TypeCode.UInt64 => DataType.Integer,
				TypeCode.Object                                                                                                                                => Type.IsSubclassOf(typeof(IEnumerable<byte>)) ? DataType.Bit : throw new TypeReflectionException(Type, $"Can't be mapped to {nameof(DataType)}"),
				_                                                                                                                                              => throw new TypeReflectionException(Type, $"Can't be mapped to {nameof(DataType)}")
			};

		public Type FormType { get; set; }

		public override string ToString()
			=> PropertyChain.Aggregate(
				new StringBuilder(),
				(builder, prop) => builder.Append($"{prop.GetJsonPropertyName()}."),
				builder => builder.ToString(0, builder.Length - 1)
			);

		public override int GetHashCode() => PropertyNameChain != null ? PropertyNameChain.GetHashCode() : 0;

		public override bool Equals(object obj) {
			if (ReferenceEquals(this, obj))
				return true;
			if (obj is null)
				return false;
			throw new NotImplementedException();
		}

		protected bool Equals(Field other) => Equals(PropertyNameChain, other.PropertyNameChain);

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

		public Field(Field field) : base(field.PropertyNameChain.ToArray()) => FormType = typeof(T);
	}

	public interface IFormType {
		public Type FormType { get; set; }
	}
}