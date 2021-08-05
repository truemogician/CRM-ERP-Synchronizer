using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Kingdee.Forms;
using Kingdee.Requests;
using Kingdee.Requests.Query;
using Kingdee.Responses;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OneOf;
using Shared.Exceptions;

namespace Kingdee {
	public class Client : ApiClient {
		public Client(string serverUrl, string dbId, string username, string password, Language lcid = Language.ChineseChina) : base(serverUrl, dbId, username, password, lcid) { }

		public Client(string serverUrl, int timeout, string dbId, string username, string password, Language lcid = Language.ChineseChina) : base(serverUrl, timeout, dbId, username, password, lcid) { }

		private void FormatFields(List<Field> fields, ArraySegment<object> data, JsonWriter writer) {
			if (fields.Count != data.Count)
				throw new Exception($"Count of {nameof(fields)} doesn't equal to that of {nameof(data)}");
			writer.WriteStartObject();
			for (int i = 0, delta; i < fields.Count; i += delta) {
				delta = 1;
				(var field, object datum) = (fields[i], data[i]);
				var stProp = field.StartingInfo;
				writer.WritePropertyName(stProp.GetJsonPropertyName());
				for (var k = 0; k < stProp.Rank; ++k)
					writer.WriteStartArray();
				if (field.Length == 1)
					writer.WriteValue(datum);
				else {
					for (int j = i + 1; j < fields.Count && fields[j].Names[0] == field.Names[0]; ++j, ++delta) { }
					FormatFields(
						fields.GetRange(i, delta).Select(f => f[1..]).AsList(),
						data[i..(i + delta)],
						writer
					);
				}
				for (var k = 0; k < stProp.Rank; ++k)
					writer.WriteEndArray();
			}
			writer.WriteEndObject();
		}

		private static IEnumerable<object> MergeForms(IEnumerable<object> forms) {
			var subFormInfo = forms.GetType().GetItemType().GetMemberWithAttribute<SubFormAttribute>();
			if (subFormInfo is null)
				return forms;
			var type = subFormInfo.GetValueType();
			var groups = forms.GroupBy(form => form.GetType().GetMemberWithAttribute<KeyAttribute>().GetValue(form));
			return groups.Select(
				group => {
					var list = group.AsList();
					var subForms = MergeForms(
						(type.Implements(typeof(ICollection<>))
							? list.SelectMany(form => (subFormInfo.GetValue(form) as IEnumerable)!.AsType<object>())
							: list.Select(form => subFormInfo.GetValue(form))).AsList()
					);
					if (type.Implements(typeof(ICollection<>))) {
						object propObj = subFormInfo.GetValue(list[0]);//ICollection<T>
						var collectionType = propObj.GetType().GetGenericInterface(typeof(ICollection<>));
						collectionType.GetMethod(nameof(ICollection<object>.Clear))!.Invoke(propObj, null);
						foreach (object subForm in subForms)
							collectionType.GetMethod(nameof(ICollection<object>.Add))!.Invoke(propObj, subForm);
						subFormInfo.SetValue(list[0], propObj);
					}
					else
						subFormInfo.SetValue(list.Single(), subForms.Single());
					return list[0];
				}
			);
		}

		private OneOf<BasicResponse, List<T>> DeserializeQueryResponse<T>(IEnumerable<Field> fields, string json) where T : ErpModelBase {
			var token = JToken.Parse(json);
			if (token.Type != JTokenType.Array)
				throw new JTokenTypeException(token, JTokenType.Array);
			var tuples = (token as JArray)!.Values<JArray>().ToArray();
			if (tuples.Length == 1 && tuples[0]!.Count == 1 && tuples[0][0].Type == JTokenType.Object)
				return tuples[0][0].ToObject<BasicResponse>();
			object[][] contents = tuples.Select(arr => arr.Values<JValue>().Select(v => v.Value).ToArray()).ToArray();
			var builder = new StringBuilder();
			var forms = contents.Select(
				data => {
					builder.Clear();
					FormatFields(fields.AsList(), data.AsArray(), new JsonTextWriter(new StringWriter(builder)));
					return JsonConvert.DeserializeObject<T>(builder.ToString());
				}
			);
			return MergeForms(forms).AsType<T>().AsList();
		}

