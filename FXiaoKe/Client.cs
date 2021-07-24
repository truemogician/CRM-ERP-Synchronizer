using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using FXiaoKe.Exceptions;
using FXiaoKe.Models;
using FXiaoKe.Requests;
using FXiaoKe.Requests.Message;
using FXiaoKe.Responses;
using FXiaoKe.Utilities;
using Newtonsoft.Json;
using Shared;
using Shared.Exceptions;

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

		protected DateTime? ExpireAt { get; private set; }

		public Staff Operator { get; set; }

		public event CommonEventHandler<ValidationFailedEventArgs> ValidationFailed = delegate { };

		public event CommonEventHandler<RequestFailedEventArgs> RequestFailed = delegate { };

		public event CommonEventHandler<RequestSucceededEventArgs> RequestSucceeded = delegate { };

		public async Task<TResponse> ReceiveResponse<TResponse, TRequest>(TRequest request) where TRequest : RequestBase where TResponse : ResponseBase => await ReceiveResponse(request, typeof(TResponse)) as TResponse;

		public Task<ResponseBase> ReceiveResponse<T>(T request) where T : RequestBase {
			var responseType = request.Attribute.ResponseType;
			if (responseType is null || !responseType.IsAssignableTo(typeof(ResponseBase)))
				throw new RequestException(request, "Response type not specified");
			return ReceiveResponse(request, responseType);
		}

		private async Task<ResponseBase> ReceiveResponse<TRequest>(TRequest request, Type responseType) where TRequest : RequestBase {
			var respMessage = await ValidateAndSend(request);
			ResponseBase response;
			if (!respMessage.IsSuccessStatusCode) {
				response = responseType.Construct() as ResponseBase;
				response!.ResponseMessage = respMessage;
				var args = new RequestFailedEventArgs(request, response);
				RequestFailed(this, args);
				return args.Continue ? response : throw new RequestFailedException(request, response);
			}
			string json = await respMessage.Content.ReadAsStringAsync();
			response = JsonConvert.DeserializeObject(json, responseType) as ResponseBase;
			response!.ResponseMessage = respMessage;
			if (response is BasicResponse resp && !resp) {
				var args = new RequestFailedEventArgs(request, response);
				RequestFailed(this, args);
				if (!args.Continue)
					throw new RequestFailedException(request, response);
			}
			else if (ValidateResponse(response))
				RequestSucceeded(this, new RequestSucceededEventArgs(request, response));
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

		public async Task<Staff> GetStaffByPhoneNumber(string phoneNumber) {
			var request = new StaffQueryRequest(phoneNumber);
			var resp = await ReceiveResponse<StaffQueryResponse, StaffQueryRequest>(request);
			return resp.Staffs.SingleOrDefault();
		}

		private async Task<List<ModelBase>> QueryByCondition(QueryByConditionRequest request, bool? eager) {
			var response = await ReceiveResponse(request, typeof(QueryByConditionResponse<>).MakeGenericType(request.Data.Type));
			var results = (((dynamic)response).Data.DataList as IList)!.AsType<ModelBase>().AsList();
			if (eager == false || results.Count == 0)
				return results;
			var resultType = results[0].GetType();
			var subModelInfos = eager.HasValue
				? resultType.GetSubModelInfos()
				: resultType.GetSubModelInfos().Where(info => info.GetCustomAttribute<SubModelAttribute>()!.Eager).AsList();
			if (subModelInfos.Count == 0)
				return results;
			foreach (var subModelInfo in subModelInfos) {
				var rawType = subModelInfo.GetValueType();
				bool many = rawType.Implements(typeof(ICollection<>));
				var subModelType = many ? rawType.GetItemType(typeof(ICollection<>)) : rawType;
				if (subModelType.GetCustomAttribute<ModelAttribute>()!.SubjectTo is var type && type != resultType)
					throw new TypeNotMatchException(resultType, type);
				var (masterKeyMember, masterKey) = subModelType.GetMemberAndAttribute<MasterKeyAttribute>();
				if (masterKey is null)
					throw new AttributeNotFoundException(typeof(MasterKeyAttribute));
				var keyInfo = masterKey.GetKey(subModelType);
				var condition = new QueryCondition(ModelFilter.In(subModelType, masterKeyMember.Name, results.Select(res => keyInfo.GetValue(res)).ToHashSet().AsArray()));
				var subReq = subModelType.IsCustomModel()
					? new QueryCustomByConditionRequest(condition)
					: new QueryByConditionRequest(condition);
				var subResults = await QueryByCondition(subReq, eager);
				foreach (var group in subResults.GroupBy(subRes => masterKeyMember.GetValue(subRes))) {
					var target = results.Single(res => keyInfo.GetValue(res).Equals(group.Key));
					if (!many)
						subModelInfo.SetValue(target, group.SingleOrDefault());
					else {
						object collection = subModelInfo.GetValue(target);
						if (collection is null) {
							var constrType = subModelInfo.GetCustomAttribute<DefaultConstructorAttribute>()?.ConstructingType;
							if (constrType is not null && !constrType.IsAssignableTo(rawType))
								throw new InvariantTypeException(rawType, constrType);
							constrType ??= rawType;
							collection = constrType.Construct();
							subModelInfo.SetValue(target, collection);
						}
						var collectionType = collection.GetType().GetGenericInterface(typeof(ICollection<>));
						collectionType.GetMethod(nameof(ICollection<object>.Clear)).Invoke(collection);
						foreach (var subRes in group)
							collectionType.GetMethod(nameof(ICollection<object>.Add)).Invoke(collection, subRes);
					}
				}
			}
			return results;
		}

		public async Task<List<T>> QueryByCondition<T>(QueryCondition<T> condition, bool? eager = null) where T : ModelBase {
			var request = ModelMeta<T>.IsCustomModel
				? new QueryCustomByConditionRequest(condition)
				: new QueryByConditionRequest(condition);
			var response = await QueryByCondition(request, eager);
			return response.Cast<T>().AsList();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Task<List<T>> QueryByCondition<T>(ModelFilter<T>[] filters, bool? eager = null) where T : ModelBase => QueryByCondition(new QueryCondition<T>(filters), eager);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Task<List<T>> GetAll<T>(int limit = 0, int offset = 0, bool? eager = null) where T : ModelBase => QueryByCondition(new QueryCondition<T> {Limit = limit, Offset = offset}, eager);

		public async Task<T> QueryById<T>(string id) where T : ModelBase {
			var response = await (ModelMeta<T>.IsCustomModel
				? ReceiveResponse<QueryByIdResponse<T>, CustomQueryByIdRequest<T>>(new CustomQueryByIdRequest<T>(id))
				: ReceiveResponse<QueryByIdResponse<T>, QueryByIdRequest<T>>(new QueryByIdRequest<T>(id)));
			return response.Data;
		}

		private async Task<CreationResponse> Create(ModelBase model, bool? cascade = null) {
			var response = await (model.GetType().IsCustomModel()
				? ReceiveResponse<CreationResponse, CustomCreationRequest<ModelBase>>(new CustomCreationRequest<ModelBase> {Data = model})
				: ReceiveResponse<CreationResponse, CreationRequest<ModelBase>>(new CreationRequest<ModelBase> {Data = model}));
			if (!response || cascade == false)
				return response;
			IEnumerable<ModelBase> cascadeModels;
			if (cascade.HasValue)
				cascadeModels = model.SubModels;
			else {
				var infos = model.GetType().GetSubModelInfos().Where(info => info.GetCustomAttribute<SubModelAttribute>()!.Cascade);
				cascadeModels = infos.SelectSingleOrMany<MemberInfo, ModelBase>(info => info.GetValue(model));
			}
			foreach (var subModel in cascadeModels) {
				if (subModel!.GetType().GetModelAttribute().SubjectTo is var srcType && srcType != model.GetType())
					throw new TypeNotMatchException(model.GetType(), srcType);
				var info = subModel.GetType().GetMemberWithAttribute<MasterKeyAttribute>() ?? throw new AttributeNotFoundException(typeof(MasterKeyAttribute));
				info.SetValue(subModel, response.DataId);
				if (await Create(subModel, cascade) is var resp && !resp)
					return resp;
			}
			return response;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Task<CreationResponse> Create<T>(T model, bool? cascade = null) where T : ModelBase => Create((ModelBase)model, cascade);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Task<BasicResponse> SendMessage<T>(T request) where T : MessageRequest => ReceiveResponse<BasicResponse, T>(request);

		public Task<BasicResponse> SendTextMessage(string text, params string[] receiversIds) {
			var receiversIdsList = receiversIds.ToList();
			if (receiversIdsList.Count == 0 && !string.IsNullOrEmpty(Operator?.Id))
				receiversIdsList.Add(Operator.Id);
			return SendMessage(new TextMessageRequest(text) {ReceiversIds = receiversIdsList});
		}

		public Task<BasicResponse> SendCompositeMessage(CompositeMessage message, params string[] receiversIds) {
			var receiversIdsList = receiversIds.ToList();
			if (receiversIdsList.Count == 0 && !string.IsNullOrEmpty(Operator?.Id))
				receiversIdsList.Add(Operator.Id);
			return SendMessage(
				new CompositeMessageRequest {
					Composite = message,
					ReceiversIds = receiversIdsList
				}
			);
		}

		public async Task<DepartmentInfo> GetDepartmentInfoTree() {
			var response = await ReceiveResponse<DepartmentListResponse, DepartmentListRequest>(new DepartmentListRequest());
			return response is null ? null : DepartmentInfo.BuildTree(response.Departments, nameof(DepartmentInfo.Id), nameof(DepartmentInfo.ParentId));
		}

		public async Task<Department> GetDepartmentDetailTree()
			=> await (await GetDepartmentInfoTree()).CastAsync(
				async info => {
					var response = await ReceiveResponse<DepartmentDetailResponse, DepartmentDetailRequest>(new DepartmentDetailRequest(info.Id));
					return response!.Department;
				}
			);

		public async Task<List<Staff>> GetStaffs(Department department, bool includeChild = true) {
			var request = new StaffListRequest(department.Id, includeChild);
			var response = await ReceiveResponse<StaffListResponse, StaffListRequest>(request);
			if (response is null)
				return null;
			var staffs = response.Staffs;
			if (includeChild)
				foreach (var staff in staffs) {
					var target = department.Search(d => d.Id == staff.Department.Id);
					if (target is null)
						throw new InvalidOperationException($"Department {staff.Department.Id} not found");
					target.Staffs ??= new List<Staff>();
					target.Staffs.Add(staff);
					staff.Department = target;
				}
			else {
				department.Staffs = staffs;
				foreach (var staff in staffs)
					staff.Department = department;
			}
			return staffs;
		}

		protected async Task AuthenticateRequest<T>(T request) where T : RequestBase {
			if (request is RequestWithBasicAuth req) {
				if (!ExpireAt.HasValue || DateTime.Now >= ExpireAt.Value) {
					var resp = await Authorize();
					ExpireAt = DateTime.Now + resp.ExpiresIn;
				}
				req.UseClient(this);
			}
		}

		private async Task<HttpResponseMessage> ValidateAndSend<T>(T request) where T : RequestBase {
			await AuthenticateRequest(request);
			var results = request.Validate();
			if (results?.Count > 0) {
				var args = new ValidationFailedEventArgs(request, results);
				ValidationFailed(this, args);
				if (!args.Continue)
					throw new ValidationFailedException(request, results);
			}
			return await HttpClient.SendAsync(request);
		}

		private bool ValidateResponse(ResponseBase response) {
			var results = response.Validate();
			if (results?.Count > 0) {
				var args = new ValidationFailedEventArgs(response, results);
				ValidationFailed(this, args);
				if (!args.Continue)
					throw new ValidationFailedException(response, results);
				return false;
			}
			return true;
		}

		public class RequestSucceededEventArgs : EventArgs {
			public RequestSucceededEventArgs(RequestBase request = null, ResponseBase response = null) {
				Request = request;
				Response = response;
			}

			public RequestBase Request { get; }

			public ResponseBase Response { get; }
		}

		public class RequestFailedEventArgs : RequestSucceededEventArgs {
			public RequestFailedEventArgs(RequestBase request = null, ResponseBase response = null) : base(request, response) { }
			public bool Continue { get; set; } = false;
		}

		public class ValidationFailedEventArgs : EventArgs {
			public ValidationFailedEventArgs(object obj, IEnumerable<ValidationResult> results) {
				SourceObject = obj;
				Results = results.AsList();
			}

			public object SourceObject { get; }

			public IReadOnlyList<ValidationResult> Results { get; }

			public bool Continue { get; set; } = true;
		}
	}
}