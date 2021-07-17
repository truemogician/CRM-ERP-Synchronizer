﻿using System;
using System.Net;
using System.Text;
using Kingdee.Utilities;
using Newtonsoft.Json.Linq;

namespace Kingdee {
	public class ApiClient {
		private readonly CookieContainer _cookiesContainer;
		private readonly FailCallbackHandler _defaultFailCallback;
		private readonly int _defaultTimeout = 300;
		private readonly Encoding _encoder;
		private readonly HttpClient _httpClient;
		private readonly string _serverUrl;

		public ApiClient(string serverUrl) {
			_encoder = new UTF8Encoding();
			_serverUrl = serverUrl;
			_cookiesContainer = new CookieContainer();
			_httpClient = new HttpClient();
		}

		public ApiClient(string serverUrl, int timeout)
			: this(serverUrl)
			=> _defaultTimeout = timeout;

		public ApiClient(string serverUrl, FailCallbackHandler defaultonFail)
			: this(serverUrl)
			=> _defaultFailCallback = defaultonFail;

		public ApiClient(string serverUrl, FailCallbackHandler defaultonFail, int timeout)
			: this(serverUrl, defaultonFail)
			=> _defaultTimeout = timeout;

		public ApiRequest CreateAsyncRequest(string serviceName, object[] parameters = null) => new ApiServiceRequest(_serverUrl, true, _encoder, _cookiesContainer, serviceName, parameters);

		public ApiRequest CreateRequest(string serviceName, object[] parameters = null) => new ApiServiceRequest(_serverUrl, false, _encoder, _cookiesContainer, serviceName, parameters);

		public ApiRequest CreateProgressQuery(string reqId) => new ApiProgressRequest(_serverUrl, false, _encoder, _cookiesContainer, reqId);

		public bool Logout() => Execute<bool>("Kingdee.BOS.WebApi.ServicesStub.AuthService.Logout", null);

		public bool Login(string dbId, string userName, string password, int lcid) => JObject.Parse(Execute<string>("Kingdee.BOS.WebApi.ServicesStub.AuthService.ValidateUserEnDeCode", new object[] {dbId, EnDecode.Encode(userName), EnDecode.Encode(password), lcid}))["LoginResultType"]?.Value<int>() == 1;

		public string ValidateLogin(string dbId, string userName, string password, int lcid) => Execute<string>("Kingdee.BOS.WebApi.ServicesStub.AuthService.ValidateUserEnDeCode", new object[] {dbId, EnDecode.Encode(userName), EnDecode.Encode(password), lcid});

		public string LoginByAppSecret(
			string dbId,
			string userName,
			string appId,
			string appSecret,
			int lcid
		) {
			return Execute<string>("Kingdee.BOS.WebApi.ServicesStub.AuthService.LoginByAppSecret", new object[] {dbId, userName, appId, appSecret, lcid});
		}

		public string LoginBySign(
			string dbId,
			string userName,
			string appId,
			long timestamp,
			string sign,
			int lcid
		) {
			return Execute<string>("Kingdee.BOS.WebApi.ServicesStub.AuthService.LoginBySign", new object[] {dbId, userName, appId, timestamp, sign, lcid});
		}

		public string LoginBySimplePassport(string passportForBase64, int lcid = 2052) => Execute<string>("Kingdee.BOS.WebApi.ServicesStub.AuthService.LoginBySimplePassport", new object[] {passportForBase64, lcid});

		public string LoginByMobileCard(
			string passportForBase64,
			string customizationParameter,
			int lcid = 2052
		) {
			return Execute<string>("Kingdee.BOS.WebApi.ServicesStub.AuthService.LoginByMobileCard", new object[] {passportForBase64, customizationParameter, lcid});
		}

		public string LoginByRsaAuth(string json) => Execute<string>("Kingdee.BOS.WebApi.ServicesStub.AuthService.LoginByRSAAuth", new object[] {json});

		public T Execute<T>(string serviceName, params object[] parameters) => Execute<T>(serviceName, parameters, timeout: _defaultTimeout);

		public T Execute<T>(
			string serviceName,
			object[] parameters = null,
			FailCallbackHandler onFail = null,
			int timeout = 100
		) {
			var request = CreateRequest(serviceName, parameters);
			request.HttpRequest.Timeout = timeout * 1000;
			return Call<T>(request, onFail);
		}

