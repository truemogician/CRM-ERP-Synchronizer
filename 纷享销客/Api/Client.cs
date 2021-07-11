using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FXiaoKe.Api.Request;
using FXiaoKe.Api.Response;
using FXiaoKe.Utilities;
using Newtonsoft.Json;

namespace FXiaoKe.Api {
	public class Client {
		public const string Origin = "https://open.fxiaoke.com";

		public Client() { }

		public Client(string appId, string appSecret, string permanentCode)
			=> AuthorizationInfo = new AuthorizationRequest() {
				AppId = appId,
				AppSecret = appSecret,
				PermanentCode = permanentCode
			};

		public HttpClient HttpClient { get; } = new();

		public AuthorizationRequest AuthorizationInfo { get; } = new();

		public string CorpId { get; private set; }
		public string CorpAccessToken { get; private set; }
		public string OperatorId { get; private set; }

		public Task<HttpResponseMessage> SendRequest<TRequest>(TRequest request) {
			var attribute = typeof(TRequest).GetCustomAttribute<RequestAttribute>();
			if (attribute is null)
				throw new InvalidOperationException($"Type {nameof(TRequest)} doesn't have attribute {nameof(RequestAttribute)}");
			string url = Origin + attribute.Path;
			var reqMessage = new HttpRequestMessage(attribute.Method, url) {
				Content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8)
			};
			reqMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
			return HttpClient.SendAsync(reqMessage);
		}

		public async Task<string> ReceiveJson<TRequest>(TRequest request) {
			var respMessage = await SendRequest(request);
			return await respMessage.Content.ReadAsStringAsync();
		}

		public async Task<dynamic> ReceiveResponse<TRequest>(TRequest request) {
			var attribute = typeof(TRequest).GetCustomAttribute<RequestAttribute>();
			string json = await ReceiveJson(request);
			return typeof(JsonConvert).GetMethod("DeserializeObject", BindingFlags.Static | BindingFlags.Public)!.MakeGenericMethod(attribute!.ResponseType).Invoke(null, new object[] {json});
		}

		public async Task<TResponse> ReceiveResponse<TResponse, TRequest>(TRequest request) => JsonConvert.DeserializeObject<TResponse>(await ReceiveJson(request));

		public async Task<bool> Authorize(string staffPhoneNumber) {
			var authResp = await ReceiveResponse<AuthorizationResponse, AuthorizationRequest>(AuthorizationInfo);
			if (authResp.ErrorCode != ErrorCode.Success)
				return false;
			CorpId = authResp.CorpId;
			CorpAccessToken = authResp.CorpAccessToken;
			var staffResp = await ReceiveResponse<StaffQueryResponse, StaffQueryRequest>(
				new StaffQueryRequest() {
					CorpId = CorpId,
					CorpAccessToken = CorpAccessToken,
					PhoneNumber = staffPhoneNumber
				}
			);
			if (staffResp.ErrorCode != ErrorCode.Success)
				return false;
			OperatorId = staffResp.Staffs[0].OpenUserId;
			return true;
		}
	}
}