using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

// ReSharper disable once CheckNamespace
namespace System.Linq {
	public static class EnumerableExtensions {
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static List<T> AsList<T>(this IEnumerable<T> enumerable) => enumerable is List<T> list ? list : enumerable.ToList();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T[] AsArray<T>(this IEnumerable<T> enumerable) => enumerable is T[] array ? array : enumerable.ToArray();

		public static IEnumerable<T> AsType<T>(this IEnumerable enumerable) => from object item in enumerable select item is T result ? result : throw new InvalidCastException();

		public static TValue SameOrDefault<T, TValue>(this IEnumerable<T> enumerable, Func<T, TValue> predicate) {
			TValue reference = default;
			bool first = true;
			foreach (var item in enumerable)
				if (first) {
					reference = predicate(item);
					first = false;
				}
				else if (!predicate(item).Equals(reference))
					throw new InvalidOperationException("Values aren't the same");
			return reference;
		}

		public static TValue Same<T, TValue>(this IEnumerable<T> enumerable, Func<T, TValue> predicate) {
			TValue reference = default;
			bool first = true;
			foreach (var item in enumerable)
				if (first) {
					reference = predicate(item);
					first = false;
				}
				else if (!predicate(item).Equals(reference))
					throw new InvalidOperationException("Values aren't the same");
			return !first ? reference : throw new InvalidOperationException("Sequence contains no element");
		}
	}
}