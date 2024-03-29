﻿using System;
using System.IO;
using System.Net;
using Kingdee.Exceptions;

namespace Kingdee {
	internal class HttpClient {
		public static string Send(ApiRequest request) {
			using (var requestStream = request.HttpRequest.GetRequestStream()) {
				byte[] bytes = request.Encoder.GetBytes(request.SerializeBody());
				requestStream.Write(bytes, 0, bytes.Length);
				requestStream.Flush();
			}
			using var responseStream = request.HttpRequest.GetResponse().GetResponseStream();
			using var streamReader = new StreamReader(responseStream!);
			return ValidateResult(streamReader.ReadToEnd());
		}

		public void SendAsync(
			ApiRequest request,
			Action<AsyncResult<string>> callback,
			Action afterSent = null
		) {
			lock (this) {
				var requestState = new RequestState(request, callback, afterSent);
				request.HttpRequest.BeginGetRequestStream(BeginGetRequestCallback, requestState);
			}
		}

		private void BeginGetRequestCallback(IAsyncResult ar) {
			var request = (RequestState)ar.AsyncState;
			request!.EndSendRequest(ar);
			request.SentCallback?.Invoke();
			request.Request.HttpRequest.BeginGetResponse(GetResponseCallback, ar.AsyncState);
		}

		private void GetResponseCallback(IAsyncResult ar) {
			var request = (RequestState)ar.AsyncState;
			var callback = request!.Callback;
			using var response = (HttpWebResponse)((RequestState)ar.AsyncState)!.Request.HttpRequest.EndGetResponse(ar);
			if (response.StatusCode == HttpStatusCode.OK) {
				using var responseStream = response.GetResponseStream();
				using var streamReader = new StreamReader(responseStream!, request.Request.Encoder);
				string result = ValidateResult(streamReader.ReadToEnd());
				callback(AsyncResult<string>.CreateSuccess(result));
			}
			else {
				var ex = new ServiceException((int)response.StatusCode, response.StatusDescription);
				callback(AsyncResult<string>.CreateFailure(ex));
			}
		}

		private static string ValidateResult(string responseText) {
			if (!responseText.StartsWith("response_error:"))
				return responseText;
			string str = responseText.TrimStart("response_error:".ToCharArray());
			if (string.IsNullOrEmpty(str))
				throw new ServiceException("返回的异常信息为空");
			throw new ServiceException(str.Replace("\"", ""));
		}

		private class RequestState {
			public RequestState(
				ApiRequest request,
				Action<AsyncResult<string>> callback,
				Action afterSent
			) {
				Request = request;
				Callback = callback;
				SentCallback = afterSent;
			}

			public Action SentCallback { get; }

			public ApiRequest Request { get; }

			public Action<AsyncResult<string>> Callback { get; }

			internal void EndSendRequest(IAsyncResult ar) {
				using var requestStream = Request.HttpRequest.EndGetRequestStream(ar);
				byte[] bytes = Request.Encoder.GetBytes(Request.SerializeBody());
				requestStream.Write(bytes, 0, bytes.Length);
				requestStream.Flush();
			}
		}
	}
}