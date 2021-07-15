using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.VisualBasic.CompilerServices;
using Newtonsoft.Json;

namespace FXiaoKe.Response {
	public class BasicResponse : ResponseBase {
		/// <summary>
		///     返回码
		/// </summary>
		[JsonProperty("errorCode")]
		[Required]
		public int RawErrorCode { get; set; }

		[JsonIgnore]
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
		public string ErrorMessage { get; set; }

		public static bool operator true(BasicResponse resp) => resp;

		public static bool operator false(BasicResponse resp) => !(bool)resp;

		public static implicit operator bool(BasicResponse resp) => resp is not null && resp.ErrorCode == ErrorCode.Success;
	}
}