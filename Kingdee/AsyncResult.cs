using System.Collections.Generic;
using System.Linq;
using Kingdee.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Kingdee {
	public class AsyncResult<T> {
		internal AsyncResult() { }

		public bool Successful { get; internal set; }

		public T ReturnValue { get; set; }

		public ServiceException Exception { get; internal set; }

		internal static AsyncResult<T> CreateFailure(ServiceException ex)
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
			if (Exception is {Handled: false})
				throw Exception;
		}

		internal AsyncResult<List<TResult>> ToList<TResult>() {
			var asyncResult = new AsyncResult<List<TResult>> {
				Successful = Successful,
				Exception = Exception,
				ReturnValue = typeof(T) != typeof(string)
					? (List<TResult>)(object)ReturnValue
					: JArray.Parse(ReturnValue.ToString()!)
						.Select(token => token is JObject jObj ? jObj.ToObject<TResult>() : token.Value<TResult>())
						.ToList()
			};
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