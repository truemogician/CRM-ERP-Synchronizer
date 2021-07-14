using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace FXiaoKe.Response {
	public class BasicResponse : ResponseBase {
		/// <summary>
		///     返回码
		/// </summary>
		[JsonProperty("errorCode")]
		[Required]
		public int RawErrorCode { get; set; }

		public ErrorCode ErrorCode {
			get {
				try {
					return (ErrorCode)RawErrorCode;
				}
				catch (Exception) {
					return ErrorCode.Unknown;
				}
			}
			set => RawErrorCode = (int)value;
		}

		/// <summary>
		///     对返回码的文本描述内容
		/// </summary>
		[JsonProperty("errorMessage")]
		[Required]
		public ErrorCode ErrorMessage { get; set; }
	}
}