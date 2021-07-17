using System.Globalization;
using System.Text.RegularExpressions;
using Shared.Exceptions;

namespace Kingdee.Requests.Query {
	public class Literal {
		private static readonly Regex BinaryPattern = new(@"^[01]*$", RegexOptions.Compiled);
		private static readonly Regex HexadecimalPattern = new(@"^[\da-f]*$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

		private Literal(DataType type, dynamic value) {
			Type = type;
			Value = value;
		}

		public DataType Type { get; }
		public dynamic Value { get; }

		public static Literal String(string value) => new(DataType.String, value);

		public static Literal Integer(long value) => new(DataType.Integer, value);

		public static Literal Float(double value) => new(DataType.Float, value);

		public static Literal Binary(string value, BitRadix radix = BitRadix.Binary)
			=> new(DataType.Bit, radix switch {
				BitRadix.Binary      => BinaryPattern.IsMatch(value) ? $"B'{value}'" : throw new RegexNotMatchException(value, BinaryPattern),
				BitRadix.Hexadecimal => HexadecimalPattern.IsMatch(value) ? $"X'{value}'" : throw new RegexNotMatchException(value, HexadecimalPattern),
				_                    => throw new EnumValueOutOfRangeException<BitRadix>()
			});

		public static implicit operator Literal(string value) => String(value);
		public static implicit operator Literal(long value) => Integer(value);
		public static implicit operator Literal(int value) => Integer(value);
		public static implicit operator Literal(double value) => Float(value);
		public static implicit operator Literal(float value) => Float(value);

		public override string ToString()
			=> Type switch {
				DataType.Bit     => Value,
				DataType.Float   => ((double)Value).ToString(CultureInfo.InvariantCulture),
				DataType.Integer => ((long)Value).ToString(),
				DataType.String  => $"'{((string)Value).Replace("'", "''")}'",
				_                => throw new EnumValueOutOfRangeException<DataType>()
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