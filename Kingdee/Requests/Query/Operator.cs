using System;
using System.Linq;

namespace Kingdee.Requests.Query {
	public class Operator {
		protected Operator(string symbol) => Symbol = symbol;

		public string Symbol { get; }

		public static implicit operator string(Operator self) => self.Symbol;

		public override string ToString() => Symbol;
	}

	public class ComparisonOperator : Operator {
		public static readonly ComparisonOperator Equal = new("=");

		public static readonly ComparisonOperator NotEqual = !Equal;

		public static readonly ComparisonOperator Greater = new(">");

		public static readonly ComparisonOperator LessEqual = !Greater;

		public static readonly ComparisonOperator Less = new("<");

		public static readonly ComparisonOperator GreaterEqual = !Less;

		public static readonly ComparisonOperator Like = new("LIKE");

		public static readonly ComparisonOperator NotLike = !Like;

		public static readonly ComparisonOperator Between = new("BETWEEN");

		public static readonly ComparisonOperator NotBetween = !Between;

		public static readonly ComparisonOperator In = new("IN");

		public static readonly ComparisonOperator NotIn = !In;
		private ComparisonOperator(string symbol) : base(symbol) { }

		public static ComparisonOperator operator !(ComparisonOperator self)
			=> new(self.Symbol switch {
				"="  => "<>",
				"<>" => "=",
				">"  => "<=",
				"<=" => ">",
				"<"  => ">=",
				">=" => "<",
				_ => self.Symbol.All(char.IsLetter)
					? self.Symbol.StartsWith("NOT ") ? self.Symbol[4..] : "NOT " + self.Symbol
					: throw new Exception("Invalid operator symbol")
			});
	}

	public class ArithmeticOperator : Operator {
		public static readonly ArithmeticOperator Add = new("+");
		public static readonly ArithmeticOperator Subtract = new("-");
		public static readonly ArithmeticOperator Multiply = new("*");
		public static readonly ArithmeticOperator Divide = new("/");
		public static readonly ArithmeticOperator Modulo = new("%");
		public static readonly ArithmeticOperator And = new("&");
		public static readonly ArithmeticOperator Or = new("|");
		public static readonly ArithmeticOperator ExclusiveOr = new("^");
		protected ArithmeticOperator(string symbol) : base(symbol) { }
	}

	public class LogicalOperator : Operator {
		public static readonly LogicalOperator And = new("AND");
		public static readonly LogicalOperator Or = new("OR");
		protected LogicalOperator(string symbol) : base(symbol) { }
	}
}