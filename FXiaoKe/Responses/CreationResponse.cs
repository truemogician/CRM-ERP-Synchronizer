using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace FXiaoKe.Responses {
	public class CreationResponse : BasicResponseWithDescription {
		/// <summary>
		///     添加成功的数据Id
		/// </summary>
		[JsonProperty("dataId")]
		[Required]
		public string DataId { get; set; }
	}
}