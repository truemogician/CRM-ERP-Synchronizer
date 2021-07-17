using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Kingdee.Requests {
	public abstract class RequestBase {
		public virtual List<ValidationResult> Validate() {
			var valContext = new ValidationContext(this, null, null);
			var result = new List<ValidationResult>();
			Validator.TryValidateObject(this, valContext, result, true);
			return result;
		}
	}
}