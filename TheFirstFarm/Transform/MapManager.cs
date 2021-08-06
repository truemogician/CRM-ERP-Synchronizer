using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FXiaoKe.Models;
using FXiaoKe.Requests;
using Kingdee.Exceptions;
using Kingdee.Forms;
using Kingdee.Requests;
using Kingdee.Requests.Query;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Shared.Exceptions;
using TheFirstFarm.Transform.Entities;
using FClient = FXiaoKe.Client;
using KClient = Kingdee.Client;

namespace TheFirstFarm.Transform {
	public class MapManager {
		private static readonly MethodInfo QueryByFXiaoKeIdMethod = typeof(MapManager).GetMethod(nameof(QueryByFXiaoKeId), BindingFlags.NonPublic | BindingFlags.Instance);

		private static readonly MethodInfo QueryByKingdeeIdMethod = typeof(MapManager).GetMethod(nameof(QueryByKingdeeId), BindingFlags.NonPublic | BindingFlags.Instance);

		private static readonly MethodInfo QueryFXiaoKeByMapPropertyMethod = typeof(MapManager).GetMethod(nameof(QueryFXiaoKeByMapProperty), BindingFlags.NonPublic | BindingFlags.Instance);

		private static readonly MethodInfo QueryKingdeeByMapPropertyMethod = typeof(MapManager).GetMethod(nameof(QueryKingdeeByMapProperty), BindingFlags.NonPublic | BindingFlags.Instance);

		private static readonly Dictionary<Type, MapInfo> MapTypeInfos = new();

		public MapManager(FClient fClient, KClient kClient, MapContext ctx = null) {
			FClient = fClient;
			KClient = kClient;
			Context = ctx ?? new MapContext();
		}

		public FClient FClient { get; set; }

		public KClient KClient { get; set; }

		public MapContext Context { get; set; }

		private static MapInfo GetMapInfo(Type mapType) {
			if (MapTypeInfos.ContainsKey(mapType))
				return MapTypeInfos[mapType];
			var info = new MapInfo(mapType);
			MapTypeInfos.Add(mapType, info);
			return info;
		}

		private async Task<TMap> QueryByFXiaoKeId<TMap, TModel>(string id, bool save = true) where TMap : class, IIdMap, new() where TModel : CrmModelBase {
			var model = await FClient.QueryById<TModel>(id);
			if (model is null)
				return null;
			var map = new TMap {FXiaoKeId = id};
			var mapInfo = GetMapInfo(typeof(TMap));
			if (mapInfo.MapAttribute!.FExtKey is not null)
				map.KingdeeId = mapInfo.MapAttribute.FExtKey.GetValue(model) as int?;
			foreach (var (prop, attr) in mapInfo.MapReferenceAttributes) {
				if (attr is null || string.IsNullOrEmpty(attr.FName))
					continue;
				prop.SetValue(map, attr.GetFMember(mapInfo.MapType).GetValue(model));
			}
			if (save)
				lock (Context) {
					Context.AddOrUpdate(map);
					Context.SaveChanges();
				}
			return map;
		}

		private async Task<TMap> QueryByKingdeeId<TMap, TForm>(int id, bool save = true) where TMap : class, IIdMap, new() where TForm : ErpModelBase {
			var keyInfo = FormMeta<TForm>.Key;
			var resp = await KClient.QueryAsync(new QueryRequest<TForm>((Field)keyInfo == id));
			if (resp.IsT0)
				throw new RequestFailedException(resp.AsT0);
			var form = resp.AsT1.SingleOrDefault();
			if (form is null)
				return null;
			var map = new TMap {KingdeeId = id};
			var mapInfo = GetMapInfo(typeof(TMap));
			if (mapInfo.MapAttribute!.KExtKey is not null)
				map.FXiaoKeId = mapInfo.MapAttribute.KExtKey.GetValue(form) as string;
			foreach (var (prop, attr) in mapInfo.MapReferenceAttributes) {
				if (attr is null || string.IsNullOrEmpty(attr.KName))
					continue;
				prop.SetValue(map, attr.GetKMember(mapInfo.MapType).GetValue(form));
			}
			if (save)
				lock (Context) {
					Context.AddOrUpdate(map);
					Context.SaveChanges();
				}
			return map;
		}

