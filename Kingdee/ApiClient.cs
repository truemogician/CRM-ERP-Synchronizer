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

	public enum Language {
		ChineseChina = 2052
	}

	public class ApiClient {
		private readonly CookieContainer _cookiesContainer;

		private readonly FailCallbackHandler _defaultFailCallback;

		private readonly int _defaultTimeout = 3000000;

		private readonly Encoding _encoder;

		private readonly HttpClient _httpClient;

		private readonly string _serverUrl;

		protected ConnectionConfig ConnectionConfig;

		#region Constructors
		public ApiClient(string serverUrl) {
			_encoder = new UTF8Encoding();
			_serverUrl = serverUrl;
			_cookiesContainer = new CookieContainer();
			_httpClient = new HttpClient();
		}

		public ApiClient(string serverUrl, string dbId, string username, string password, Language lcid = Language.ChineseChina) : this(serverUrl)
			=> ConnectionConfig = new ConnectionConfig {
				Url = serverUrl,
				DatabaseId = dbId,
				Username = username,
				Password = password,
				Language = lcid
			};

		public ApiClient(string serverUrl, int timeout, string dbId, string username, string password, Language lcid = Language.ChineseChina)
			: this(serverUrl, dbId, username, password, lcid)
			=> _defaultTimeout = timeout;

		public ApiClient(string serverUrl, FailCallbackHandler onFail) : this(serverUrl) => _defaultFailCallback = onFail;

		public ApiClient(string serverUrl, FailCallbackHandler onFail, int timeout) : this(serverUrl, onFail) => _defaultTimeout = timeout;
		#endregion

		#region Methods
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ApiRequest CreateAsyncRequest(string serviceName, object[] parameters = null) => new ApiServiceRequest(_serverUrl, true, _encoder, _cookiesContainer, serviceName, parameters);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ApiRequest CreateRequest(string serviceName, object[] parameters = null) => new ApiServiceRequest(_serverUrl, false, _encoder, _cookiesContainer, serviceName, parameters);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ApiRequest CreateProgressQuery(string reqId) => new ApiProgressRequest(_serverUrl, false, _encoder, _cookiesContainer, reqId);

		#region Login
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Login(string dbId, string userName, string password, int lcid) => JObject.Parse(Execute<string>("Kingdee.BOS.WebApi.ServicesStub.AuthService.ValidateUserEnDeCode", dbId, EnDecode.Encode(userName), EnDecode.Encode(password), lcid))["LoginResultType"]?.Value<int>() == 1;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public LoginResponse ValidateLogin() => ValidateLogin(ConnectionConfig.DatabaseId, ConnectionConfig.Username, ConnectionConfig.Password, ConnectionConfig.Language);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public LoginResponse ValidateLogin(string dbId, string userName, string password, Language lcid) => Execute<LoginResponse>("Kingdee.BOS.WebApi.ServicesStub.AuthService.ValidateUserEnDeCode", dbId, EnDecode.Encode(userName), EnDecode.Encode(password), (int)lcid);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Task<LoginResponse> ValidateLoginAsync() => ValidateLoginAsync(ConnectionConfig.DatabaseId, ConnectionConfig.Username, ConnectionConfig.Password, ConnectionConfig.Language);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Task<LoginResponse> ValidateLoginAsync(string dbId, string userName, string password, Language lcid) => ExecuteAsync<LoginResponse>("Kingdee.BOS.WebApi.ServicesStub.AuthService.ValidateUserEnDeCode", dbId, EnDecode.Encode(userName), EnDecode.Encode(password), (int)lcid);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public LoginResponse ValidateLogin2(bool kickOff) => ValidateLogin2(ConnectionConfig.DatabaseId, ConnectionConfig.Username, ConnectionConfig.Password, kickOff, ConnectionConfig.Language);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public LoginResponse ValidateLogin2(
			string dbId,
			string username,
			string password,
			bool isKickOff,
			Language lcid
		)
			=> Execute<LoginResponse>("Kingdee.BOS.WebApi.ServicesStub.AuthService.ValidateUser2", dbId, username, password, isKickOff, (int)lcid);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string LoginByAppSecret(
			string dbId,
			string userName,
			string appId,
			string appSecret,
			int lcid
		)
			=> Execute<string>("Kingdee.BOS.WebApi.ServicesStub.AuthService.LoginByAppSecret", dbId, userName, appId, appSecret, lcid);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string LoginByAppSecret2(
			string dbId,
			string userName,
			string appId,
			string appSecret,
			bool isKickOff,
			int lcid = 2052
		)
			=> Execute<string>("Kingdee.BOS.WebApi.ServicesStub.AuthService.LoginByAppSecret2", dbId, userName, appId, appSecret, isKickOff, lcid);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string LoginBySign(
			string dbId,
			string userName,
			string appId,
			long timestamp,
			string sign,
			int lcid
		)
			=> Execute<string>("Kingdee.BOS.WebApi.ServicesStub.AuthService.LoginBySign", dbId, userName, appId, timestamp, sign, lcid);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string LoginBySign2(
			string dbId,
			string userName,
			string appId,
			long timestamp,
			string sign,
			bool isKickOff,
			int lcid = 2052
		)
			=> Execute<string>("Kingdee.BOS.WebApi.ServicesStub.AuthService.LoginBySign2", dbId, userName, appId, timestamp, sign, isKickOff, lcid);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string LoginBySimplePassport(string passportForBase64, int lcid = 2052) => Execute<string>("Kingdee.BOS.WebApi.ServicesStub.AuthService.LoginBySimplePassport", passportForBase64, lcid);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string LoginBySimplePassport2(string passportForBase64, bool isKickOff, int lcid = 2052) => Execute<string>("Kingdee.BOS.WebApi.ServicesStub.AuthService.LoginBySimplePassport2", passportForBase64, isKickOff, lcid);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string LoginByMobileCard(
			string passportForBase64,
			string customizationParameter,
			int lcid = 2052
		)
			=> Execute<string>("Kingdee.BOS.WebApi.ServicesStub.AuthService.LoginByMobileCard", passportForBase64, customizationParameter, lcid);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string LoginByRsaAuth(string json) => Execute<string>("Kingdee.BOS.WebApi.ServicesStub.AuthService.LoginByRSAAuth", json);
		#endregion

		#region Logout
		public bool Logout() => Execute<bool>("Kingdee.BOS.WebApi.ServicesStub.AuthService.Logout", Array.Empty<object>());
		#endregion

		#region Execute
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TResponse ValidateAndExecute<TResponse, TForm>(string serviceName, RequestBase request) where TForm : FormBase => ValidateAndExecute<TResponse>(serviceName, FormMeta<TForm>.Name, JsonConvert.SerializeObject(request));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T ValidateAndExecute<T>(string serviceName, params object[] parameters) => ValidateAndExecute<T>(serviceName, parameters, timeout: _defaultTimeout);

		public T ValidateAndExecute<T>(
			string serviceName,
			object[] parameters = null,
			FailCallbackHandler onFail = null,
			int timeout = 1000000
		) {
			var loginResp = ValidateLogin();
			if (!loginResp)
				throw new LoginFailedException(loginResp);
			return Execute<T>(serviceName, parameters, onFail, timeout);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TResponse Execute<TResponse, TForm>(string serviceName, RequestBase request) where TForm : FormBase => Execute<TResponse>(serviceName, FormMeta<TForm>.Name, JsonConvert.SerializeObject(request));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T Execute<T>(string serviceName, params object[] parameters) => Execute<T>(serviceName, parameters, timeout: _defaultTimeout);

		public T Execute<T>(
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
		public Task<TResponse> ValidateAndExecuteAsync<TResponse, TForm>(
			string serviceName,
			RequestBase request
		) where TForm : FormBase
			=> ValidateAndExecuteAsync<TResponse>(
				serviceName,
				FormMeta<TForm>.Name,
				JsonConvert.SerializeObject(request)
			);

		public async Task<T> ValidateAndExecuteAsync<T>(
			string serviceName,
			params object[] parameters
		) {
			var loginResp = await ValidateLoginAsync();
			if (!loginResp)
				throw new LoginFailedException(loginResp);
			return await ExecuteAsync<T>(serviceName, parameters);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Task<TResponse> ExecuteAsync<TResponse, TForm>(
			string serviceName,
			RequestBase request
		) where TForm : FormBase
			=> ExecuteAsync<TResponse>(
				serviceName,
				FormMeta<TForm>.Name,
				JsonConvert.SerializeObject(request)
			);

		public Task<T> ExecuteAsync<T>(
			string serviceName,
			params object[] parameters
		) {
			var asyncRequest = CreateAsyncRequest(serviceName, parameters);
			asyncRequest.HttpRequest.Timeout = _defaultTimeout;
			return CallAsync<T>(asyncRequest);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ApiRequest ExecuteAsync<TResponse, TForm>(
			string serviceName,
			RequestBase request,
			Action<TResponse> onSucceed,
			ProgressChangedHandler onProgressChange = null,
			FailCallbackHandler onFail = null,
			int timeout = Timeout.Infinite,
			int reportInterval = 5
		) where TForm : FormBase
			=> ExecuteAsync(
				serviceName,
				onSucceed,
				new object[] {FormMeta<TForm>.Name, JsonConvert.SerializeObject(request)},
				onProgressChange,
				onFail,
				timeout,
				reportInterval
			);

		public ApiRequest ExecuteAsync<T>(
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
			new DelayInvoker<ApiRequest>(
				_ => {
					try {
						reporter.TimerCallback(null);
					}
					catch (WebException ex) {
						if (ex.Status == WebExceptionStatus.RequestCanceled)
							throw new TimeoutException($"请求超时{timeout}毫秒，请求被终止");
					}
				},
				request,
				timeout
			).Invoke();
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