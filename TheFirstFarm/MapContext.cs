using System.Data.Common;
using System.IO;
using Microsoft.EntityFrameworkCore;
using TheFirstFarm.Models.Database;

namespace TheFirstFarm {
	public class MapContext : DbContext {
		public MapContext() {
			var builder = new DbConnectionStringBuilder();
			var folder = @"D:\Code\Work\企荫信息兼职\Synchronizer\TheFirstFarm\SQLite";
			builder.Add("Data Source", Path.Combine(folder, "map.db"));
			ConnectionString = builder.ToString();
		}

		public string ConnectionString { get; }

		public DbSet<CustomerMap> CustomerMaps { get; set; }

		public DbSet<StaffMap> StaffMaps { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite(ConnectionString);
	}
}