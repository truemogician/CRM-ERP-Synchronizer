using System;

namespace TheFirstFarm.Utilities {
	public static class Utility {
		public static TTarget? TransformEnum<TSource, TTarget>(TSource source) where TSource : struct, Enum where TTarget : struct, Enum {
			string sourceName = Enum.GetName(source);
			return Enum.TryParse<TTarget>(sourceName, out var result) ? result : null;
		}
	}
}