using System;

namespace Shared {
	public delegate void CommonEventHandler<in T>(object sender, T args) where T : EventArgs;
}