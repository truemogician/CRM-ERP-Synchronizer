using System.Collections.Generic;
using System.Linq;
using Kingdee.Forms;
using Newtonsoft.Json;

namespace Kingdee.Requests {
	public class BatchSaveRequest<T> : CreationRequest<List<T>> where T : ErpModelBase {
		public BatchSaveRequest() { }

		public BatchSaveRequest(IEnumerable<T> data) : base(data.ToList()) { }

		/// <summary>
		///     服务端开启的线程数
		/// </summary>
		[JsonProperty("BatchCount")]
		public int ThreadCount { get; set; }
	}
}