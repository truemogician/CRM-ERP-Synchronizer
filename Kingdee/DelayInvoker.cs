using System;
using System.Threading;

namespace Kingdee {
	internal class DelayInvoker<T> {
		private readonly Action<T> _action;
		private readonly T _state;
		private readonly int _timeout;

		public DelayInvoker(Action<T> action, T state, int timeout) {
			_action = action;
			_state = state;
			_timeout = timeout;
		}

		internal void Invoke() {
			var timer = new Timer(s => _action((T)s), _state, _timeout, -1);
		}
	}
}