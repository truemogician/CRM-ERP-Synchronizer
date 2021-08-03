using System.Reflection;
using FXiaoKe.Utilities;
using Newtonsoft.Json;

namespace FXiaoKe.Models {
	[Model]
	public abstract class ModelBase {
		[JsonIgnore]
		public MemberInfo Key => GetType().GetKey();
	}
}