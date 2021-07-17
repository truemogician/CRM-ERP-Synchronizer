using System;
using System.Collections.Generic;
using System.Linq;
using OneOf;

namespace Kingdee.Requests.Query {
	public class Function : IFormType {
		public Function(string funcName, DataType returnType, params OneOf<Column, Literal>[] arguments) {
			FuncName = funcName;
			Type = returnType;
			Arguments = arguments.ToList();
		}

		public DataType Type { get; }
		public string FuncName { get; }
		public List<OneOf<Column, Literal>> Arguments { get; }
		public override string ToString() => $"{FuncName}({string.Join(',', Arguments.Select(arg => arg.IsT0 ? arg.AsT0.ToString() : arg.AsT1.ToString()))})";

		public Type FormType {
			get {
				Type result = null;
				foreach (var arg in Arguments.Where(arg => !arg.IsT1))
					if (result is null)
						result = arg.AsT0.FormType;
					else if (arg.AsT0.FormType != result)
						throw new Exception($"{nameof(Arguments)} contains different {nameof(FormType)}");
				return result;
			}
			set {
				foreach (var arg in Arguments.Where(arg => !arg.IsT1))
					arg.AsT0.FormType = value;
			}
		}
	}
}