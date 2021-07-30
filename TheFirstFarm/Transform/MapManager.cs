using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FXiaoKe.Models;
using FXiaoKe.Requests;
using Kingdee.Forms;
using Kingdee.Requests;
using Kingdee.Requests.Query;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Shared.Exceptions;
using TheFirstFarm.Transform.Entities;
using FClient = FXiaoKe.Client;
using KClient = Kingdee.Client;
using FModel = TheFirstFarm.Models.FXiaoKe;
using KModel = TheFirstFarm.Models.Kingdee;

namespace TheFirstFarm.Transform {
	public class MapManager {
		private static readonly MethodInfo QueryByFXiaoKeIdMethod = typeof(MapManager).GetMethod(nameof(QueryByFXiaoKeId));

		private static readonly MethodInfo QueryByKingdeeIdMethod = typeof(MapManager).GetMethod(nameof(QueryByKingdeeId));

		private static readonly MethodInfo QueryFXiaoKeByMapPropertyMethod = typeof(MapManager).GetMethod(nameof(QueryFXiaoKeByMapProperty));

		private static readonly MethodInfo QueryKingdeeByMapPropertyMethod = typeof(MapManager).GetMethod(nameof(QueryKingdeeByMapProperty));

		public MapManager(FClient fClient, KClient kClient, MapContext ctx = null) {
			FClient = fClient;
			KClient = kClient;
			Context = ctx ?? new MapContext();
		}

		public FClient FClient { get; set; }

		public KClient KClient { get; set; }

		public MapContext Context { get; set; }

		private async Task<TMap> QueryByFXiaoKeId<TMap, TModel>(string id) where TMap : IdMap, new() where TModel : CrmModelBase {
			var model = await FClient.QueryById<TModel>(id);
			if (model is null)
				return null;
			var map = new TMap {FXiaoKeId = id};
			var mapAttr = typeof(TMap).GetCustomAttribute<MapAttribute>();
			if (mapAttr!.FExtKey is not null)
				map.KingdeeId = mapAttr.FExtKey.GetValue(model) as int?;
			foreach (var prop in typeof(TMap).GetProperties()) {
				var mapRefAttr = prop.GetCustomAttribute<MapReferenceAttribute>();
				if (mapRefAttr is null || string.IsNullOrEmpty(mapRefAttr.FName))
					continue;
				prop.SetValue(map, mapRefAttr.GetFMember(typeof(TMap)).GetValue(model));
			}
			Context.AddOrUpdate(map);
			await Context.SaveChangesAsync();
			return map;
		}

		private async Task<TMap> QueryByKingdeeId<TMap, TForm>(int id) where TMap : IdMap, new() where TForm : FormBase {
			var keyInfo = FormMeta<TForm>.Key;
			var resp = await KClient.QueryAsync(new QueryRequest<TForm>((Field)keyInfo == id));
			if (resp.IsT0)
				return null;
			var form = resp.AsT1.SingleOrDefault();
			if (form is null)
				return null;
			var map = new TMap {KingdeeId = id};
			var mapAttr = typeof(TMap).GetCustomAttribute<MapAttribute>();
			if (mapAttr!.KExtKey is not null)
				map.FXiaoKeId = mapAttr.KExtKey.GetValue(form) as string;
			foreach (var prop in typeof(TMap).GetProperties()) {
				var mapRefAttr = prop.GetCustomAttribute<MapReferenceAttribute>();
				if (mapRefAttr is null || string.IsNullOrEmpty(mapRefAttr.KName))
					continue;
				prop.SetValue(map, mapRefAttr.GetKMember(typeof(TMap)).GetValue(form));
			}
			Context.AddOrUpdate(map);
			await Context.SaveChangesAsync();
			return map;
		}

		private async Task<TMap> QueryFXiaoKeByMapProperty<TMap, TModel>(string propName, object value) where TMap : class, IIdMap, new() where TModel : CrmModelBase {
			var property = typeof(TMap).GetMostDerivedProperty(propName);
			if (property is null)
				throw new MemberNotFoundException(typeof(TMap), propName, MemberTypes.Property);
			var mapRefAttr = property.GetCustomAttribute<MapReferenceAttribute>();
			string fName = mapRefAttr?.GetFMember<TMap>()?.Name;
			if (string.IsNullOrEmpty(fName))
				throw new Exception();//ToDo: better exception
			var model = (await FClient.QueryByCondition(new[] {ModelFilter<TModel>.Equal(fName, value)}, false))?.SingleOrDefault();
			if (model is null)
				return null;
			var map = new TMap {FXiaoKeId = model.DataId};
			var mapAttr = typeof(TMap).GetCustomAttribute<MapAttribute>();
			if (mapAttr!.FExtKey is not null)
				map.KingdeeId = mapAttr.FExtKey.GetValue(model);
			foreach (var prop in typeof(TMap).GetProperties()) {
				mapRefAttr = prop.GetCustomAttribute<MapReferenceAttribute>();
				if (mapRefAttr is null || string.IsNullOrEmpty(mapRefAttr.FName))
					continue;
				prop.SetValue(map, mapRefAttr.GetFMember(typeof(TMap)).GetValue(model));
			}
			Context.AddOrUpdate(map);
			await Context.SaveChangesAsync();
			return map;
		}

