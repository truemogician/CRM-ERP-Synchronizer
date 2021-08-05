using System;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Kingdee.Exceptions;
using Kingdee.Forms;
using Kingdee.Requests;
using Kingdee.Responses;
using Kingdee.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Kingdee {
	public class ConnectionConfig {
		public string Url { get; set; }

		public string DatabaseId { get; set; }

		public string Username { get; set; }

		public string Password { get; set; }

		public Language Language { get; set; } = Language.ChineseChina;
	}

	public class ApiClient {
		public static readonly TimeSpan LoginInterval = TimeSpan.FromHours(1);

		private readonly CookieContainer _cookiesContainer;

		private readonly FailCallbackHandler _defaultFailCallback;

		private readonly int _defaultTimeout = 3000000;

		private readonly Encoding _encoder;

		private readonly HttpClient _httpClient;

		protected string ServerUrl;

		protected LoginRequest DefaultLoginRequest;

		protected DateTime? LastLoginTime;

		#region Constructors
		public ApiClient(string serverUrl) {
			ServerUrl = serverUrl;
			_encoder = new UTF8Encoding();
			_cookiesContainer = new CookieContainer();
			_httpClient = new HttpClient();
		}

		public ApiClient(string serverUrl, string dbId, string username, string password, Language lcid = Language.ChineseChina) : this(serverUrl)
			=> DefaultLoginRequest = new LoginRequest {
				DatabaseId = dbId,
				Username = username,
				Password = password,
				Language = lcid
			};

		public ApiClient(string serverUrl, int timeout, string dbId, string username, string password, Language lcid = Language.ChineseChina)
			: this(serverUrl, dbId, username, password, lcid)
			=> _defaultTimeout = timeout;

		public ApiClient(string serverUrl, FailCallbackHandler onFail) : this(serverUrl) => _defaultFailCallback = onFail;
		#endregion

		#region Methods
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected ApiRequest CreateAsyncRequest(string serviceName, object[] parameters = null) => new ApiServiceRequest(ServerUrl, true, _encoder, _cookiesContainer, serviceName, parameters);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected ApiRequest CreateRequest(string serviceName, object[] parameters = null) => new ApiServiceRequest(ServerUrl, false, _encoder, _cookiesContainer, serviceName, parameters);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal ApiRequest CreateProgressQuery(string reqId) => new ApiProgressRequest(ServerUrl, false, _encoder, _cookiesContainer, reqId);

		#region Login
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public LoginResponse ValidateLogin() => ValidateLogin(DefaultLoginRequest);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public LoginResponse ValidateLogin(LoginRequest request) => Execute<LoginRequest, LoginResponse>(request);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Task<LoginResponse> ValidateLoginAsync() => ValidateLoginAsync(DefaultLoginRequest);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Task<LoginResponse> ValidateLoginAsync(LoginRequest request) => ExecuteAsync<LoginRequest, LoginResponse>(request);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public LoginResponse ValidateLogin2(bool kickOff) => ValidateLogin2(DefaultLoginRequest, kickOff);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public LoginResponse ValidateLogin2(LoginRequest request, bool isKickOff) => Execute<LoginRequest, LoginResponse>(request.DatabaseId, request.Username, request.Password, isKickOff, (int)request.Language);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string LoginByAppSecret(
			string dbId,
			string userName,
			string appId,
			string appSecret,
			Language lcid = Language.ChineseChina
		)
			=> Execute<string>("Kingdee.BOS.WebApi.ServicesStub.AuthService.LoginByAppSecret", new object[] {dbId, userName, appId, appSecret, (int)lcid});

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string LoginByAppSecret2(
			string dbId,
			string userName,
			string appId,
			string appSecret,
			bool isKickOff,
			Language lcid = Language.ChineseChina
		)
			=> Execute<string>("Kingdee.BOS.WebApi.ServicesStub.AuthService.LoginByAppSecret2", new object[] {dbId, userName, appId, appSecret, isKickOff, (int)lcid});

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string LoginBySign(
			string dbId,
			string userName,
			string appId,
			long timestamp,
			string sign,
			Language lcid = Language.ChineseChina
		)
			=> Execute<string>("Kingdee.BOS.WebApi.ServicesStub.AuthService.LoginBySign", new object[] {dbId, userName, appId, timestamp, sign, (int)lcid});

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string LoginBySign2(
			string dbId,
			string userName,
			string appId,
			long timestamp,
			string sign,
			bool isKickOff,
			Language lcid = Language.ChineseChina
		)
			=> Execute<string>("Kingdee.BOS.WebApi.ServicesStub.AuthService.LoginBySign2", new object[] {dbId, userName, appId, timestamp, sign, isKickOff, (int)lcid});

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string LoginBySimplePassport(string passportForBase64, Language lcid = Language.ChineseChina) => Execute<string>("Kingdee.BOS.WebApi.ServicesStub.AuthService.LoginBySimplePassport", new object[] {passportForBase64, (int)lcid});

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string LoginBySimplePassport2(string passportForBase64, bool isKickOff, Language lcid = Language.ChineseChina) => Execute<string>("Kingdee.BOS.WebApi.ServicesStub.AuthService.LoginBySimplePassport2", new object[] {passportForBase64, isKickOff, (int)lcid});

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string LoginByMobileCard(
			string passportForBase64,
			string customizationParameter,
			Language lcid = Language.ChineseChina
		)
			=> Execute<string>("Kingdee.BOS.WebApi.ServicesStub.AuthService.LoginByMobileCard", new object[] {passportForBase64, customizationParameter, (int)lcid});

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string LoginByRsaAuth(string json) => Execute<string>("Kingdee.BOS.WebApi.ServicesStub.AuthService.LoginByRSAAuth", new object[] {json});
		#endregion

		#region Logout
		public bool Logout() => Execute<bool>("Kingdee.BOS.WebApi.ServicesStub.AuthService.Logout", Array.Empty<object>());
		#endregion

		#region Execute
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected TResponse ValidateAndExecute<TRequest, TResponse, TModel>(TRequest request) where TRequest : RequestBase where TModel : ErpModelBase => ValidateAndExecute<TRequest, TResponse>(FormMeta<TModel>.Name, JsonConvert.SerializeObject(request));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected T ValidateAndExecute<TRequest, T>(params object[] parameters) where TRequest : RequestBase => ValidateAndExecute<TRequest, T>(parameters, timeout: _defaultTimeout);

		protected T ValidateAndExecute<TRequest, T>(
			object[] parameters = null,
			FailCallbackHandler onFail = null,
			int timeout = 1000000
		) where TRequest : RequestBase {
			var now = DateTime.Now;
			if (!LastLoginTime.HasValue || now - LastLoginTime.Value >= LoginInterval) {
				var loginResp = ValidateLogin();
				if (!loginResp)
					throw new LoginFailedException(loginResp);
				LastLoginTime = now;
			}
			return Execute<T>(typeof(TRequest).GetServiceName(), parameters, onFail, timeout);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected TResponse Execute<TRequest, TResponse, TModel>(TRequest request) where TRequest : RequestBase where TModel : ErpModelBase => Execute<TRequest, TResponse>(FormMeta<TModel>.Name, JsonConvert.SerializeObject(request));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected T Execute<TRequest, T>(params object[] parameters) where TRequest : RequestBase => Execute<T>(typeof(TRequest).GetServiceName(), parameters, timeout: _defaultTimeout);

		protected T Execute<T>(
			string serviceName,
			object[] parameters = null,
			FailCallbackHandler onFail = null,
			int timeout = 1000000
		) {
			var request = CreateRequest(serviceName, parameters);
			request.HttpRequest.Timeout = timeout;
			return Call<T>(request, onFail);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected Task<TResponse> ValidateAndExecuteAsync<TRequest, TResponse, TModel>(TRequest request) where TRequest : RequestBase where TModel : ErpModelBase => ValidateAndExecuteAsync<TRequest, TResponse>(FormMeta<TModel>.Name, JsonConvert.SerializeObject(request));

		protected async Task<TResponse> ValidateAndExecuteAsync<TRequest, TResponse>(params object[] parameters) where TRequest : RequestBase {
			var now = DateTime.Now;
			if (!LastLoginTime.HasValue || now - LastLoginTime.Value >= LoginInterval) {
				var loginResp = await ValidateLoginAsync();
				if (!loginResp)
					throw new LoginFailedException(loginResp);
				LastLoginTime = now;
			}
			return await ExecuteAsync<TRequest, TResponse>(parameters);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected Task<TResponse> ExecuteAsync<TRequest, TResponse, TModel>(TRequest request) where TRequest : RequestBase where TModel : ErpModelBase => ExecuteAsync<TRequest, TResponse>(FormMeta<TModel>.Name, JsonConvert.SerializeObject(request));

		protected Task<T> ExecuteAsync<TRequest, T>(params object[] parameters) where TRequest : RequestBase {
			var asyncRequest = CreateAsyncRequest(typeof(TRequest).GetServiceName(), parameters);
			asyncRequest.HttpRequest.Timeout = _defaultTimeout;
			return CallAsync<T>(asyncRequest);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected ApiRequest ExecuteAsync<TResponse, TModel>(
			string serviceName,
			RequestBase request,
			Action<TResponse> onSucceed,
			ProgressChangedHandler onProgressChange = null,
			FailCallbackHandler onFail = null,
			int timeout = Timeout.Infinite,
			int reportInterval = 5
		) where TModel : ErpModelBase
			=> ExecuteAsync(
				serviceName,
				onSucceed,
				new object[] {FormMeta<TModel>.Name, JsonConvert.SerializeObject(request)},
				onProgressChange,
				onFail,
				timeout,
				reportInterval
			);

		protected ApiRequest ExecuteAsync<T>(
			string serviceName,
			Action<T> onSucceed,
			object[] parameters = null,
			ProgressChangedHandler onProgressChange = null,
			FailCallbackHandler onFail = null,
			int timeout = Timeout.Infinite,
			int reportInterval = 5
		) {
			var asyncRequest = CreateAsyncRequest(serviceName, parameters);
			asyncRequest.HttpRequest.Timeout = timeout;
			CallAsync<T>(
				asyncRequest,
				ret => {
					if (ret.Successful)
						onSucceed(ret.ReturnValue);
					else {
						var failCallback = OnFailOrDefault(onFail);
						if (failCallback != null)
							failCallback(ret.Exception);
						else
							ret.ThrowEx();
					}
				},
				onProgressChange,
				timeout,
				reportInterval
			);
			return asyncRequest;
		}
		#endregion

		#region Call
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string Call(ApiRequest request, FailCallbackHandler onFail = null) => SafeDo(() => HttpClient.Send(request), OnFailOrDefault(onFail));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T Call<T>(ApiRequest request, FailCallbackHandler onFail = null)
			=> SafeDo(
				() => {
					string json = HttpClient.Send(request);
					return string.IsNullOrEmpty(json)
						? default
						: typeof(T) == typeof(string)
							? (dynamic)json
							: JsonConvert.DeserializeObject<T>(json);
				},
				OnFailOrDefault(onFail)
			);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Task<T> CallAsync<T>(ApiRequest request, FailCallbackHandler onFail = null)
			=> SafeDoAsync(
				() => {
					var taskSource = new TaskCompletionSource<T>();
					_httpClient.SendAsync(
						request,
						result => { taskSource.SetResult(typeof(T) == typeof(string) ? (dynamic)result.ReturnValue : JsonConvert.DeserializeObject<T>(result.ReturnValue)); }
					);
					return taskSource.Task;
				},
				onFail
			);

		public void CallAsync<T>(
			ApiRequest request,
			Action<AsyncResult<T>> callback,
			ProgressChangedHandler onProgressChange = null,
			int timeout = Timeout.Infinite,
			int reportInterval = 5
		) {
			_httpClient.SendAsync(
				request,
				ret => {
					ProgressReporter.Finish(request.RequestId);
					callback(ret.Cast<T>());
				}
			);
			if (onProgressChange == null)
				return;
			var reporter = ProgressReporter.Create(this, request, onProgressChange, reportInterval);
			if (reportInterval <= 0)
				return;
			if (timeout != Timeout.Infinite) {
				var _ = Task.Delay(timeout)
					.ContinueWith(
						_ => {
							try {
								reporter.TimerCallback(null);
							}
							catch (WebException ex) {
								if (ex.Status == WebExceptionStatus.RequestCanceled)
									throw new TimeoutException($"请求超时{timeout}毫秒，请求被终止");
							}
						}
					);
			}
		}
		#endregion

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private FailCallbackHandler OnFailOrDefault(FailCallbackHandler onFail) => onFail ?? _defaultFailCallback;

		private static T SafeDo<T>(Func<T> action, FailCallbackHandler onFail) {
			try {
				return action();
			}
			catch (ServiceException ex) {
				onFail?.Invoke(ex);
				if (onFail is null || !ex.Handled)
					throw;
				return default;
			}
		}

		private static async Task<T> SafeDoAsync<T>(Func<Task<T>> action, FailCallbackHandler onFail) {
			try {
				return await action();
			}
			catch (ServiceException ex) {
				onFail?.Invoke(ex);
				if (onFail is null || !ex.Handled)
					throw;
				return default;
			}
		}
		#endregion
	}
}