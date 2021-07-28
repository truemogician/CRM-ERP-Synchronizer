using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using FXiaoKe.Utilities;
using Newtonsoft.Json;
using Shared.Utilities;

namespace FXiaoKe.Models {
	[Model]
	public abstract class ModelBase {
		[JsonProperty("_id")]
		[Generated]
		public virtual string DataId { get; set; }

		[JsonIgnore]
		public IEnumerable<ModelBase> SubModels {
			get {
				var infos = GetType().GetSubModelInfos(false);
				return infos.SelectSingleOrMany<MemberInfo, ModelBase>(info => info.GetValue(this)).ToList();
			}
		}

		[JsonIgnore]
		public MemberInfo KeyInfo => GetType().GetMemberWithAttribute<KeyAttribute>();

		public virtual List<ValidationResult> Validate(bool recursive = true) => Utility.Validate(this, recursive);
	}
}