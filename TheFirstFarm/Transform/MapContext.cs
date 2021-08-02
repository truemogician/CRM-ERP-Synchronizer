using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Reflection;
using FXiaoKe.Models;
using Kingdee.Forms;
using Microsoft.EntityFrameworkCore;
using Shared.Exceptions;
using TheFirstFarm.Transform.Entities;
using FModelBase = FXiaoKe.Models.ModelBase;
using KModelBase = Kingdee.Forms.ModelBase;

namespace TheFirstFarm.Transform {
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

		public DbSet<ProductMap> ProductMaps { get; set; }

		public DbSet<ReturnOrderMap> ReturnOrderMap { get; set; }

		public static IEnumerable<PropertyInfo> MapInfos {
			get {
				var props = typeof(MapContext).GetProperties();
				return props.Where(p => p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>));
			}
		}

		public static IEnumerable<Type> MapTypes => MapInfos.Select(p => p.PropertyType.GetGenericArguments()[0]);

		#nullable enable
		public static Type? GetMapType(Type modelType) {
			bool isFModel = modelType.IsAssignableTo(typeof(FModelBase));
			if (!modelType.IsAssignableTo(typeof(ErpModelBase)))
				throw new TypeException(modelType, $"{modelType.Name} neither derives from {nameof(FModelBase)} nor {nameof(ErpModelBase)}");
			return MapTypes.SingleOrDefault(type => type.GetCustomAttribute<MapAttribute>() is { } attr && (isFModel ? attr.FModel == modelType : attr.KModel == modelType));
		}
		#nullable disable

		protected override void OnConfiguring(DbContextOptionsBuilder options) {
			options.UseSqlite(ConnectionString)
				.EnableSensitiveDataLogging();
		}
	}
}