		private async Task<TMap> QueryFXiaoKeByMapProperty<TMap, TModel>(string propName, object value, bool save = true) where TMap : class, IIdMap, new() where TModel : CrmModelBase {
			var mapInfo = GetMapInfo(typeof(TMap));
			var property = mapInfo.GetProperty(propName);
			if (property is null)
				throw new MemberNotFoundException(mapInfo.MapType, propName, MemberTypes.Property);
			var mapRefAttr = mapInfo.MapReferenceAttributes[property];
			string fName = mapRefAttr?.GetFMember<TMap>()?.Name;
			if (string.IsNullOrEmpty(fName))
				throw new Exception();//ToDo: better exception
			var model = (await FClient.QueryByCondition(new[] {ModelFilter<TModel>.Equal(fName, value)}, false))?.SingleOrDefault();
			if (model is null)
				return null;
			var map = new TMap {FXiaoKeId = model.DataId};
			if (mapInfo.MapAttribute!.FExtKey is not null)
				map.KingdeeId = mapInfo.MapAttribute.FExtKey.GetValue(model);
			foreach (var (prop, attr) in mapInfo.MapReferenceAttributes) {
				if (attr is null || string.IsNullOrEmpty(attr.FName))
					continue;
				prop.SetValue(map, attr.GetFMember(mapInfo.MapType).GetValue(model));
			}
			if (save)
				lock (Context) {
					Context.AddOrUpdate(map);
					Context.SaveChanges();
				}
			return map;
		}

		private async Task<TMap> QueryKingdeeByMapProperty<TMap, TForm>(string propName, object value, bool save = true) where TMap : class, IIdMap, new() where TForm : ErpModelBase {
			var mapInfo = GetMapInfo(typeof(TMap));
			var property = mapInfo.GetProperty(propName);
			if (property is null)
				throw new MemberNotFoundException(mapInfo.MapType, propName, MemberTypes.Property);
			var mapRefAttr = mapInfo.MapReferenceAttributes[property];
			string kName = mapRefAttr?.GetKMember<TMap>()?.Name;
			if (string.IsNullOrEmpty(kName))
				throw new Exception();//ToDo: better exception
			var resp = await KClient.QueryAsync(new QueryRequest<TForm>(new Field(kName) == new Literal(value)));
			if (resp.IsT0 || resp.AsT1.SingleOrDefault() is var form && form is null)
				return null;
			var map = new TMap {KingdeeId = FormMeta<TForm>.Key.GetValue(form)};
			if (mapInfo.MapAttribute!.KExtKey is not null)
				map.FXiaoKeId = mapInfo.MapAttribute.KExtKey.GetValue(form) as string;
			foreach (var (prop, attr) in mapInfo.MapReferenceAttributes) {
				if (attr is null || string.IsNullOrEmpty(attr.KName))
					continue;
				prop.SetValue(map, attr.GetKMember(mapInfo.MapType).GetValue(form));
			}
			if (save)
				lock (Context) {
					Context.AddOrUpdate(map);
					Context.SaveChanges();
				}
			return map;
		}
		#nullable enable
		public TMap? GetByFXiaoKeId<TMap>(string? id) where TMap : class, IIdMap => GetByFXiaoKeId(Context.GetDbSet<TMap>()!, id);

		public static TMap? GetByFXiaoKeId<TMap>(DbSet<TMap> dbSet, string? id) where TMap : class, IIdMap {
			if (string.IsNullOrEmpty(id))
				return null;
			lock (dbSet)
				return dbSet.Find(id);
		}

		public TMap? GetByKingdeeId<TMap>(int id) where TMap : class, IIdMap => GetByKingdeeId(Context.GetDbSet<TMap>()!, id);

		public static TMap? GetByKingdeeId<TMap>(DbSet<TMap> dbSet, int id) where TMap : class, IIdMap {
			lock (dbSet)
				return dbSet.SingleOrDefault(map => id.Equals(map.KingdeeId));
		}

		public TMap? GetByMapProperty<TMap>([NotNull] string propName, object? value) where TMap : class, IIdMap => GetByMapProperty(Context.GetDbSet<TMap>()!, propName, value);

		public static TMap? GetByMapProperty<TMap>(DbSet<TMap> dbSet, [NotNull] string propName, object? value) where TMap : class, IIdMap {
			if (value is null)
				return null;
			var prop = typeof(TMap).GetMostDerivedProperty(propName);
			if (prop is null)
				throw new MemberNotFoundException(typeof(TMap), propName, MemberTypes.Property);
			lock (dbSet) {
				return dbSet.AsEnumerable()
					.SingleOrDefault(
						map => {
							object? propValue = prop.GetValue(map);
							return value.Equals(propValue);
						}
					);
			}
		}

		public TMap? GetByMapProperty<TMap, TProperty>([NotNull] string propName, TProperty? value) where TMap : class, IIdMap => GetByMapProperty(Context.GetDbSet<TMap>()!, propName, value);

		public static TMap? GetByMapProperty<TMap, TProperty>(DbSet<TMap> dbSet, [NotNull] string propName, TProperty? value) where TMap : class, IIdMap {
			if (value is null)
				return null;
			var prop = typeof(TMap).GetMostDerivedProperty(propName);
			if (prop is null)
				throw new MemberNotFoundException(typeof(TMap), propName, MemberTypes.Property);
			lock (dbSet) {
				return dbSet.AsEnumerable()
					.SingleOrDefault(
						map => {
							object? v = prop.GetValue(map);
							return v is TProperty vv && value.Equals(vv);
						}
					);
			}
		}

