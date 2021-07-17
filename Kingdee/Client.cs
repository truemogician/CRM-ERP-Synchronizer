using System;
using System.Collections.Generic;
using Kingdee.Forms;
using Kingdee.Requests;
using Kingdee.Requests.Query;
using Newtonsoft.Json;

namespace Kingdee {
	public class Client : ApiClient {
		public Client(string serverUrl)
			: base(serverUrl) { }

		public Client(string serverUrl, int timeout)
			: base(serverUrl, timeout) {}

		public List<DataCenter> GetDataCenters() => Execute<List<DataCenter>>("Kingdee.BOS.ServiceFacade.ServicesStub.Account.AccountService.GetDataCenterList", Array.Empty<object>());

		public string ExecuteOperation(string formId, string opNumber, string data) => Execute<string>("Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.ExecuteOperation", new object[] {formId, opNumber, data});

		/// <summary>
		/// 保存
		/// </summary>
		/// <param name="formId"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		public string Save(string formId, string data) => Execute<string>("Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.Save", formId, data);

		/// <summary>
		/// 批量保存
		/// </summary>
		/// <param name="formId"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		public string BatchSave(string formId, string data) => Execute<string>("Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.BatchSave", formId, data);

		/// <summary>
		/// 审核
		/// </summary>
		/// <param name="formId"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		public string Audit(string formId, string data) => Execute<string>("Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.Audit", formId, data);

		/// <summary>
		/// 删除
		/// </summary>
		/// <param name="formId"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		public string Delete(string formId, string data) => Execute<string>("Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.Delete", formId, data);

		/// <summary>
		/// 反审核
		/// </summary>
		/// <param name="formId"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		public string Unaudit(string formId, string data) => Execute<string>("Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.UnAudit", formId, data);

		/// <summary>
		/// 提交
		/// </summary>
		/// <param name="formId"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		public string Submit(string formId, string data) => Execute<string>("Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.Submit", formId, data);

		/// <summary>
		/// 查看
		/// </summary>
		/// <param name="formId"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		public string View(string formId, string data) => Execute<string>("Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.View", formId, data);

		/// <summary>
		/// 单据查询
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		public List<List<object>> Query<T>(QueryRequest<T> request) where T : FormBase => Execute<List<List<object>>>("Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.ExecuteBillQuery", JsonConvert.SerializeObject(request));

		/// <summary>
		/// 暂存
		/// </summary>
		/// <param name="formId"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		public string Draft(string formId, string data) => Execute<string>("Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.Draft", formId, data);

		public string Allocate(string formId, string data) => Execute<string>("Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.Allocate", formId, data);

		public string FlexSave(string formId, string data) => Execute<string>("Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.FlexSave", formId, data);

		public string SendMsg(string data) => Execute<string>("Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.SendMsg", data);

		/// <summary>
		/// 下推
		/// </summary>
		/// <param name="formId"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		public string Push(string formId, string data) => Execute<string>("Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.Push", formId, data);

		public string GroupSave(string formId, string data) => Execute<string>("Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.GroupSave", formId, data);

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

		public void SaveAsync(string formId, string data, Action<string> onSucceed, FailCallbackHandler onFail = null, ProgressChangedHandler onProgressChange = null, int reportInterval = 5) {
			ExecuteAsync("Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.Save", onSucceed, new object[] {formId, data}, onProgressChange, onFail, 10, reportInterval);
		}

		public void BatchSaveAsync(string formId, string data, Action<string> onSucceed, FailCallbackHandler onFail = null, ProgressChangedHandler onProgressChange = null, int reportInterval = 5) {
			ExecuteAsync("Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.BatchSave", onSucceed, new object[] {formId, data}, onProgressChange, onFail, 10, reportInterval);
		}

		public void AuditAsync(string formId, string data, Action<string> onSucceed, FailCallbackHandler onFail = null, ProgressChangedHandler onProgressChange = null, int reportInterval = 5) {
			ExecuteAsync("Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.Audit", onSucceed, new object[] {formId, data}, onProgressChange, onFail, 10, reportInterval);
		}

		public void DeleteAsync(string formId, string data, Action<string> onSucceed, FailCallbackHandler onFail = null, ProgressChangedHandler onProgressChange = null, int reportInterval = 5) {
			ExecuteAsync("Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.Delete", onSucceed, new object[] {formId, data}, onProgressChange, onFail, 10, reportInterval);
		}

		public void UnauditAsync(string formId, string data, Action<string> onSucceed, FailCallbackHandler onFail = null, ProgressChangedHandler onProgressChange = null, int reportInterval = 5) {
			ExecuteAsync("Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.UnAudit", onSucceed, new object[] {formId, data}, onProgressChange, onFail, 10, reportInterval);
		}

		public void SubmitAsync(string formId, string data, Action<string> onSucceed, FailCallbackHandler onFail = null, ProgressChangedHandler onProgressChange = null, int reportInterval = 5) {
			ExecuteAsync("Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.Submit", onSucceed, new object[] {formId, data}, onProgressChange, onFail, 10, reportInterval);
		}

		public void ViewAsync(string formId, string data, Action<string> onSucceed, FailCallbackHandler onFail = null, ProgressChangedHandler onProgressChange = null, int reportInterval = 5) {
			ExecuteAsync("Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.View", onSucceed, new object[] {formId, data}, onProgressChange, onFail, 10, reportInterval);
		}

		public void QueryAsync(
			string data,
			Action<List<List<object>>> onSucceed,
			FailCallbackHandler onFail = null,
			ProgressChangedHandler onProgressChange = null,
			int reportInterval = 5
		) {
			ExecuteAsync("Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.ExecuteBillQuery", onSucceed, new object[] {data}, onProgressChange, onFail, 10, reportInterval);
		}

		public void DraftAsync(string formId, string data, Action<string> onSucceed, FailCallbackHandler onFail = null, ProgressChangedHandler onProgressChange = null, int reportInterval = 5) {
			ExecuteAsync("Kingdee.BOS.WebApi.ServicesStub.DynamicFormService.Draft", onSucceed, new object[] {formId, data}, onProgressChange, onFail, 10, reportInterval);
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
	}
}