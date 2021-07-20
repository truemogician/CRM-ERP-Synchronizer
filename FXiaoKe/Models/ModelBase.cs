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
		public List<ModelBase> CascadeSubModels
			=> GetType()
				.GetCascadeSubModels()
				.Select(
					member => (member is FieldInfo field ? field.GetValue(this) : (member as PropertyInfo)!.GetValue(this)) as ModelBase
				)
				.ToList();

		public virtual List<ValidationResult> Validate() => Utility.Validate(this);
	}
}