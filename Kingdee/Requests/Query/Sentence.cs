namespace Kingdee.Requests.Query {
	public class Sentence : ExpressionBase<Clause, Sentence, LogicalOperator> {
		protected Sentence(Clause body) : base(body) { }
		protected Sentence(Sentence left, LogicalOperator @operator, Sentence right) : base(left, @operator, right) { }

		public static implicit operator Sentence(Clause value) => new(value);
		public static Sentence operator &(Sentence left, Sentence right) => new(left, LogicalOperator.And, right);
		public static Sentence operator |(Sentence left, Sentence right) => new(left, LogicalOperator.Or, right);
	}

	public class Sentence<T> : Sentence {
		private Sentence(Clause body) : base(body) => FormType = typeof(T);
		private Sentence(Sentence left, LogicalOperator @operator, Sentence right) : base(left, @operator, right) => FormType = typeof(T);

		public Sentence(Sentence sentence) : base(sentence.Body) {
			Left = sentence.Left;
			Right = sentence.Left;
			Operator = sentence.Operator;
			FormType = typeof(T);
		}

		public static implicit operator Sentence<T>(Clause value) => new(value);
		public static Sentence<T> operator &(Sentence<T> left, Sentence<T> right) => new(left, LogicalOperator.And, right);
		public static Sentence<T> operator |(Sentence<T> left, Sentence<T> right) => new(left, LogicalOperator.Or, right);
	}
}