		public ApiRequest ExecuteAsync<T>(
			string serviceName,
			Action<T> successCallback,
			object[] parameters = null,
			ProgressChangedHandler progressCallback = null,
			FailCallbackHandler onFail = null,
			int timeout = 0,
			int reportInterval = 5
		) {
			return ExecuteAsyncInternal(
				serviceName,
				(Action<AsyncResult<T>>)(ret => {
					if (ret.Successful)
						successCallback(ret.ReturnValue);
					else {
						var reallyFailCallback = GetReallyFailCallback(onFail);
						if (reallyFailCallback != null)
							reallyFailCallback(ret.Exception);
						else
							ret.ThrowEx();
					}
				}),
				parameters,
				progressCallback,
				timeout,
				reportInterval
			);
		}

		private ApiRequest ExecuteAsyncInternal<T>(
			string serviceName,
			Action<AsyncResult<T>> callback,
			object[] parameters = null,
			ProgressChangedHandler progressCallback = null,
			int timeout = 0,
			int reportInterval = 5
		) {
			var asyncRequest = CreateAsyncRequest(serviceName, parameters);
			asyncRequest.HttpRequest.Timeout = timeout * 1000;
			CallAsync(asyncRequest, callback, progressCallback, timeout, reportInterval);
			return asyncRequest;
		}

		public string Call(ApiRequest request, FailCallbackHandler onFail = null) => SafeDo(() => _httpClient.Send(request), GetReallyFailCallback(onFail));

		public T Call<T>(ApiRequest request, FailCallbackHandler onFail = null)
			=> SafeDo(
				() => {
					string json = _httpClient.Send(request);
					return string.IsNullOrEmpty(json)
						? default
						: typeof(T) == typeof(string)
							? (dynamic)json
							: JsonObject.Deserialize<T>(json);
				},
				GetReallyFailCallback(onFail)
			);

		public void CallAsync<T>(
			ApiRequest request,
			Action<AsyncResult<T>> callback,
			ProgressChangedHandler progressCallback = null,
			int timeout = 0,
			int reportInterval = 5
		) {
			_httpClient.SendAsync(
				request,
				ret => {
					ProgressReporter.Finish(request.RequestId);
					callback(ret.Cast<T>());
				}
			);
			if (progressCallback == null)
				return;
			var reporter = ProgressReporter.Create(this, request, progressCallback, reportInterval);
			if (reportInterval <= 0)
				return;
			new DelayInvoker<ApiRequest>(
				_ => {
					try {
						reporter.TimerCallback(null);
					}
					catch (WebException ex) {
						if (ex.Status == WebExceptionStatus.RequestCanceled)
							throw new TimeoutException(string.Format("请求超时{0}秒，请求被终止", timeout));
					}
				},
				request,
				timeout
			).Invoke();
		}

		private FailCallbackHandler GetReallyFailCallback(
			FailCallbackHandler onFail
		) {
			if (onFail != null)
				return onFail;
			return _defaultFailCallback;
		}

		private static T SafeDo<T>(Func<T> action, FailCallbackHandler onFail) {
			try {
				return action();
			}
			catch (ServiceException ex) {
				bool flag = false;
				if (onFail != null) {
					onFail(ex);
					flag = ex.Handled;
				}
				if (!flag)
					throw;
			}
			return default;
		}

		public string ValidateLogin2(
			string dbId,
			string username,
			string password,
			bool isKickOff,
			int lcid
		) {
			return Execute<string>("Kingdee.BOS.WebApi.ServicesStub.AuthService.ValidateUser2", new object[] {dbId, username, password, isKickOff, lcid});
		}

		public string LoginByAppSecret2(
			string dbId,
			string userName,
			string appId,
			string appSecret,
			bool isKickOff,
			int lcid = 2052
		) {
			return Execute<string>("Kingdee.BOS.WebApi.ServicesStub.AuthService.LoginByAppSecret2", new object[] {dbId, userName, appId, appSecret, isKickOff, lcid});
		}

		public string LoginBySign2(
			string dbId,
			string userName,
			string appId,
			long timestamp,
			string sign,
			bool isKickOff,
			int lcid = 2052
		) {
			return Execute<string>("Kingdee.BOS.WebApi.ServicesStub.AuthService.LoginBySign2", new object[] {dbId, userName, appId, timestamp, sign, isKickOff, lcid});
		}

		public string LoginBySimplePassport2(string passportForBase64, bool isKickOff, int lcid = 2052) => Execute<string>("Kingdee.BOS.WebApi.ServicesStub.AuthService.LoginBySimplePassport2", new object[] {passportForBase64, isKickOff, lcid});
	}
}