		#region Sync Requests
		public List<DataCenter> GetDataCenters() => Execute<List<DataCenter>>("Kingdee.BOS.ServiceFacade.ServicesStub.Account.AccountService.GetDataCenterList");

		public string ExecuteOperation(string formId, string opNumber, string data) => Execute<string>("Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.ExecuteOperation", new object[] {formId, opNumber, data});

		/// <summary>
		///     单据查询
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		public OneOf<BasicResponse, List<T>> Query<T>(QueryRequest<T> request) where T : ErpModelBase {
			string json = ValidateAndExecute<QueryRequest<T>, string>(JsonConvert.SerializeObject(request));
			return DeserializeQueryResponse<T>(request.Fields, json);
		}

		/// <summary>
		///     保存
		/// </summary>
		/// <returns></returns>
		public SaveResponse Save<T>(SaveRequest<T> request) where T : ErpModelBase => ValidateAndExecute<SaveRequest<T>, SaveResponse, T>(request);

		/// <summary>
		///     批量保存
		/// </summary>
		/// <returns></returns>
		public BatchSaveResponse BatchSave<T>(BatchSaveRequest<T> request) where T : ErpModelBase => ValidateAndExecute<BatchSaveRequest<T>, BatchSaveResponse, T>(request);

		/// <summary>
		///     暂存
		/// </summary>
		/// <returns></returns>
		public SaveResponse Draft<T>(DraftRequest<T> request) where T : ErpModelBase => ValidateAndExecute<DraftRequest<T>, SaveResponse, T>(request);

		/// <summary>
		///     审核
		/// </summary>
		/// <returns></returns>
		public BasicResponse Audit<T>(AuditRequest<T> request) where T : ErpModelBase => ValidateAndExecute<AuditRequest<T>, BasicResponse, T>(request);

		/// <summary>
		///     反审核
		/// </summary>
		/// <returns></returns>
		public BasicResponse Unaudit<T>(UnauditRequest<T> request) where T : ErpModelBase => ValidateAndExecute<UnauditRequest<T>, BasicResponse, T>(request);

		/// <summary>
		///     删除
		/// </summary>
		/// <returns></returns>
		public BasicResponse Delete<T>(DeleteRequest<T> request) where T : ErpModelBase => ValidateAndExecute<DeleteRequest<T>, BasicResponse, T>(request);

		/// <summary>
		///     提交审核
		/// </summary>
		/// <returns></returns>
		public BasicResponse Submit<T>(SubmitRequest<T> request) where T : ErpModelBase => ValidateAndExecute<SubmitRequest<T>, BasicResponse, T>(request);

		/// <summary>
		///     查看
		/// </summary>
		/// <returns></returns>
		public string View(string formId, string data) => Execute<string>("Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.View", new object[] {formId, data});

		public string Allocate(string formId, string data) => Execute<string>("Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.Allocate", new object[] {formId, data});

		public string FlexSave(string formId, string data) => Execute<string>("Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.FlexSave", new object[] {formId, data});

		public string SendMsg(string data) => Execute<string>("Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.SendMsg", new object[] {data});

		/// <summary>
		///     下推
		/// </summary>
		/// <returns></returns>
		public string Push(string formId, string data) => Execute<string>("Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.Push", new object[] {formId, data});

		public string GroupSave(string formId, string data) => Execute<string>("Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.GroupSave", new object[] {formId, data});
		#endregion

		#region Async(Task) Requests
		public async Task<OneOf<BasicResponse, List<T>>> QueryAsync<T>(QueryRequest<T> request) where T : ErpModelBase {
			string json = await ValidateAndExecuteAsync<QueryRequest<T>, string>(JsonConvert.SerializeObject(request));
			return DeserializeQueryResponse<T>(request.Fields, json);
		}

