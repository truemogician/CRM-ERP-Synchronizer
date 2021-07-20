using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Kingdee {
	public class AsyncResult<T> {
		internal AsyncResult() { }

		public bool Successful { get; internal set; }

		public T ReturnValue { get; set; }

		public ServiceException Exception { get; internal set; }

		internal static AsyncResult<T> CreateUnsuccess(ServiceException ex)
			=> new() {
				Successful = false,
				ReturnValue = default,
				Exception = ex
			};

		internal static AsyncResult<T> CreateSuccess(T result)
			=> new() {
				Successful = true,
				ReturnValue = result,
				Exception = null
			};

		internal void ThrowEx() {
			if (Exception != null && !Exception.Handled)
				throw Exception;
		}

		internal AsyncResult<List<To>> ToList<To>() {
			var asyncResult = new AsyncResult<List<To>> {
				Successful = Successful,
				Exception = Exception
			};
			asyncResult.ReturnValue = !(typeof(T) == typeof(string)) ? (List<To>)(object)ReturnValue : JsonArray.Parse(ReturnValue.ToString()).ConvertTo<To>().ToList();
			return asyncResult;
		}

		internal AsyncResult<To> Cast<To>() {
			var asyncResult = new AsyncResult<To> {
				Successful = Successful,
				Exception = Exception
			};
			if (typeof(T) == typeof(string)) {
				string json = ReturnValue == null ? "" : ReturnValue.ToString();
				asyncResult.ReturnValue = !string.IsNullOrEmpty(json) ? JsonConvert.DeserializeObject<To>(json) : default;
			}
			else
				asyncResult.ReturnValue = (To)(object)ReturnValue;
			return asyncResult;
		}
	}
}