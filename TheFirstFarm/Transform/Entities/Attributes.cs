using System;
using System.Reflection;

namespace TheFirstFarm.Transform.Entities {
	[AttributeUsage(AttributeTargets.Class)]
	public class MapAttribute : Attribute {
		private readonly string _fExtKeyName;

		private readonly string _kExtKeyName;

		public MapAttribute() { }

		public MapAttribute(Type fModelType) => FModel = fModelType;

		public MapAttribute(Type fModelType, Type kModelType) : this(fModelType) => KModel = kModelType;

		public Type FModel { get; }

		public string FExtKeyName {
			get => _fExtKeyName;
			init {
				_fExtKeyName = value;
				FExtKey = FModel.GetMember(value, MemberTypes.Property | MemberTypes.Field);
			}
		}

		public MemberInfo FExtKey { get; private init; }

		public Type KModel { get; }

		public string KExtKeyName {
			get => _kExtKeyName;
			init {
				_kExtKeyName = value;
				KExtKey = KModel.GetMember(value, MemberTypes.Property | MemberTypes.Field);
			}
		}

		public MemberInfo KExtKey { get; private init; }
	}

	[AttributeUsage(AttributeTargets.Property)]
	public class MapReferenceAttribute : Attribute {
		public string FName { get; init; }

		public string KName { get; init; }

		public MemberInfo GetFMember<TMap>() where TMap : IIdMap => GetFMember(typeof(TMap));

		public MemberInfo GetFMember(Type declaringType) {
			var fType = declaringType.GetCustomAttribute<MapAttribute>()?.FModel;
			return fType?.GetMostDerivedMember(FName);
		}

		public MemberInfo GetKMember<TMap>() where TMap : IIdMap => GetKMember(typeof(TMap));

		public MemberInfo GetKMember(Type declaringType) {
			var kType = declaringType.GetCustomAttribute<MapAttribute>()?.KModel;
			return kType?.GetMostDerivedMember(KName);
		}
	}
}