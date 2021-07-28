using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Shared.Utilities;

namespace Kingdee.Responses {
	public abstract class ResponseBase {
		public virtual List<ValidationResult> Validate(bool recursive = true) => Utility.Validate(this, true);
	}
}