		public async Task<bool> HasFXiaoKeId<TMap>(string? id) where TMap : class, IIdMap, new() => await FromFXiaoKeId<TMap>(id) is not null;

		public async Task<TMap?> FromFXiaoKeId<TMap>(string? id) where TMap : class, IIdMap, new() {
			if (id is null)
				return null;
			if (GetByFXiaoKeId<TMap>(id) is { } map)
				return map;
			var mapInfo = GetMapInfo(typeof(TMap));
			var modelType = mapInfo.MapAttribute!.FModel;
			map = await (dynamic)QueryByFXiaoKeIdMethod.MakeGenericMethod(mapInfo.MapType, modelType).Invoke(this, id, true);
			if (map is not null)
				lock (Context) {
					Context.AddOrUpdate(map);
					Context.SaveChanges();
				}
			return map;
		}

		public async Task<bool> HasKingdeeId<TMap>(int id) where TMap : class, IIdMap, new() => await FromKingdeeId<TMap>(id) is not null;

		public async Task<TMap?> FromKingdeeId<TMap>(int id) where TMap : class, IIdMap, new() {
			if (GetByKingdeeId<TMap>(id) is { } map)
				return map;
			var mapInfo = GetMapInfo(typeof(TMap));
			var formType = mapInfo.MapAttribute!.KModel;
			map = await (dynamic)QueryByKingdeeIdMethod.MakeGenericMethod(mapInfo.MapType, formType).Invoke(this, id, true);
			if (map is not null)
				lock (Context) {
					Context.AddOrUpdate(map);
					Context.SaveChanges();
				}
			return map;
		}

		public async Task<bool> HasMapProperty<TMap>(string propName, object? value) where TMap : class, IIdMap, new() => await FromMapProperty<TMap>(propName, value) is not null;

		public async Task<TMap?> FromMapProperty<TMap>(string propName, object? value) where TMap : class, IIdMap, new() {
			if (value is null)
				return null;
			var map = GetByMapProperty<TMap>(propName, value);
			if (!(map?.FXiaoKeId is null || map.KingdeeId is null))
				return map;
			var mapInfo = GetMapInfo(typeof(TMap));
			var mapProp = mapInfo.GetProperty(propName);
			if (mapProp is null)
				throw new MemberNotFoundException(mapInfo.MapType, propName, MemberTypes.Property);
			if (!mapInfo.MapReferenceAttributes.TryGetValue(mapProp, out var refAttr) || refAttr is null)
				throw new AttributeNotFoundException(typeof(MapReferenceAttribute));
			if (!string.IsNullOrEmpty(refAttr.FName) && map?.FXiaoKeId is null) {
				TMap fMap = await (dynamic)QueryFXiaoKeByMapPropertyMethod.MakeGenericMethod(mapInfo.MapType, mapInfo.MapAttribute!.FModel).Invoke(this, propName, value, false);
				if (map is null)
					map = fMap;
				else if (fMap is not null) {
					map.FXiaoKeId = fMap.FXiaoKeId;
					foreach (var prop in mapInfo.Properties)
						if (prop.GetValue(fMap) is { } fValue)
							prop.SetValue(map, fValue);
				}
			}
			if (!string.IsNullOrEmpty(refAttr.KName) && (map?.KingdeeId is null || map.KingdeeId.IsDefault())) {
				TMap kMap = await (dynamic)QueryKingdeeByMapPropertyMethod.MakeGenericMethod(mapInfo.MapType, mapInfo.MapAttribute!.KModel).Invoke(this, propName, value, false);
				if (map is null)
					map = kMap;
				else if (kMap is not null) {
					map.KingdeeId = kMap.KingdeeId;
					foreach (var prop in mapInfo.Properties)
						if (prop.GetValue(kMap) is { } kValue)
							prop.SetValue(map, kValue);
				}
			}
			if (map is not null)
				lock (Context) {
					Context.AddOrUpdate(map);
					Context.SaveChanges();
				}
			return map;
		}
	}

	internal class MapInfo {
		internal MapAttribute MapAttribute;

		internal Dictionary<PropertyInfo, MapReferenceAttribute?> MapReferenceAttributes = new();

		internal Type MapType;

		internal MapInfo(Type mapType) {
			MapType = mapType;
			MapAttribute = MapType.GetCustomAttribute<MapAttribute>()!;
			foreach (var prop in MapType.GetMostDerivedProperties()) {
				if (prop.Name is nameof(IIdMap.FXiaoKeId) or nameof(IIdMap.KingdeeId))
					continue;
				MapReferenceAttributes.Add(prop, prop.GetCustomAttribute<MapReferenceAttribute>());
			}
			Properties = MapReferenceAttributes.Keys.ToArray();
		}

		internal PropertyInfo[] Properties { get; }

		internal PropertyInfo? GetProperty(string name) => MapReferenceAttributes.Keys.FirstOrDefault(p => p.Name == name);
	}
}