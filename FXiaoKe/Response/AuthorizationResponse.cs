using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Shared.JsonConverters;

namespace FXiaoKe.Response {
	public class AuthorizationResponse : BasicResponse {
		/// <summary>
		///     企业应用访问公司合法性凭证
		/// </summary>
		[JsonProperty("corpAccessToken")]
		[Required]
		public string CorpAccessToken { get; set; }

		/// <summary>
		///     开放平台派发的公司帐号
		/// </summary>
		[JsonProperty("corpId")]
		[Required]
		public string CorpId { get; set; }

		/// <summary>
		///     企业应用访问公司合法性凭证的过期时间，单位为秒，取值在0~7200之间，在过期时间在0-6600之间请求该接口会返回相同的corpAccessToken，在6600-7200之间请求该接口会返回新的token，如果要续期token，则需要在该时刻进行请求。
		/// </summary>
		[JsonConverter(typeof(TimeSpanUnitConverter<int>), TimeSpanUnit.Second)]
		[JsonProperty("expiresIn")]
		[Required]
		public TimeSpan ExpiresIn { get; set; }
	}
}