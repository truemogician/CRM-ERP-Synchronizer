using Kingdee.Exceptions;

namespace Kingdee {
	public delegate void FailCallbackHandler(ServiceException ex);

	public delegate void ProgressChangedHandler(ProgressInfo[] progresses);
}