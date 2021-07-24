using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using DataAnnotationsValidator;
using FXiaoKe.Utilities;
using Newtonsoft.Json;
using Shared.Utilities;
using OneOf;

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

		public virtual List<ValidationResult> Validate() => Utility.Validate(this);
	}
}