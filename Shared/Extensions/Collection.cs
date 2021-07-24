using System.Linq;
using System.Runtime.CompilerServices;

// ReSharper disable once CheckNamespace
namespace System.Collections.Generic {
	public static class CollectionExtensions {
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> values) => values.Each(collection.Add);

		/// <summary>
		/// Remove all occurrences of a specific object from the <see cref="ICollection{T}"/>
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="collection"></param>
		/// <param name="value"><inheritdoc cref="ICollection{T}.Remove" path="/param[@name='item']"/></param>
		/// <returns>The count of occurrences removed from <see cref="ICollection{T}"/></returns>
		public static int RemoveAll<T>(this ICollection<T> collection, T value) {
			int count = 0;
			while (collection.Remove(value))
				++count;
			return count;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int RemoveRange<T>(this ICollection<T> collection, IEnumerable<T> values) => values.Count(collection.Remove);
	}
}