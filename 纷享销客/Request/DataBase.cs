using System.ComponentModel.DataAnnotations;
using FXiaoKe.Models;
using FXiaoKe.Utilities;
using Newtonsoft.Json;

namespace FXiaoKe.Request {
	public class DataBase<T> where T : ModelBase {
		public DataBase() => ObjectName = typeof(T).GetModelName();

		/// <summary>
		///     对象的api_name
		/// </summary>
		[JsonProperty("dataObjectApiName")]
		[Required]
		public virtual string ObjectName { get; }
	}
}