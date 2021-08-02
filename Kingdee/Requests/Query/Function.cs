using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Kingdee.Requests.Query {
	public sealed class Function : ArgumentCollection {
		public Function(string funcName, DataType returnType, params Expression[] arguments) {
			FuncName = funcName;
			Type = returnType;
			Arguments = arguments.ToList();
		}

		public IList<Expression> Parameters => Arguments;

		public DataType Type { get; }

		public string FuncName { get; }

		public override string ToString() => $"{FuncName}({string.Join(", ", Arguments.Select(arg => arg.ToString()))})";
	}

	public sealed class BetweenPair : ArgumentCollection {
		public BetweenPair(Expression from, Expression to)
			=> Arguments = new List<Expression>(2) {
				from,
				to
			};

		public Expression From {
			get => Arguments[0];
			set => Arguments[0] = value;
		}

		public Expression To {
			get => Arguments[1];
			set => Arguments[1] = value;
		}

		public override string ToString() => $"{From} AND {To}";
	}

	public sealed class InList : ArgumentCollection, IList<Expression> {
		public InList(params Expression[] expressions) => Arguments = expressions.ToList();

		public IEnumerator<Expression> GetEnumerator() => Arguments.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public void Add(Expression item) => Arguments.Add(item);

		public void Clear() => Arguments.Clear();

		public bool Contains(Expression item) => Arguments.Contains(item);

		public void CopyTo(Expression[] array, int arrayIndex) => Arguments.CopyTo(array, arrayIndex);

		public bool Remove(Expression item) => Arguments.Remove(item);

		public int Count => Arguments.Count;

		public bool IsReadOnly => Arguments.IsReadOnly;

		public int IndexOf(Expression item) => Arguments.IndexOf(item);

		public void Insert(int index, Expression item) => Arguments.Insert(index, item);

		public void RemoveAt(int index) => Arguments.RemoveAt(index);

		public Expression this[int index] {
			get => Arguments[index];
			set => Arguments[index] = value;
		}

		public override string ToString() => $"({string.Join(", ", Arguments.Select(arg => arg.ToString()))})";
	}

	public abstract class ArgumentCollection : IFormType {
		protected virtual IList<Expression> Arguments { get; init; }

		public Type FormType {
			get {
				Type result = null;
				foreach (var arg in Arguments)
					if (result is null)
						result = arg.FormType;
					else if (arg.FormType != result)
						throw new Exception($"{nameof(Arguments)} contains different {nameof(FormType)}");
				return result;
			}
			set {
				foreach (var arg in Arguments)
					arg.FormType = value;
			}
		}

		public abstract override string ToString();
	}
}