		public Task<SaveResponse> SaveAsync<T>(SaveRequest<T> request) where T : ErpModelBase => ValidateAndExecuteAsync<SaveRequest<T>, SaveResponse, T>(request);

		public Task<BatchSaveResponse> BatchSaveAsync<T>(BatchSaveRequest<T> request) where T : ErpModelBase => ValidateAndExecuteAsync<BatchSaveRequest<T>, BatchSaveResponse, T>(request);

		public Task<BasicResponse> AuditAsync<T>(AuditRequest<T> request) where T : ErpModelBase => ValidateAndExecuteAsync<AuditRequest<T>, BasicResponse, T>(request);

		public Task<BasicResponse> UnauditAsync<T>(UnauditRequest<T> request) where T : ErpModelBase => ValidateAndExecuteAsync<UnauditRequest<T>, BasicResponse, T>(request);

		public Task<BasicResponse> DeleteAsync<T>(DeleteRequest<T> request) where T : ErpModelBase => ValidateAndExecuteAsync<DeleteRequest<T>, BasicResponse, T>(request);

		public async Task<BasicResponse> UnauditAndDeleteAsync<T>(DeleteRequest<T> request) where T : ErpModelBase {
			await UnauditAsync(new UnauditRequest<T>(request));
			return await DeleteAsync(request);
		}
		#endregion

		#region Asnyc(Callback) Requests
		public void GetDataCentersAsync(
			Action<List<DataCenter>> onSucceed,
			FailCallbackHandler onFail = null,
			ProgressChangedHandler onProgressChange = null,
			int reportInterval = 5
		) {
			ExecuteAsync("Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.GetDataCenterList", onSucceed, Array.Empty<object>(), onProgressChange, onFail, 10, reportInterval);
		}

		public void ExecuteOperationAsync(
			string formId,
			string opNumber,
			string data,
			Action<string> onSucceed,
			FailCallbackHandler onFail = null,
			ProgressChangedHandler onProgressChange = null,
			int reportInterval = 5
		) {
			ExecuteAsync("Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.ExecuteOperation", onSucceed, new object[] {formId, opNumber, data}, onProgressChange, onFail, 10, reportInterval);
		}

		public void QueryAsync<T>(
			QueryRequest<T> request,
			Action<OneOf<BasicResponse, List<T>>> onSucceed,
			FailCallbackHandler onFail = null,
			ProgressChangedHandler onProgressChange = null,
			int reportInterval = 5
		) where T : ErpModelBase {
			ExecuteAsync<string>(
				"Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.ExecuteBillQuery",
				json => onSucceed(DeserializeQueryResponse<T>(request.Fields, json)),
				new object[] {JsonConvert.SerializeObject(request)},
				onProgressChange,
				onFail,
				10,
				reportInterval
			);
		}

		public void SaveAsync<T>(
			SaveRequest<T> request,
			Action<SaveResponse> onSucceed,
			FailCallbackHandler onFail = null,
			ProgressChangedHandler onProgressChange = null,
			int reportInterval = 5
		) where T : ErpModelBase {
			ExecuteAsync<SaveResponse, T>(
				"Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.Save",
				request,
				onSucceed,
				onProgressChange,
				onFail,
				10,
				reportInterval
			);
		}

		public void BatchSaveAsync<T>(
			BatchSaveRequest<T> request,
			Action<BatchSaveResponse> onSucceed,
			FailCallbackHandler onFail = null,
			ProgressChangedHandler onProgressChange = null,
			int reportInterval = 5
		) where T : ErpModelBase {
			ExecuteAsync<BatchSaveResponse, T>(
				"Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.BatchSave",
				request,
				onSucceed,
				onProgressChange,
				onFail,
				10,
				reportInterval
			);
		}

		public void DraftAsync<T>(
			SaveRequest<T> request,
			Action<SaveResponse> onSucceed,
			FailCallbackHandler onFail = null,
			ProgressChangedHandler onProgressChange = null,
			int reportInterval = 5
		) where T : ErpModelBase {
			ExecuteAsync<SaveResponse, T>(
				"Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.Draft",
				request,
				onSucceed,
				onProgressChange,
				onFail,
				10,
				reportInterval
			);
		}

