using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using FXiaoKe.Exceptions;
using FXiaoKe.Models;
using FXiaoKe.Request;
using FXiaoKe.Request.Message;
using FXiaoKe.Response;
using FXiaoKe.Utilities;
using Newtonsoft.Json;
using Shared.Exceptions;

namespace FXiaoKe {
	public class Client {
		private static readonly MethodInfo DeserializeObject = typeof(JsonConvert)
			.GetMethods(BindingFlags.Static | BindingFlags.Public)
			.Single(method => method.Name == nameof(JsonConvert.DeserializeObject) && method.IsGenericMethod);

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

		protected DateTime? ExpireAt { get; private set; }

		public string OperatorId { get; private set; }

		public async Task<dynamic> ReceiveResponse<T>(T request) where T : RequestBase {
			var attrs = typeof(T).GetRequestAttributes();
			var responseType = attrs.FirstOrDefault(attr => attr.ResponseType is not null)?.ResponseType;
			if (responseType is null)
				throw new RequestException<T>(request, "Response type not specified");
			string json = await ValidateAndSend(request);
			var response = DeserializeObject.MakeGenericMethod(responseType).Invoke(null, new object[] {json}) as ResponseBase;
			ValidateResponse(response);
			return response;
		}

		public async Task<TResponse> ReceiveResponse<TResponse, TRequest>(TRequest request) where TRequest : RequestBase where TResponse : ResponseBase {
			var response = JsonConvert.DeserializeObject<TResponse>(await ValidateAndSend(request));
			ValidateResponse(response);
			return response;
		}

		public async Task<AuthorizationResponse> Authorize() {
			var authResp = await ReceiveResponse<AuthorizationResponse, AuthorizationRequest>(AuthorizationInfo);
			if (authResp.ErrorCode != ErrorCode.Success)
				return authResp;
			CorpId = authResp.CorpId;
			CorpAccessToken = authResp.CorpAccessToken;
			return authResp;
		}

		public async Task<bool> SetOperator(string staffPhoneNumber) {
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

		public Task<BasicResponse> SendMessage<T>(T request) where T : MessageRequest => ReceiveResponse<BasicResponse, T>(request);

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

		protected async Task AuthenticateRequest<T>(T request) where T : RequestBase {
			if (request is RequestWithBasicAuth req) {
				if (ExpireAt.HasValue && DateTime.Now < ExpireAt.Value)
					return;
				var resp = await Authorize();
				if (!resp)
					throw new GenericException<AuthorizationResponse>(resp, "Authorization failed");
				ExpireAt = DateTime.Now + resp.ExpiresIn;
				req.UseClient(this);
			}
		}

		private async Task<string> ValidateAndSend<T>(T request) where T : RequestBase {
			await AuthenticateRequest(request);
			var validationResults = request.Validate();
			if (validationResults?.Count > 0)
				throw new GenericException<List<ValidationResult>>(validationResults, "Request validation failed");
			var respMessage = await HttpClient.SendAsync(request);
			string json = await respMessage.Content.ReadAsStringAsync();
			return json;
		}

		private static void ValidateResponse(ResponseBase response) {
			var result = response.Validate();
			if (result?.Count > 0)
				throw new GenericException<List<ValidationResult>>(result, "Response validation failed");
		}
	}
}