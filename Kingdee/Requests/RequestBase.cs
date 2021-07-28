using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Shared.Utilities;

namespace Kingdee.Requests {
	public abstract class RequestBase {
		public virtual List<ValidationResult> Validate(bool recursive = true) => Utility.Validate(this, recursive);
	}
}