		public void AuditAsync<T>(
			AuditRequest<T> request,
			Action<BasicResponse> onSucceed,
			FailCallbackHandler onFail = null,
			ProgressChangedHandler onProgressChange = null,
			int reportInterval = 5
		) where T : ErpModelBase {
			ExecuteAsync<BasicResponse, T>(
				"Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.Audit",
				request,
				onSucceed,
				onProgressChange,
				onFail,
				10,
				reportInterval
			);
		}

		public void UnauditAsync<T>(
			AuditRequest<T> request,
			Action<BasicResponse> onSucceed,
			FailCallbackHandler onFail = null,
			ProgressChangedHandler onProgressChange = null,
			int reportInterval = 5
		) where T : ErpModelBase {
			ExecuteAsync<BasicResponse, T>(
				"Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.UnAudit",
				request,
				onSucceed,
				onProgressChange,
				onFail,
				10,
				reportInterval
			);
		}

		public void DeleteAsync<T>(
			DeleteRequest<T> request,
			Action<BasicResponse> onSucceed,
			FailCallbackHandler onFail = null,
			ProgressChangedHandler onProgressChange = null,
			int reportInterval = 5
		) where T : ErpModelBase {
			ExecuteAsync<BasicResponse, T>(
				"Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.Delete",
				request,
				onSucceed,
				onProgressChange,
				onFail,
				10,
				reportInterval
			);
		}

		public void SubmitAsync(string formId, string data, Action<string> onSucceed, FailCallbackHandler onFail = null, ProgressChangedHandler onProgressChange = null, int reportInterval = 5) {
			ExecuteAsync("Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.Submit", onSucceed, new object[] {formId, data}, onProgressChange, onFail, 10, reportInterval);
		}

		public void ViewAsync(string formId, string data, Action<string> onSucceed, FailCallbackHandler onFail = null, ProgressChangedHandler onProgressChange = null, int reportInterval = 5) {
			ExecuteAsync("Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.View", onSucceed, new object[] {formId, data}, onProgressChange, onFail, 10, reportInterval);
		}

		public void AllocateAsync(string formId, string data, Action<string> onSucceed, FailCallbackHandler onFail = null, ProgressChangedHandler onProgressChange = null, int reportInterval = 5) {
			ExecuteAsync("Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.Allocate", onSucceed, new object[] {formId, data}, onProgressChange, onFail, 10, reportInterval);
		}

		public void FlexSaveAsync(
			string formId,
			string data,
			Action<string> onSucceed,
			FailCallbackHandler onFail = null,
			ProgressChangedHandler onProgressChange = null,
			int reportInterval = 5
		) {
			ExecuteAsync("Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.FlexSave", onSucceed, new object[] {formId, data}, onProgressChange, onFail, 10, reportInterval);
		}

		public void SendMsgAsync(
			string data,
			Action<string> onSucceed,
			FailCallbackHandler onFail = null,
			ProgressChangedHandler onProgressChange = null,
			int reportInterval = 5
		) {
			ExecuteAsync("Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.SendMsg", onSucceed, new object[] {data}, onProgressChange, onFail, 10, reportInterval);
		}

		public void PushAsync(
			string formId,
			string data,
			Action<string> onSucceed,
			FailCallbackHandler onFail = null,
			ProgressChangedHandler onProgressChange = null,
			int reportInterval = 5
		) {
			ExecuteAsync("Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.Push", onSucceed, new object[] {formId, data}, onProgressChange, onFail, 10, reportInterval);
		}

		public void GroupSaveAsync(
			string formId,
			string data,
			Action<string> onSucceed,
			FailCallbackHandler onFail = null,
			ProgressChangedHandler onProgressChange = null,
			int reportInterval = 5
		) {
			ExecuteAsync("Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.GroupSave", onSucceed, new object[] {formId, data}, onProgressChange, onFail, 10, reportInterval);
		}
		#endregion
	}
}