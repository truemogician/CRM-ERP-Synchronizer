using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Shared.Exceptions;
using Shared.Utilities;
using Shared.Validation;

namespace Shared {
	public class PropertyChain : IList<EnumerablePropertyInfo>, IFormattable {
		public PropertyChain(string propertyName) => NameChain.Add(propertyName);

		public PropertyChain(params string[] propertyNameChain) => NameChain.AddRange(propertyNameChain);

		public PropertyChain(PropertyChain src) {
			StartingType = src.StartingType;
			NameChain = new List<string>(src.NameChain);
		}

		[Required]
		public Type StartingType { get; set; }

		public EnumerablePropertyInfo StartingInfo => InfoChain.First();

		public Type EndingType => EndingInfo.PropertyType;

		public EnumerablePropertyInfo EndingInfo => InfoChain.Last();

		public int Length => NameChain.Count;

		public IReadOnlyList<string> Names => NameChain;

		public IEnumerable<int> Ranks => InfoChain.Select(info => info.Rank);

		[NotEmpty]
		protected List<string> NameChain { get; private set; } = new();

		protected IEnumerable<EnumerablePropertyInfo> InfoChain {
			get {
				Utility.ValidatedOrThrow(this);
				var type = StartingType;
				foreach (string propName in NameChain) {
					var prop = new EnumerablePropertyInfo(type.GetProperty(propName));
					if (prop is null)
						throw new TypeException(type, $"Property \"{propName}\" doesn't exist");
					yield return prop;
					type = prop.ElementType;
				}
			}
		}

		public virtual PropertyChain this[Range range] {
			get {
				int start = range.Start.GetOffset(NameChain.Count);
				int end = range.End.GetOffset(NameChain.Count);
				if (start < 0 || end > NameChain.Count)
					throw new IndexOutOfRangeException();
				var result = new PropertyChain(NameChain.GetRange(start, end - start).ToArray()) {StartingType = StartingType};
				if (start > 0) {
					using var enumerator = InfoChain.GetEnumerator();
					enumerator.MoveNext();
					for (var i = 0; i < start - 1; ++i, enumerator.MoveNext()) { }
					result.StartingType = enumerator.Current?.ElementType;
				}
				return result;
			}
		}

		public virtual string ToString(string format = null, IFormatProvider formatProvider = null) {
			Func<EnumerablePropertyInfo, string> getName = format?.ToLowerInvariant() switch {
				"json" or "j" => prop => prop.GetJsonPropertyName(),
				_             => prop => prop.Name
			};
			return InfoChain.Aggregate(
				new StringBuilder(),
				(builder, prop) => builder.Append($"{getName(prop)}."),
				builder => builder.ToString(0, builder.Length - 1)
			);
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public IEnumerator<EnumerablePropertyInfo> GetEnumerator() => InfoChain.GetEnumerator();

		public void Add(EnumerablePropertyInfo item) {
			if (item is not null)
				NameChain.Add(item.Name);
		}

		public void Clear() => NameChain.Clear();

		public bool Contains(EnumerablePropertyInfo item) => NameChain.Contains(item?.Name);

		public void CopyTo(EnumerablePropertyInfo[] array, int arrayIndex) {
			throw new NotImplementedException();
		}

		public bool Remove(EnumerablePropertyInfo item) => NameChain.Remove(item?.Name);

		public int Count => NameChain.Count;
		public bool IsReadOnly => false;
		public int IndexOf(EnumerablePropertyInfo item) => NameChain.IndexOf(item?.Name);

		public void Insert(int index, EnumerablePropertyInfo item) => NameChain.Insert(index, item?.Name);

		public void RemoveAt(int index) => NameChain.RemoveAt(index);

		public EnumerablePropertyInfo this[int index] {
			get => InfoChain.ToList()[index];
			set => NameChain[index] = value.Name;
		}

		public void FromString(string chain, string format = null) {
			var result = FromString(StartingType, chain, format);
			NameChain = result.NameChain;
		}

		public static PropertyChain FromString(Type type, string chain, string format = null) {
			Func<string, string> getPropName = format?.ToLowerInvariant() switch {
				"json" or "j" => name => type.GetMemberFromJsonPropertyName(name).Name,
				_             => name => name
			};
			string[] propertyNameChain = chain.Split('.')
				.Aggregate(
					new List<string>(),
					(list, name) => {
						list.Add(getPropName(name));
						return list;
					},
					list => list.ToArray()
				);
			return new PropertyChain(propertyNameChain) {
				StartingType = type
			};
		}

		public virtual PropertyChain Concat(PropertyChain chain) {
			if (EndingType != chain.StartingType)
				throw new Exception($"{nameof(EndingType)} of the preceding chain must equals to {nameof(StartingType)} of the succeeding chain");
			var result = new PropertyChain(this);
			result.NameChain.AddRange(chain.NameChain);
			return result;
		}

		public virtual PropertyChain Concat(string propertyName) {
			var result = new PropertyChain(this);
			result.NameChain.Add(propertyName);
			return result;
		}

		public object GetValue(object obj, params int[][] indices) {
			Utility.ValidatedOrThrow(this);
			(var chain, object cur) = (this, obj);
			var i = 0;
			while (chain.Length > 1 && chain.StartingType.IsInstanceOfType(cur)) {
				if (chain.StartingInfo is var info && info.Rank > 0)
					cur = info.GetIndexedValue(cur, indices[i++]);
				else
					cur = info.GetIndexedValue(cur);
				chain = chain[1..];
			}
			if (cur is null)
				return null;
			if (chain.Length > 1 || !chain.StartingType.IsInstanceOfType(cur))
				throw new InvariantTypeException(chain.StartingType, cur.GetType());
			return chain.EndingInfo.Rank > 0 ? chain.EndingInfo.GetIndexedValue(cur, indices[i]) : chain.EndingInfo.GetIndexedValue(cur);
		}

		public void SetValue(object obj, object value, params int[][] indices) {
			Utility.ValidatedOrThrow(this);
			(var chain, object cur) = (this, obj);
			var i = 0;
			while (chain.Length > 1 && chain.StartingType.IsInstanceOfType(cur)) {
				if (chain.StartingInfo is var info && info.Rank > 0)
					cur = info.GetIndexedValue(cur, indices[i++]);
				else
					cur = info.GetIndexedValue(cur);
				chain = chain[1..];
			}
			if (chain.Length > 1 || !chain.StartingType.IsInstanceOfType(cur))
				throw new InvariantTypeException(chain.StartingType, cur?.GetType());
			if (chain.EndingInfo.Rank > 0)
				chain.EndingInfo.SetIndexedValue(cur, value, indices[i]);
			else
				chain.EndingInfo.SetIndexedValue(cur, value);
		}

		public override int GetHashCode() => NameChain != null ? NameChain.GetHashCode() : 0;

		public override bool Equals(object obj) {
			if (ReferenceEquals(this, obj))
				return true;
			if (obj is null)
				return false;
			return obj is PropertyChain chain && Equals(chain);
		}

		protected bool Equals(PropertyChain other) => Equals(NameChain, other.NameChain) && StartingType == other.StartingType;
	}

