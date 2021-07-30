using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using FXiaoKe.Utilities;
using Newtonsoft.Json;
using Shared.Utilities;

namespace FXiaoKe.Models {
	[Model]
	public abstract class ModelBase {
		[JsonIgnore]
		public MemberInfo Key => GetType().GetKey();

		public virtual List<ValidationResult> Validate(bool recursive = true) => Utility.Validate(this, recursive);
	}
}