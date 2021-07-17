using System;
using OneOf;

namespace Kingdee.Requests.Query {
	public class Expression : ExpressionBase<ExpressionBody, Expression, ArithmeticOperator> {
		private Expression(OneOf<ArgumentCollection, Field, Literal> body) : base(body) { }

		private Expression(Expression left, ArithmeticOperator @operator, Expression right) : base(left, @operator, right) { }

		public override bool Equals(object obj) {
			if (obj is null)
				return false;
			if (ReferenceEquals(this, obj))
				return true;
			return obj.GetType() == GetType() && Equals((Expression)obj);
		}

		public override int GetHashCode() => HashCode.Combine(Body, Operator, Left, Right);

		public Clause Like(string pattern) => new(this, ComparisonOperator.Like, pattern);

		public Clause NotLike(string pattern) => new(this, ComparisonOperator.NotLike, pattern);

		public Clause Between(Expression from, Expression to) => new(this, ComparisonOperator.Between, new BetweenPair(from, to));

		public Clause NotBetween(Expression from, Expression to) => new(this, ComparisonOperator.NotBetween, new BetweenPair(from, to));

		public Clause In(params Expression[] expressions) => new(this, ComparisonOperator.In, new InList(expressions));

		public Clause NotIn(params Expression[] expressions) => new(this, ComparisonOperator.NotIn, new InList(expressions));

		protected bool Equals(Expression other) => Equals(Body, other.Body) && Equals(Operator, other.Operator) && Equals(Left, other.Left) && Equals(Right, other.Right);

		#region Conversion Operators
		public static implicit operator Expression(ArgumentCollection value) => new(value);
		public static implicit operator Expression(Field value) => new(value);
		public static implicit operator Expression(Literal value) => new(value);
		public static implicit operator Expression(string value) => (Literal)value;
		public static implicit operator Expression(long value) => (Literal)value;
		public static implicit operator Expression(int value) => (Literal)value;
		public static implicit operator Expression(double value) => (Literal)value;
		public static implicit operator Expression(float value) => (Literal)value;
		#endregion

		//ToDo: Add DataType checking

		#region Arithmetic Operators
		public static Expression operator +(Expression left, Expression right) => new(left, ArithmeticOperator.Add, right);
		public static Expression operator -(Expression left, Expression right) => new(left, ArithmeticOperator.Subtract, right);
		public static Expression operator *(Expression left, Expression right) => new(left, ArithmeticOperator.Multiply, right);
		public static Expression operator /(Expression left, Expression right) => new(left, ArithmeticOperator.Divide, right);
		public static Expression operator %(Expression left, Expression right) => new(left, ArithmeticOperator.Modulo, right);
		public static Expression operator &(Expression left, Expression right) => new(left, ArithmeticOperator.And, right);
		public static Expression operator |(Expression left, Expression right) => new(left, ArithmeticOperator.Or, right);
		public static Expression operator ^(Expression left, Expression right) => new(left, ArithmeticOperator.ExclusiveOr, right);
		#endregion

		#region Comparison Operators
		public static Clause operator ==(Expression left, Expression right) => new(left, ComparisonOperator.Equal, right);
		public static Clause operator !=(Expression left, Expression right) => new(left, ComparisonOperator.NotEqual, right);
		public static Clause operator >(Expression left, Expression right) => new(left, ComparisonOperator.Greater, right);
		public static Clause operator <=(Expression left, Expression right) => new(left, ComparisonOperator.LessEqual, right);
		public static Clause operator <(Expression left, Expression right) => new(left, ComparisonOperator.Less, right);
		public static Clause operator >=(Expression left, Expression right) => new(left, ComparisonOperator.GreaterEqual, right);
		#endregion
	}

	public class ExpressionBody : IFormType {
		private readonly OneOf<ArgumentCollection, Field, Literal> _content;
		public ExpressionBody(OneOf<ArgumentCollection, Field, Literal> content) => _content = content;

		public Type FormType {
			get {
				if (_content.IsT0)
					return _content.AsT0.FormType;
				return _content.IsT1 ? _content.AsT1.FormType : null;
			}
			set {
				if (_content.IsT0)
					_content.AsT0.FormType = value;
				if (_content.IsT1)
					_content.AsT1.FormType = value;
			}
		}

		public static implicit operator ExpressionBody(OneOf<ArgumentCollection, Field, Literal> oneOf) => new(oneOf);
		public static implicit operator ExpressionBody(Function function) => new(function);
		public static implicit operator ExpressionBody(Field column) => new(column);
		public static implicit operator ExpressionBody(Literal literal) => new(literal);
		public static implicit operator OneOf<ArgumentCollection, Field, Literal>(ExpressionBody exp) => exp._content;

		public override string ToString()
			=> _content.Index switch {
				0 => _content.AsT0.ToString(),
				1 => _content.AsT1.ToString(),
				2 => _content.AsT2.ToString(),
				_ => throw new InvalidOperationException($"{nameof(_content.Index)} out of range")
			};
	}

	public class ExpressionBase<TBody, TOperand, TOperator> : IFormType where TBody : class, IFormType where TOperand : ExpressionBase<TBody, TOperand, TOperator> where TOperator : Operator {
		protected ExpressionBase(TBody body) => Body = body;

		protected ExpressionBase(TOperand left, TOperator @operator, TOperand right) {
			Left = left;
			Operator = @operator;
			Right = right;
		}

		public TBody Body { get; init; }

		public TOperator Operator { get; init; }
		public TOperand Left { get; init; }
		public TOperand Right { get; init; }

		public bool IsLeaf => Body is not null;

		public Type FormType {
			get {
				if (IsLeaf)
					return Body.FormType;
				var (left, right) = (Left.FormType, Right.FormType);
				return left is null ? right : right is null ? left : left == right ? left : throw new Exception($"Two operands have different {nameof(FormType)}");
			}
			set {
				if (IsLeaf)
					Body.FormType = value;
				else {
					Left.FormType = value;
					Right.FormType = value;
				}
			}
		}

		public override string ToString() => IsLeaf ? Body.ToString() : $"({Left} {Operator} {Right})";
	}
}