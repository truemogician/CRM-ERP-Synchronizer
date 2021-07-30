using Kingdee.Forms;
using Newtonsoft.Json;
using Shared.Serialization;

namespace Kingdee.Requests {
	public class SaveRequest<T> : CreationRequest<T> where T : FormBase {
		public SaveRequest() { }
		public SaveRequest(T data) : base(data) { }

		/// <summary>
		///     是否验证所有的基础资料有效性，默认为false
		/// </summary>
		[JsonProperty("IsVerifyBaseDataField")]
		[JsonConverter(typeof(BoolConverter))]
		public bool ValidateEffectiveness { get; set; } = false;
	}
}