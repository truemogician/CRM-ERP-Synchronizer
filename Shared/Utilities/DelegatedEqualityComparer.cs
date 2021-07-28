// ReSharper disable once CheckNamespace
namespace System.Collections.Generic {
	public sealed class DelegatedEqualityComparer<T> : IEqualityComparer<T> {
		public DelegatedEqualityComparer(Func<T, T, bool> equals) => _equals = equals;

		public DelegatedEqualityComparer(Func<T, T, bool> equals, Func<T, int> getHashCode) : this(equals) => _getHashCode = getHashCode;

		private readonly Func<T, int> _getHashCode = x => x.GetHashCode();

		private readonly Func<T, T, bool> _equals;

		public bool Equals(T x, T y) => _equals(x, y);

		public int GetHashCode(T obj) => _getHashCode(obj);

		public static implicit operator DelegatedEqualityComparer<T>(Func<T, T, bool> equals) => new(equals);
	}
}