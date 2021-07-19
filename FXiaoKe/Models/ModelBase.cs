using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using FXiaoKe.Utilities;
using Shared.Utilities;

namespace FXiaoKe.Models {
	[Model]
	public abstract class ModelBase {
		public virtual List<ValidationResult> Validate() => Utility.Validate(this);

		public List<ModelBase> CascadeSubModels
			=> GetType()
				.GetCascadeSubModels()
				.Select(
					member => (member is FieldInfo field ? field.GetValue(this) : (member as PropertyInfo)!.GetValue(this)) as ModelBase
				)
				.ToList();
	}
}