	public class PropertyChain<T> : PropertyChain {
		public PropertyChain(string propertyName) : base(propertyName) => StartingType = typeof(T);

		public PropertyChain(params string[] propertyNameChain) : base(propertyNameChain) => StartingType = typeof(T);

		public PropertyChain(PropertyChain field) : base(field.Names.ToArray()) => StartingType = typeof(T);
	}

	public class EnumerablePropertyInfo : PropertyInfo {
		public EnumerablePropertyInfo(PropertyInfo info) {
			Info = info;
			for (Rank = 0, ElementType = info.PropertyType; ElementType.Implements(typeof(IList)); ++Rank) {
				ElementType = ElementType.GetItemType(typeof(IList<>)) ?? typeof(object);
				if (ElementType is null)
					throw new TypeException("Fail to get element type");
			}
		}

		public PropertyInfo Info { get; }
		public int Rank { get; }
		public Type ElementType { get; }

		public override Type DeclaringType => Info.DeclaringType;
		public override string Name => Info.Name;
		public override Type ReflectedType => Info.ReflectedType;

		public override PropertyAttributes Attributes => Info.Attributes;
		public override bool CanRead => Info.CanRead;
		public override bool CanWrite => Info.CanWrite;
		public override Type PropertyType => Info.PropertyType;

		public object GetIndexedValue(object obj, params int[] indices) {//ToDo: int[] -> Index[]
			if (indices.Length != Rank)
				throw new Exception($"{Rank} indices expected");
			var type = Info.PropertyType;
			if (obj.GetType() != type)
				throw new TypeNotMatchException(type, obj.GetType());
			object result = Info.GetValue(obj);
			for (var i = 0; i < Rank; ++i)
				result = (result as IList)![indices[i]];
			return result;
		}

		public void SetIndexedValue(object obj, object value, params int[] indices) {
			if (indices.Length != Rank)
				throw new ArgumentNullException($"{Rank} indices expected");
			var type = Info.PropertyType;
			if (!type.IsInstanceOfType(obj))
				throw new InvariantTypeException(type, obj?.GetType());
			object result = Info.GetValue(obj);
			for (var i = 0; i < Rank; ++i) {
				if (result!.GetType().Implements(typeof(IList)))
					throw new TypeException(result?.GetType(), $"{nameof(IList)} required for setting value");
				if (i < Rank - 1)
					result = (result as IList)![indices[i]];
				else
					(result as IList)![indices[i]] = value;
			}
		}

		public override object[] GetCustomAttributes(bool inherit) => Info.GetCustomAttributes(inherit);

		public override object[] GetCustomAttributes(Type attributeType, bool inherit) => Info.GetCustomAttributes(attributeType, inherit);

		public override bool IsDefined(Type attributeType, bool inherit) => Info.IsDefined(attributeType, inherit);
		public override MethodInfo[] GetAccessors(bool nonPublic) => Info.GetAccessors(nonPublic);

		public override MethodInfo GetGetMethod(bool nonPublic) => Info.GetGetMethod(nonPublic);

		public override ParameterInfo[] GetIndexParameters() => Enumerable.Repeat(typeof(List<int>).GetIndexer(typeof(int)).GetIndexParameters()[0], Rank).ToArray();

		public override MethodInfo GetSetMethod(bool nonPublic) => Info.GetSetMethod(nonPublic);

		public override object GetValue(object obj, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture) => index?.Length > 0 && index.All(idx => idx is int) ? GetIndexedValue(obj, index.Cast<int>().ToArray()) : Info.GetValue(obj, invokeAttr, binder, index, culture);

		public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture) {
			if (index?.Length > 0 && index.All(idx => idx is int))
				SetIndexedValue(obj, value, index.Cast<int>().ToArray());
			else
				Info.SetValue(obj, value, invokeAttr, binder, index, culture);
		}
	}
}