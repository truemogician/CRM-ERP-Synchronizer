using System;
using System.Threading;

namespace Kingdee {
	internal class LockOneTime {
		private const int Max = 1;
		private readonly SemaphoreSlim _sema = new(Max);

		internal void Run(Action action) {
			if (_sema.CurrentCount == 0)
				return;
			_sema.Wait();
			try {
				action();
			}
			finally {
				_sema.Release();
			}
		}
	}
}