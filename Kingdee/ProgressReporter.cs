using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;

namespace Kingdee {
	internal class ProgressReporter {
		private static readonly ConcurrentDictionary<string, ProgressReporter> Reporters = new();

		private readonly ApiClient _client;

		private readonly bool _getLastAuto;

		private readonly LockOneTime _locker = new();

		private readonly ProgressChangedHandler _progressCallback;

		private readonly string _requestId;

		private readonly Timer _timer;

		private ProgressReporter(
			ApiClient client,
			ApiRequest request,
			ProgressChangedHandler progressCallback,
			int interval
		) {
			string requestId = request.RequestId;
			if (interval < 0 || interval > 30)
				interval = 5;
			int num = 1000 * interval;
			_timer = new Timer(TimerCallback, null, num, num);
			_requestId = requestId;
			_progressCallback = progressCallback;
			_client = client;
			_getLastAuto = request.AutoGetLastProgress;
		}

		internal static ProgressReporter Create(
			ApiClient client,
			ApiRequest request,
			ProgressChangedHandler progressCallback,
			int interval
		) {
			string requestId = request.RequestId;
			Reporters[requestId] = new ProgressReporter(client, request, progressCallback, interval);
			return Reporters[requestId];
		}

		internal static void Finish(string rid) {
			if (!Reporters.ContainsKey(rid))
				return;
			Reporters[rid].DoLast();
			Reporters.TryRemove(rid, out var _);
		}

		private void DoLast() {
			if (!_getLastAuto)
				return;
			_timer.Change(0, 0);
		}

		internal void TimerCallback(object state)
			=> _locker.Run(
				() => {
					var progress = GetProgress();
					if (progress == null || progress.Length <= 0)
						return;
					_progressCallback(progress);
				}
			);

		internal ProgressInfo[] GetProgress() {
			try {
				string json = _client.Call(_client.CreateProgressQuery(_requestId));
				if (!string.IsNullOrEmpty(json))
					return JsonArray.Parse(json).ConvertTo<ProgressInfo>().ToArray();
			}
			catch (Exception ex) {
				Console.Write(ex.Message);
			}
			return null;
		}
	}

	public class ProgressInfo {
		public byte Percentage { get; set; }

		public string Message { get; set; }
	}
}