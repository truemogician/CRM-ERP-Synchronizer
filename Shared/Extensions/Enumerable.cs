using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using OneOf;
using Shared.Exceptions;

// ReSharper disable once CheckNamespace
namespace System.Linq {
	public static class EnumerableExtensions {
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static List<T> AsList<T>(this IEnumerable<T> enumerable) => enumerable is List<T> list ? list : enumerable.ToList();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T[] AsArray<T>(this IEnumerable<T> enumerable) => enumerable is T[] array ? array : enumerable.ToArray();

		public static IEnumerable<T> AsType<T>(this IEnumerable enumerable) => from object item in enumerable select item is T result ? result : throw new InvalidCastException();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IndexedEnumerable<T> ToIndexed<T>(this IEnumerable<T> enumerable) => new(enumerable);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T SameOrDefault<T>(this IEnumerable<T> enumerable) => enumerable.SameOrDefault(x => x);

		public static TResult SameOrDefault<TSource, TResult>(this IEnumerable<TSource> enumerable, Func<TSource, TResult> predicate) {
			TResult reference = default;
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

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T Same<T>(this IEnumerable<T> enumerable) => enumerable.Same(x => x);

		public static TResult Same<TSource, TResult>(this IEnumerable<TSource> enumerable, Func<TSource, TResult> predicate) {
			TResult reference = default;
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

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Each<T>(this IEnumerable<T> enumerable, Action<T> action) {
			foreach (var item in enumerable)
				action(item);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Each<T>(this IEnumerable<T> enumerable, Action<T, int> action) {
			foreach ((var item, int index) in enumerable.ToIndexed())
				action(item, index);
		}

		public static IEnumerable<TResult> SelectSingleOrMany<TSource, TResult>(this IEnumerable<TSource> enumerable, Func<TSource, object> selector) {
			foreach (var item in enumerable) {
				object result = selector(item);
				switch (result) {
					case IEnumerable<TResult> subEnumerable: {
						foreach (var subItem in subEnumerable)
							yield return subItem;
						break;
					}
					case TResult res:
						yield return res;
						break;
					default: throw new TypeException(result.GetType(), $"Should be covariant with {typeof(TResult).FullName} or {typeof(IEnumerable<TResult>).FullName}");
				}
			}
		}
	}

	public class IndexedEnumerable<T> : IEnumerable<(T Value, int Index)> {
		public IndexedEnumerable(IEnumerable<T> enumerable) => Enumerable = enumerable;

		protected IEnumerable<T> Enumerable { get; }

		public IEnumerator<(T Value, int Index)> GetEnumerator() {
			int index = 0;
			foreach (var item in Enumerable)
				yield return (item, index++);
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}