using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using FXiaoKe.Models;
using FXiaoKe.Request;
using FXiaoKe.Request.Message;
using FXiaoKe.Response;
using FXiaoKe.Utilities;
using Newtonsoft.Json;

namespace FXiaoKe {
	public class Client {
		public const string Origin = "https://open.fxiaoke.com";

		public Client() { }

		public Client(string appId, string appSecret, string permanentCode)
			=> AuthorizationInfo = new AuthorizationRequest {
				AppId = appId,
				AppSecret = appSecret,
				PermanentCode = permanentCode
			};

		public HttpClient HttpClient { get; } = new();

		public AuthorizationRequest AuthorizationInfo { get; } = new();

		public string CorpId { get; private set; }
		public string CorpAccessToken { get; private set; }
		public string OperatorId { get; private set; }

		public async Task<string> ReceiveJson<T>(T request) where T : RequestBase {
			var respMessage = await HttpClient.SendAsync(request);
			return await respMessage.Content.ReadAsStringAsync();
		}

		public async Task<dynamic> ReceiveResponse<T>(T request) where T : RequestBase {
			var attrs = typeof(T).GetRequestAttributes();
			string json = await ReceiveJson(request);
			return typeof(JsonConvert).GetMethod("DeserializeObject", BindingFlags.Static | BindingFlags.Public)!.MakeGenericMethod(attrs.First(attr => attr.ResponseType is not null).ResponseType).Invoke(null, new object[] {json});
		}

		public async Task<TResponse> ReceiveResponse<TResponse, TRequest>(TRequest request) where TRequest : RequestBase => JsonConvert.DeserializeObject<TResponse>(await ReceiveJson(request));

		public async Task<bool> Authorize(string staffPhoneNumber) {
			var authResp = await ReceiveResponse<AuthorizationResponse, AuthorizationRequest>(AuthorizationInfo);
			if (authResp.ErrorCode != ErrorCode.Success)
				return false;
			CorpId = authResp.CorpId;
			CorpAccessToken = authResp.CorpAccessToken;
			var staffResp = await ReceiveResponse<StaffQueryResponse, StaffQueryRequest>(
				new StaffQueryRequest {
					CorpId = CorpId,
					CorpAccessToken = CorpAccessToken,
					PhoneNumber = staffPhoneNumber
				}
			);
			if (staffResp.ErrorCode != ErrorCode.Success)
				return false;
			OperatorId = staffResp.Staffs[0].Id;
			return true;
		}

		public async Task<List<T>> QueryByCondition<T>(QueryCondition<T> condition) where T : ModelBase {
			var response = await (ModelMeta<T>.IsCustomModel
				? ReceiveResponse<QueryByConditionResponse<T>, QueryCustomByConditionRequest<T>>(
					new QueryCustomByConditionRequest<T>(this) {
						Data = (ConditionInfo<T>)condition
					}
				)
				: ReceiveResponse<QueryByConditionResponse<T>, QueryByConditionRequest<T>>(
					new QueryByConditionRequest<T>(this) {
						Data = (ConditionInfo<T>)condition
					}
				));
			return response.Data.DataList;
		}

		public Task<List<T>> QueryByCondition<T>(ModelFilter<T>[] filters) where T : ModelBase => QueryByCondition(new QueryCondition<T>(filters));

		public async Task<T> QueryById<T>(string id) where T : ModelBase {
			var response = await (ModelMeta<T>.IsCustomModel
				? ReceiveResponse<QueryByIdResponse<T>, QueryCustomByIdRequest<T>>(new QueryCustomByIdRequest<T>(id, this))
				: ReceiveResponse<QueryByIdResponse<T>, QueryByIdRequest<T>>(new QueryByIdRequest<T>(id, this)));
			return response.Data;
		}

		public Task<BasicResponse> SendMessage<T>(T request) where T : MessageRequest => SendRequestWithAuth<BasicResponse, T>(request);

		public Task<BasicResponse> SendTextMessage(string text, params string[] receiversIds) {
			var receiversIdsList = receiversIds.ToList();
			if (receiversIdsList.Count == 0 && !string.IsNullOrEmpty(OperatorId))
				receiversIdsList.Add(OperatorId);
			return SendMessage(
				new TextMessageRequest(text, this) {
					ReceiversIds = receiversIdsList
				}
			);
		}

		private Task<TResponse> SendRequestWithAuth<TResponse, TRequest>(TRequest request) where TRequest : RequestWithBasicAuth {
			request.UseClient(this);
			return ReceiveResponse<TResponse, TRequest>(request);
		}
	}
}