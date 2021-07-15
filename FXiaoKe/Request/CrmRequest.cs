using Newtonsoft.Json;

namespace FXiaoKe.Request {
	public class CrmRequest<T> : RequestWithAdvancedAuth {
		public CrmRequest() { }
		public CrmRequest(Client client) : base(client) { }
		public CrmRequest(T data, Client client) : this(client) => Data = data;

		/// <summary>
		///     数据Map
		/// </summary>
		[JsonProperty("data")]
		public virtual T Data { get; set; }
	}
}