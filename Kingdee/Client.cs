using System;
using System.Collections.Generic;
using Kingdee.Forms;
using Kingdee.Requests;
using Newtonsoft.Json;

namespace Kingdee {
	public class Client : ApiClient {
		public Client(string serverUrl) : base(serverUrl) { }

		public Client(string serverUrl, int timeout) : base(serverUrl, timeout) { }

		#region Sync Requests
		public List<DataCenter> GetDataCenters() => Execute<List<DataCenter>>("Kingdee.BOS.ServiceFacade.ServicesStub.Account.AccountService.GetDataCenterList", Array.Empty<object>());

		public string ExecuteOperation(string formId, string opNumber, string data) => Execute<string>("Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.ExecuteOperation", formId, opNumber, data);

		/// <summary>
		///     单据查询
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		public List<List<object>> Query<T>(QueryRequest<T> request) where T : FormBase
			=> Execute<List<List<object>>>(
				"Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.ExecuteBillQuery",
				JsonConvert.SerializeObject(request)
			);

		/// <summary>
		///     保存
		/// </summary>
		/// <returns></returns>
		public string Save<T>(SaveRequest<T> request) where T : FormBase => Execute<string, T>("Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.Save", request);

		/// <summary>
		///     批量保存
		/// </summary>
		/// <returns></returns>
		public string BatchSave<T>(BatchSaveRequest<T> request) where T : FormBase => Execute<string, T>("Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.BatchSave", request);

		/// <summary>
		///     暂存
		/// </summary>
		/// <returns></returns>
		public string Draft<T>(SaveRequest<T> request) where T : FormBase => Execute<string, T>("Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.Draft", request);

		/// <summary>
		///     审核
		/// </summary>
		/// <returns></returns>
		public string Audit<T>(AuditRequest<T> request) where T : FormBase => Execute<string, T>("Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.Audit", request);

		/// <summary>
		///     反审核
		/// </summary>
		/// <returns></returns>
		public string Unaudit<T>(AuditRequest<T> request) where T : FormBase => Execute<string, T>("Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.UnAudit", request);

		/// <summary>
		///     删除
		/// </summary>
		/// <returns></returns>
		public string Delete<T>(DeleteRequest<T> request) where T : FormBase => Execute<string, T>("Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.Delete", request);

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
			Action<List<List<object>>> onSucceed,
			FailCallbackHandler onFail = null,
			ProgressChangedHandler onProgressChange = null,
			int reportInterval = 5
		) where T : FormBase {
			ExecuteAsync(
				"Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.ExecuteBillQuery",
				onSucceed,
				new object[] {JsonConvert.SerializeObject(request)},
				onProgressChange,
				onFail,
				10,
				reportInterval
			);
		}

		public void SaveAsync<T>(
			SaveRequest<T> request,
			Action<string> onSucceed,
			FailCallbackHandler onFail = null,
			ProgressChangedHandler onProgressChange = null,
			int reportInterval = 5
		) where T : FormBase {
			ExecuteAsync<string, T>(
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
			Action<string> onSucceed,
			FailCallbackHandler onFail = null,
			ProgressChangedHandler onProgressChange = null,
			int reportInterval = 5
		) where T : FormBase {
			ExecuteAsync<string, T>(
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
			Action<string> onSucceed,
			FailCallbackHandler onFail = null,
			ProgressChangedHandler onProgressChange = null,
			int reportInterval = 5
		) where T : FormBase {
			ExecuteAsync<string, T>(
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
			Action<string> onSucceed,
			FailCallbackHandler onFail = null,
			ProgressChangedHandler onProgressChange = null,
			int reportInterval = 5
		) where T : FormBase {
			ExecuteAsync<string, T>(
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
			Action<string> onSucceed,
			FailCallbackHandler onFail = null,
			ProgressChangedHandler onProgressChange = null,
			int reportInterval = 5
		) where T : FormBase {
			ExecuteAsync<string, T>(
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
			Action<string> onSucceed,
			FailCallbackHandler onFail = null,
			ProgressChangedHandler onProgressChange = null,
			int reportInterval = 5
		) where T : FormBase {
			ExecuteAsync<string, T>(
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