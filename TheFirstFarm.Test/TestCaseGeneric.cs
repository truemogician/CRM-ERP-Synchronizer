using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Builders;

namespace TheFirstFarm.Test {
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public class TestCaseGenericAttribute : TestCaseAttribute, ITestBuilder {
		public TestCaseGenericAttribute(params object[] arguments)
			: base(arguments) { }

		public Type GenericArgument {
			get => GenericArguments.SingleOrDefault();
			set => GenericArguments = new[] {value};
		}

		public Type[] GenericArguments { get; set; }

		IEnumerable<TestMethod> ITestBuilder.BuildFrom(IMethodInfo method, NUnit.Framework.Internal.Test suite) {
			if (!method.IsGenericMethodDefinition)
				return BuildFrom(method, suite);

			if (GenericArguments == null || GenericArguments.Length != method.GetGenericArguments().Length) {
				var @params = new TestCaseParameters {RunState = RunState.NotRunnable};
				@params.Properties.Set("_SKIPREASON", $"{nameof(GenericArguments)} should have {method.GetGenericArguments().Length} elements");
				return new[] {new NUnitTestCaseBuilder().BuildTestMethod(method, suite, @params)};
			}

			var genMethod = method.MakeGenericMethod(GenericArguments);
			return BuildFrom(genMethod, suite);
		}
	}
}