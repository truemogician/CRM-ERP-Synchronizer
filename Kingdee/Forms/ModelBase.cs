using Kingdee.Requests.Query;

namespace Kingdee.Forms {
	public abstract class ModelBase {
		public object this[Field field] {
			get => field.GetValue(this);
			set => field.SetValue(this, value);
		}
	}
}