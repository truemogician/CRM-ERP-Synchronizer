using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using Shared.Exceptions;
using Shared.Serialization;

namespace Kingdee.Requests.Query {
	public class Literal {
		private static readonly Regex BinaryPattern = new(@"^[01]*$", RegexOptions.Compiled);

		private static readonly Regex HexadecimalPattern = new(@"^[\da-f]*$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

		private Literal(DataType type, object value) {
			Type = type;
			Value = value;
		}

		public Literal(object value) {
			Type = System.Type.GetTypeCode(value.GetType()) switch {
				TypeCode.String                                                                                                                                => DataType.String,
				TypeCode.Double or TypeCode.Single                                                                                                             => DataType.Float,
				TypeCode.SByte or TypeCode.Int16 or TypeCode.Int32 or TypeCode.Int64 or TypeCode.Byte or TypeCode.UInt16 or TypeCode.UInt32 or TypeCode.UInt64 => DataType.Integer,
				_                                                                                                                                              => throw new TypeException(value.GetType())
			};
			Value = value;
		}

		public DataType Type { get; }

		public object Value { get; }

		public static Literal String(string value) => new(DataType.String, value);

		public static Literal Integer(long value) => new(DataType.Integer, value);

		public static Literal Float(double value) => new(DataType.Float, value);

		public static Literal Binary(string value, BitRadix radix = BitRadix.Binary)
			=> new(DataType.Bit, radix switch {
				BitRadix.Binary      => BinaryPattern.IsMatch(value) ? $"B'{value}'" : throw new RegexNotMatchException(value, BinaryPattern),
				BitRadix.Hexadecimal => HexadecimalPattern.IsMatch(value) ? $"X'{value}'" : throw new RegexNotMatchException(value, HexadecimalPattern),
				_                    => throw new EnumValueOutOfRangeException(typeof(BitRadix), radix)
			});

		public static implicit operator Literal(string value) => String(value);

		public static implicit operator Literal(long value) => Integer(value);

		public static implicit operator Literal(int value) => Integer(value);

		public static implicit operator Literal(double value) => Float(value);

		public static implicit operator Literal(float value) => Float(value);

		public static implicit operator Literal(Enum value) {
			if (value.GetType().GetMembersWithAnyAttributes(typeof(EnumValueAttribute), typeof(EnumMemberAttribute)).Length > 0)
				return String(value.GetValue());
			return Integer(Convert.ToInt64(value));
		}

		public static implicit operator Literal(DateTime value) => String(value.ToString("s"));

		public override string ToString()
			=> Type switch {
				DataType.Bit     => Value as string,
				DataType.Float   => ((double)Value).ToString(CultureInfo.InvariantCulture),
				DataType.Integer => ((long)Value).ToString(),
				DataType.String  => $"'{((string)Value).Replace("'", "''")}'",
				_                => throw new EnumValueOutOfRangeException(typeof(DataType), Type)
			};
	}

	public enum DataType : byte {
		String,

		Integer,

		Float,

		Bit
	}

	public enum BitRadix : byte {
		Binary,

		Hexadecimal
	}
}