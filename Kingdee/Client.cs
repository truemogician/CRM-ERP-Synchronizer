using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using Kingdee.Forms;
using Kingdee.Requests;
using Kingdee.Responses;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OneOf;
using Shared.Exceptions;
using Shared.Serialization;
using Shared.Utilities;

namespace Kingdee {
	public class Client : ApiClient {
		public Client(string serverUrl) : base(serverUrl) { }

		public Client(string serverUrl, int timeout) : base(serverUrl, timeout) { }

		private void FormatFields(List<Field> fields, ArraySegment<object> data, JsonWriter writer) {
			if (fields.Count != data.Count)
				throw new Exception($"Count of {nameof(fields)} doesn't equal to that of {nameof(data)}");
			writer.WriteStartObject();
			for (int i = 0, delta; i < fields.Count; i += delta) {
				delta = 1;
				(var field, object datum) = (fields[i], data[i]);
				var stProp = field.StartingInfo;
				writer.WritePropertyName(stProp.GetJsonPropertyName());
				for (int k = 0; k < stProp.Rank; ++k)
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
				for (int k = 0; k < stProp.Rank; ++k)
					writer.WriteEndArray();
			}
			writer.WriteEndObject();
		}

		private List<object> MergeForms(List<object> forms) {
			var subFormInfo = forms[0].GetType().GetMemberWithAttribute<SubFormAttribute>();
			if (subFormInfo is null)
				return forms;
			var type = subFormInfo.GetValueType();
			var groups = forms.GroupBy(form => form.GetType().GetMemberWithAttribute<KeyAttribute>().GetValue(form));
			return groups.Select(
					group => {
						var list = group.AsList();
						var subForms = MergeForms(
							(type.Implements(typeof(IList))
								? list.SelectMany(form => (subFormInfo.GetValue(form) as IList)!.OfType<object>())
								: list.Select(form => subFormInfo.GetValue(form))).AsList()
						);
						if (type.Implements(typeof(IList))) {
							var propObj = subFormInfo.GetValue(list[0]) as IList;
							propObj!.Clear();
							foreach (object subForm in subForms)
								propObj.Add(subForm);
							subFormInfo.SetValue(list[0], propObj);
						}
						else
							subFormInfo.SetValue(list.Single(), subForms.Single());
						return list[0];
					}
				)
				.AsList();
		}

		#region Sync Requests
		public List<DataCenter> GetDataCenters() => Execute<List<DataCenter>>("Kingdee.BOS.ServiceFacade.ServicesStub.Account.AccountService.GetDataCenterList", Array.Empty<object>());

		public string ExecuteOperation(string formId, string opNumber, string data) => Execute<string>("Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.ExecuteOperation", formId, opNumber, data);

		/// <summary>
		///     单据查询
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		public OneOf<BasicResponse, List<T>> Query<T>(QueryRequest<T> request) where T : FormBase {
			string json = Execute<string>(
				"Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.ExecuteBillQuery",
				JsonConvert.SerializeObject(request)
			);
			var token = JToken.Parse(json);
			if (token.Type == JTokenType.Object)
				return token.Value<BasicResponse>();
			var contents = token is JArray array
				? array.Values<JArray>().Select(arr => arr.Values<JValue>().Select(v => v.Value))
				: throw new JTokenTypeException(token, JTokenType.Array);
			var builder = new StringBuilder();
			var forms = contents.Select(
				data => {
					builder.Clear();
					FormatFields(request.Fields.AsList(), data.AsArray(), new JsonTextWriter(new StringWriter(builder)));
					return JsonConvert.DeserializeObject<T>(builder.ToString());
				}
			);
			return MergeForms(forms.OfType<object>().AsList()).Cast<T>().AsList();
		}

		/// <summary>
		///     保存
		/// </summary>
		/// <returns></returns>
		public SaveResponse Save<T>(SaveRequest<T> request) where T : FormBase => Execute<SaveResponse, T>("Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.Save", request);

		/// <summary>
		///     批量保存
		/// </summary>
		/// <returns></returns>
		public BatchSaveResponse BatchSave<T>(BatchSaveRequest<T> request) where T : FormBase => Execute<BatchSaveResponse, T>("Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.BatchSave", request);

		/// <summary>
		///     暂存
		/// </summary>
		/// <returns></returns>
		public SaveResponse Draft<T>(SaveRequest<T> request) where T : FormBase => Execute<SaveResponse, T>("Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.Draft", request);

		/// <summary>
		///     审核
		/// </summary>
		/// <returns></returns>
		public BasicResponse Audit<T>(AuditRequest<T> request) where T : FormBase => Execute<BasicResponse, T>("Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.Audit", request);

		/// <summary>
		///     反审核
		/// </summary>
		/// <returns></returns>
		public BasicResponse Unaudit<T>(AuditRequest<T> request) where T : FormBase => Execute<BasicResponse, T>("Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.UnAudit", request);

		/// <summary>
		///     删除
		/// </summary>
		/// <returns></returns>
		public BasicResponse Delete<T>(DeleteRequest<T> request) where T : FormBase => Execute<BasicResponse, T>("Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.Delete", request);

		/// <summary>
		///     提交
		/// </summary>
		/// <param name="formId"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		public string Submit(string formId, string data) => Execute<string>("Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.Submit", formId, data);

		/// <summary>
		///     查看
		/// </summary>
		/// <param name="formId"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		public string View(string formId, string data) => Execute<string>("Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.View", formId, data);

		public string Allocate(string formId, string data) => Execute<string>("Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.Allocate", formId, data);

		public string FlexSave(string formId, string data) => Execute<string>("Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.FlexSave", formId, data);

		public string SendMsg(string data) => Execute<string>("Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.SendMsg", data);

		/// <summary>
		///     下推
		/// </summary>
		/// <param name="formId"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		public string Push(string formId, string data) => Execute<string>("Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.Push", formId, data);

		public string GroupSave(string formId, string data) => Execute<string>("Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.GroupSave", formId, data);
		#endregion

		#region Async Requests
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
		) where T : FormBase {
			ExecuteAsync<string>(
				"Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.ExecuteBillQuery",
				json => {
					var token = JToken.Parse(json);
					if (token.Type == JTokenType.Object)
						onSucceed(token.Value<BasicResponse>());
					else {
						var contents = token.Value<List<List<object>>>();
						onSucceed(contents?.Select(data => FormMeta<T>.CreateFromQueryFields(request.Fields, data)).ToList());
					}
				},
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
		) where T : FormBase {
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
		) where T : FormBase {
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
		) where T : FormBase {
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
		) where T : FormBase {
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
		) where T : FormBase {
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
		) where T : FormBase {
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