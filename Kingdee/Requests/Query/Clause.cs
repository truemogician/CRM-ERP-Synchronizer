using System;

namespace Kingdee.Requests.Query {
	public class Clause : IFormType {
		internal Clause(Expression left, ComparisonOperator @operator, Expression right) {
			Left = left;
			Operator = @operator;
			Right = right;
		}

		public Expression Left { get; init; }

		public Expression Right { get; init; }

		public ComparisonOperator Operator { get; init; }

		public Type FormType {
			get {
				var (left, right) = (Left.FormType, Right.FormType);
				return left is null ? right : right is null ? left : left == right ? left : throw new Exception($"Two operands have different {nameof(FormType)}");
			}
			set {
				Left.FormType = value;
				Right.FormType = value;
			}
		}

		public override string ToString() => $"({Left} {Operator} {Right})";
	}
}