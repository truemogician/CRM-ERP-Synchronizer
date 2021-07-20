using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace FXiaoKe.Request {
	public abstract class CrmRequestBase<T> : RequestWithAdvancedAuth {
		[JsonIgnore]
		protected T Content;

		protected CrmRequestBase() { }
		protected CrmRequestBase(T data) => Content = data;
	}

	public class CrmRequest<T> : CrmRequestBase<T> {
		public CrmRequest() { }
		public CrmRequest(T data) : base(data) { }

		/// <summary>
		///     数据Map
		/// </summary>
		[JsonProperty("data")]
		public T Data {
			get => Content;
			set => Content = value;
		}
	}

	public class CovariantCrmRequest<T> : CrmRequestBase<T> {
		public CovariantCrmRequest() { }
		public CovariantCrmRequest(T data) : base(data) { }

		/// <summary>
		///     数据Map
		/// </summary>
		[JsonProperty("data")]
		public virtual T Data => Content;
	}
}