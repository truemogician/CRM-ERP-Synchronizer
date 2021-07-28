using System;
using System.Linq;
using System.Threading.Tasks;
using FXiaoKe.Models;
using FXiaoKe.Requests;
using Kingdee.Forms;
using Microsoft.EntityFrameworkCore;
using TheFirstFarm.Transform.Models;
using FClient = FXiaoKe.Client;
using KClient = Kingdee.Client;
using System.Reflection;
using System.Runtime.CompilerServices;
using Kingdee.Requests;
using Shared.Exceptions;
using FModel = TheFirstFarm.Models.FXiaoKe;
using KModel = TheFirstFarm.Models.Kingdee;

namespace TheFirstFarm.Transform {
	public class IdTransformer {
		public IdTransformer(FClient fClient, KClient kClient, MapContext ctx = null) {
			FClient = fClient;
			KClient = kClient;
			Context = ctx ?? new MapContext();
		}

		public FClient FClient { get; set; }

		public KClient KClient { get; set; }

		public MapContext Context { get; set; }

		public Task<string> ToFXiaoKeId<T>(string kingdeeId) where T : ModelBase {
			if (string.IsNullOrEmpty(kingdeeId))
				return Task.FromResult<string>(null);
			var type = typeof(T);
			if (type == typeof(Staff))
				return GetOrQueryFXiaoKeId<StaffMap, T>(Context.StaffMaps, kingdeeId, null);
			if (type == typeof(FModel.Customer))
				return GetOrQueryFXiaoKeId<CustomerMap, T>(Context.CustomerMaps, kingdeeId, nameof(FModel.Customer.KingdeeId));
			throw new NotImplementedException();
		}

		public Task<string> ToKingdeeId<T>(string fXiaoKeId) where T : FormBase {
			if (string.IsNullOrEmpty(fXiaoKeId))
				return Task.FromResult<string>(null);
			throw new NotImplementedException();
		}

		private async Task<string> GetOrQueryFXiaoKeId<TMap, TModel>(DbSet<TMap> dbSet, string kingdeeId, string kingdeeIdName) where TMap : IdMap<string, string> where TModel : ModelBase {
			string fXiaoKeId;
			lock (dbSet)
				fXiaoKeId = GetFXiaoKeId(dbSet, kingdeeId);
			if (!string.IsNullOrEmpty(fXiaoKeId))
				return fXiaoKeId;
			if (string.IsNullOrEmpty(kingdeeIdName))
				throw new NotFoundException(kingdeeId, $"No mapping from Kingdee id {kingdeeId} found");
			var result = await FClient.QueryByCondition<TModel>(ModelFilter<TModel>.Equal(kingdeeIdName, kingdeeId));
			if (result is null || result.Count == 0)
				return null;
			var propInfo = typeof(TModel).GetMember(kingdeeIdName, MemberTypes.Property | MemberTypes.Field);
			fXiaoKeId = propInfo.GetValue(result) as string;
			dbSet.Add((typeof(TMap).Construct(fXiaoKeId, kingdeeId) as TMap)!);
			await Context.SaveChangesAsync();
			return fXiaoKeId;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static string GetFXiaoKeId<TMap>(DbSet<TMap> dbSet, string kingdeeId) where TMap : IdMap<string, string> => dbSet.ToArray().SingleOrDefault(m => m.KingdeeId == kingdeeId)?.FXiaoKeId;

		private async Task<string> GetOrQueryKingdeeId<TMap, TForm>(DbSet<TMap> dbSet, string fXiaoKeId, string fXiaoKeIdName) where TMap : IdMap<string, string> where TForm : FormBase {
			string kingdeeId = GetKingdeeId(dbSet, fXiaoKeId);
			if (!string.IsNullOrEmpty(kingdeeId))
				return kingdeeId;
			if (dbSet.SingleOrDefault(m => m.FXiaoKeId == fXiaoKeId) is { } map)
				return map.KingdeeId;
			if (string.IsNullOrEmpty(fXiaoKeIdName))
				throw new NotFoundException(fXiaoKeId, $"No mapping from FXiaoKe id {fXiaoKeId} found");
			var resp = await KClient.QueryAsync(new QueryRequest<TForm>(new Field<TForm>(fXiaoKeIdName) == fXiaoKeId));
			if (!resp.TryPickT1(out var result, out _) || result is null || result.Count == 0)
				return null;
			var propInfo = typeof(TForm).GetMember(fXiaoKeIdName, MemberTypes.Property | MemberTypes.Field);
			kingdeeId = propInfo.GetValue(result) as string;
			dbSet.Add((typeof(TMap).Construct(fXiaoKeId, kingdeeId) as TMap)!);
			await Context.SaveChangesAsync();
			return kingdeeId;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static string GetKingdeeId<TMap>(DbSet<TMap> dbSet, string fXiaoKeId) where TMap : IdMap<string, string> => dbSet.ToArray().SingleOrDefault(m => m.FXiaoKeId == fXiaoKeId)?.KingdeeId;
	}
}