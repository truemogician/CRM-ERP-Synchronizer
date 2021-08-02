using System;
using Kingdee.Requests;
using Kingdee.Requests.Query;
using Newtonsoft.Json;

namespace Kingdee.Forms {
	public abstract class ModelBase {
		[JsonProperty("FCreatorId")]
		public int? CreatorId { get; set; }

		[JsonProperty("FCreateDate")]
		public DateTime? CreationTime { get; set; }

		[JsonProperty("FModifierId")]
		public int? ModifierId { get; set; }

		[JsonProperty("FModifyDate")]
		public DateTime? ModificationTime { get; set; }

		public object this[Field field] {
			get => field.GetValue(this);
			set => field.SetValue(this, value);
		}
	}
}