		private async Task<TMap> QueryKingdeeByMapProperty<TMap, TForm>(string propName, Literal value) where TMap : class, IIdMap, new() where TForm : FormBase {
			var property = typeof(TMap).GetMostDerivedProperty(propName);
			if (property is null)
				throw new MemberNotFoundException(typeof(TMap), propName, MemberTypes.Property);
			var mapRefAttr = property.GetCustomAttribute<MapReferenceAttribute>();
			string kName = mapRefAttr?.GetKMember<TMap>()?.Name;
			if (string.IsNullOrEmpty(kName))
				throw new Exception();//ToDo: better exception
			var resp = await KClient.QueryAsync(new QueryRequest<TForm>(new Field(kName) == value));
			if (resp.IsT0 || resp.AsT1.SingleOrDefault() is var form && form is null)
				return null;
			var map = new TMap {KingdeeId = FormMeta<TForm>.Key.GetValue(form)};
			var mapAttr = typeof(TMap).GetCustomAttribute<MapAttribute>();
			if (mapAttr!.KExtKey is not null)
				map.FXiaoKeId = mapAttr.KExtKey.GetValue(form);
			foreach (var prop in typeof(TMap).GetProperties()) {
				mapRefAttr = prop.GetCustomAttribute<MapReferenceAttribute>();
				if (mapRefAttr is null || string.IsNullOrEmpty(mapRefAttr.KName))
					continue;
				prop.SetValue(map, mapRefAttr.GetKMember(typeof(TMap)).GetValue(form));
			}
			Context.AddOrUpdate(map);
			await Context.SaveChangesAsync();
			return map;
		}

		public T GetByFXiaoKeId<T>(string id) where T : IdMap => GetByFXiaoKeId(Context.GetDbSet<T>(), id);

		public static T GetByFXiaoKeId<T>(DbSet<T> dbSet, string id) where T : IdMap {
			if (string.IsNullOrEmpty(id))
				return null;
			lock (dbSet)
				return dbSet.Find(id);
		}

		public T GetByKingdeeId<T>(int id) where T : IdMap => GetByKingdeeId(Context.GetDbSet<T>(), id);

		public static T GetByKingdeeId<T>(DbSet<T> dbSet, int id) where T : IdMap {
			lock (dbSet)
				return dbSet.SingleOrDefault(map => map.KingdeeId == id);
		}

		public T GetByMapProperty<T>(string propName, object value) where T : class, IIdMap => GetByMapProperty(Context.GetDbSet<T>(), propName, value);

		public static T GetByMapProperty<T>(DbSet<T> dbSet, string propName, object value) where T : class, IIdMap {
			var prop = typeof(T).GetMostDerivedProperty(propName);
			if (prop is null)
				throw new MemberNotFoundException(typeof(T), propName, MemberTypes.Property);
			lock (dbSet)
				return dbSet.SingleOrDefault(map => prop.GetValue(map).Equals(value));
		}

		public async Task<bool> HasFXiaoKeId<TMap>(string id) where TMap : IdMap, new() => await FromFXiaoKeId<TMap>(id) is not null;

		public async Task<TMap> FromFXiaoKeId<TMap>(string id) where TMap : IdMap, new() {
			if (GetByFXiaoKeId<TMap>(id) is { } map)
				return map;
			var modelType = typeof(TMap).GetCustomAttribute<MapAttribute>()!.FModel;
			return await (dynamic)QueryByFXiaoKeIdMethod.MakeGenericMethod(typeof(TMap), modelType).Invoke(this, id);
		}

		public async Task<bool> HasKingdeeId<TMap>(int id) where TMap : IdMap, new() => await FromKingdeeId<TMap>(id) is not null;

		public async Task<TMap> FromKingdeeId<TMap>(int id) where TMap : IdMap, new() {
			if (GetByKingdeeId<TMap>(id) is { } map)
				return map;
			var formType = typeof(TMap).GetCustomAttribute<MapAttribute>()!.KModel;
			return await (dynamic)QueryByKingdeeIdMethod.MakeGenericMethod(typeof(TMap), formType).Invoke(this, id);
		}

		public async Task<TMap> FromMapPropertyF<TMap>(string propName, object value) where TMap : class, IIdMap, new() {
			if (GetByMapProperty<TMap>(propName, value) is { } map)
				return map;
			var modelType = typeof(TMap).GetCustomAttribute<MapAttribute>()!.FModel;
			return await (dynamic)QueryFXiaoKeByMapPropertyMethod.MakeGenericMethod(typeof(TMap), modelType).Invoke(this, propName, value);
		}

		public async Task<TMap> FromMapPropertyK<TMap>(string propName, Literal value) where TMap : class, IIdMap, new() {
			if (GetByMapProperty<TMap>(propName, value) is { } map)
				return map;
			var formType = typeof(TMap).GetCustomAttribute<MapAttribute>()!.KModel;
			return await (dynamic)QueryKingdeeByMapPropertyMethod.MakeGenericMethod(typeof(TMap), formType).Invoke(this, propName, value);
		}
	}
}