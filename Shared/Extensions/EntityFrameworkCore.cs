using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Infrastructure;

// ReSharper disable once CheckNamespace
namespace Microsoft.EntityFrameworkCore.ChangeTracking {
	public static class DbSetExtension {
		public static DbContext GetContext<TEntity>(this DbSet<TEntity> dbSet) where TEntity : class => dbSet.GetService<ICurrentDbContext>().Context;

		public static EntityEntry<T> AddOrUpdate<T>(this DbSet<T> dbSet, T entity) where T : class {
			string keyName = dbSet.GetContext().Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties.Select(p => p.Name).Single();
			var idProp = entity.GetType().GetMostDerivedProperty(keyName);
			object id = idProp.GetValue(entity);
			return dbSet.AsEnumerable().Any(x => idProp.GetValue(x)!.Equals(id)) ? dbSet.Update(entity) : dbSet.Add(entity);
		}

		public static void AddOrUpdateRange<T>(this DbSet<T> dbSet, IEnumerable<T> entities) where T : class {
			string keyName = dbSet.GetContext().Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties.Select(p => p.Name).Single();
			var idProp = typeof(T).GetMostDerivedProperty(keyName);
			foreach (var entity in entities) {
				object id = idProp.GetValue(entity);
				if (dbSet.AsEnumerable().Any(x => idProp.GetValue(x)!.Equals(id)))
					dbSet.Update(entity);
				else
					dbSet.Add(entity);
			}
		}
	}
}