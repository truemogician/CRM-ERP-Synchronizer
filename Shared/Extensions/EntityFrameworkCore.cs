using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore.Infrastructure;

// ReSharper disable once CheckNamespace
namespace Microsoft.EntityFrameworkCore.ChangeTracking {
	public static class DbSetExtension {
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static DbContext GetContext<TEntity>(this DbSet<TEntity> dbSet) where TEntity : class => dbSet.GetService<ICurrentDbContext>().Context;

		public static DbSet<TEntity> GetDbSet<TEntity>(this DbContext ctx) where TEntity : class {
			var type = ctx.GetType();
			var members = type.GetMembers(MemberTypes.Property | MemberTypes.Field);
			var target = members.SingleOrDefault(member => member.GetValueType().IsAssignableTo(typeof(DbSet<TEntity>)));
			return target?.GetValue(ctx) as DbSet<TEntity>;
		}

		private static EntityEntry<T> AddOrUpdate<T>(DbContext ctx, DbSet<T> dbSet, T entity) where T : class {
			string keyName = ctx.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties.Select(p => p.Name).Single();
			var idProp = entity.GetType().GetMostDerivedProperty(keyName);
			object id = idProp.GetValue(entity);
			var existingEntity = dbSet.AsEnumerable().FirstOrDefault(x => idProp.GetValue(x)!.Equals(id));
			if (existingEntity is null)
				return dbSet.Add(entity);
			var entry = ctx.Entry(existingEntity);
			entry.CurrentValues.SetValues(entity);
			return entry;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static EntityEntry<T> AddOrUpdate<T>(this DbSet<T> dbSet, T entity) where T : class => AddOrUpdate(dbSet.GetContext(), dbSet, entity);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static EntityEntry<T> AddOrUpdate<T>(this DbContext ctx, T entity) where T : class => AddOrUpdate(ctx, ctx.GetDbSet<T>(), entity);

		private static void AddOrUpdateRange<T>(DbContext ctx, DbSet<T> dbSet, IEnumerable<T> entities) where T : class {
			string keyName = ctx.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties.Select(p => p.Name).Single();
			var idProp = typeof(T).GetMostDerivedProperty(keyName);
			foreach (var entity in entities) {
				object id = idProp.GetValue(entity);
				var existingEntity = dbSet.AsEnumerable().FirstOrDefault(x => idProp.GetValue(x)!.Equals(id));
				if (existingEntity is null)
					dbSet.Add(entity);
				else
					ctx.Entry(existingEntity).CurrentValues.SetValues(entity);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void AddOrUpdateRange<T>(this DbSet<T> dbSet, IEnumerable<T> entities) where T : class => AddOrUpdateRange(dbSet.GetContext(), dbSet, entities);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void AddOrUpdateRange<T>(this DbContext ctx, IEnumerable<T> entities) where T : class => AddOrUpdateRange(ctx, ctx.GetDbSet<T>(), entities);
	}
}