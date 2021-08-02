using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace FXiaoKe.Responses {
	public class CreationResponse : BasicResponse {
		/// <summary>
		///     添加成功的数据Id
		/// </summary>
		[JsonProperty("dataId")]
		[Required]
		public string DataId { get; set; }
	}
}