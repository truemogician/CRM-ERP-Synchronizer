using System;
using System.ComponentModel.DataAnnotations;
using FXiaoKe.Models;
using FXiaoKe.Utilities;
using Newtonsoft.Json;

namespace FXiaoKe.Requests {
	public abstract class DataBase<T> : DataBase where T : CrmModelBase {
		protected DataBase() : base(typeof(T)) { }
	}

	public abstract class DataBase {
		protected DataBase() { }

		protected DataBase(Type type) => ObjectName = type.GetModelName();

		/// <summary>
		///     对象的api_name
		/// </summary>
		[JsonProperty("dataObjectApiName")]
		[Required]
		public string ObjectName { get; init; }
	}
}