using System;

namespace Kingdee.Requests {
	[AttributeUsage(AttributeTargets.Class)]
	public class RequestAttribute : Attribute {
		public RequestAttribute(string serviceName) => ServiceName = serviceName;

		public string ServiceName { get; }
	}
}