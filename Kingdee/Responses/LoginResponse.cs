// ReSharper disable StringLiteralTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Kingdee.Responses {
	public class LoginResponse {
		[JsonProperty("Message")]
		public object Message { get; set; }

		[JsonProperty("MessageCode")]
		public string MessageCode { get; set; }

		[JsonProperty("LoginResultType")]
		public int LoginResultType { get; set; }

		[JsonProperty("Context")]
		public Context Context { get; set; }

		[JsonProperty("KDSVCSessionId")]
		public string KDSVCSessionId { get; set; }

		[JsonProperty("FormId")]
		public object FormId { get; set; }

		[JsonProperty("RedirectFormParam")]
		public object RedirectFormParam { get; set; }

		[JsonProperty("FormInputObject")]
		public object FormInputObject { get; set; }

		[JsonProperty("ErrorStackTrace")]
		public object ErrorStackTrace { get; set; }

		[JsonProperty("Lcid")]
		public Language Lcid { get; set; }

		[JsonProperty("AccessToken")]
		public object AccessToken { get; set; }

		[JsonProperty("KdAccessResult")]
		public object KdAccessResult { get; set; }

		[JsonProperty("IsSuccessByAPI")]
		public bool IsSuccessByApi { get; set; }

		public static implicit operator bool(LoginResponse self) => self.LoginResultType == 1;

		public static bool operator true(LoginResponse self) => self;

		public static bool operator false(LoginResponse self) => !(bool)self;
	}

	public class UseLanguage {
		[JsonProperty("LocaleId")]
		public int LocaleId { get; set; }

		[JsonProperty("LocaleName")]
		public string LocaleName { get; set; }

		[JsonProperty("Alias")]
		public string Alias { get; set; }
	}

	public class CurrentOrganizationInfo {
		[JsonProperty("ID")]
		public int Id { get; set; }

		[JsonProperty("AcctOrgType")]
		public string AcctOrgType { get; set; }

		[JsonProperty("Name")]
		public string Name { get; set; }

		[JsonProperty("FunctionIds")]
		public List<int> FunctionIds { get; set; }
	}

	public class EncoderFallback {
		[JsonProperty("DefaultString")]
		public string DefaultString { get; set; }

		[JsonProperty("MaxCharCount")]
		public int MaxCharCount { get; set; }
	}

	public class DecoderFallback {
		[JsonProperty("DefaultString")]
		public string DefaultString { get; set; }

		[JsonProperty("MaxCharCount")]
		public int MaxCharCount { get; set; }
	}

	public class Charset {
		[JsonProperty("BodyName")]
		public string BodyName { get; set; }

		[JsonProperty("EncodingName")]
		public string EncodingName { get; set; }

		[JsonProperty("HeaderName")]
		public string HeaderName { get; set; }

		[JsonProperty("WebName")]
		public string WebName { get; set; }

		[JsonProperty("WindowsCodePage")]
		public int WindowsCodePage { get; set; }

		[JsonProperty("IsBrowserDisplay")]
		public bool IsBrowserDisplay { get; set; }

		[JsonProperty("IsBrowserSave")]
		public bool IsBrowserSave { get; set; }

		[JsonProperty("IsMailNewsDisplay")]
		public bool IsMailNewsDisplay { get; set; }

		[JsonProperty("IsMailNewsSave")]
		public bool IsMailNewsSave { get; set; }

		[JsonProperty("IsSingleByte")]
		public bool IsSingleByte { get; set; }

		[JsonProperty("EncoderFallback")]
		public EncoderFallback EncoderFallback { get; set; }

		[JsonProperty("DecoderFallback")]
		public DecoderFallback DecoderFallback { get; set; }

		[JsonProperty("IsReadOnly")]
		public bool IsReadOnly { get; set; }

		[JsonProperty("CodePage")]
		public int CodePage { get; set; }
	}

	public class WeiboAuthInfo {
		[JsonProperty("WeiboUrl")]
		public object WeiboUrl { get; set; }

		[JsonProperty("NetWorkID")]
		public object NetWorkID { get; set; }

		[JsonProperty("CompanyNetworkID")]
		public object CompanyNetworkID { get; set; }

		[JsonProperty("Account")]
		public object Account { get; set; }

		[JsonProperty("AppKey")]
		public string AppKey { get; set; }

		[JsonProperty("AppSecret")]
		public string AppSecret { get; set; }

		[JsonProperty("TokenKey")]
		public object TokenKey { get; set; }

		[JsonProperty("TokenSecret")]
		public object TokenSecret { get; set; }

		[JsonProperty("Verify")]
		public object Verify { get; set; }

		[JsonProperty("CallbackUrl")]
		public object CallbackUrl { get; set; }

		[JsonProperty("UserId")]
		public object UserId { get; set; }

		[JsonProperty("Charset")]
		public Charset Charset { get; set; }
	}

	public class UTimeZone {
		[JsonProperty("OffsetTicks")]
		public long OffsetTicks { get; set; }

		[JsonProperty("StandardName")]
		public string StandardName { get; set; }

		[JsonProperty("Id")]
		public int Id { get; set; }

		[JsonProperty("Number")]
		public string Number { get; set; }

		[JsonProperty("CanBeUsed")]
		public bool CanBeUsed { get; set; }
	}

	public class STimeZone {
		[JsonProperty("OffsetTicks")]
		public long OffsetTicks { get; set; }

		[JsonProperty("StandardName")]
		public string StandardName { get; set; }

		[JsonProperty("Id")]
		public int Id { get; set; }

		[JsonProperty("Number")]
		public string Number { get; set; }

		[JsonProperty("CanBeUsed")]
		public bool CanBeUsed { get; set; }
	}

	public class Context {
		[JsonProperty("UserLocale")]
		public string UserLocale { get; set; }

		[JsonProperty("LogLocale")]
		public string LogLocale { get; set; }

		[JsonProperty("DBid")]
		public string DBid { get; set; }

		[JsonProperty("DatabaseType")]
		public int DatabaseType { get; set; }

		[JsonProperty("SessionId")]
		public string SessionId { get; set; }

		[JsonProperty("UseLanguages")]
		public List<UseLanguage> UseLanguages { get; set; }

		[JsonProperty("UserId")]
		public int UserId { get; set; }

		[JsonProperty("UserName")]
		public string UserName { get; set; }

		[JsonProperty("CustomName")]
		public string CustomName { get; set; }

		[JsonProperty("DisplayVersion")]
		public string DisplayVersion { get; set; }

		[JsonProperty("DataCenterName")]
		public string DataCenterName { get; set; }

		[JsonProperty("UserToken")]
		public string UserToken { get; set; }

		[JsonProperty("CurrentOrganizationInfo")]
		public CurrentOrganizationInfo CurrentOrganizationInfo { get; set; }

		[JsonProperty("IsCH_ZH_AutoTrans")]
		public bool IsCHZHAutoTrans { get; set; }

		[JsonProperty("ClientType")]
		public int ClientType { get; set; }

		[JsonProperty("WeiboAuthInfo")]
		public WeiboAuthInfo WeiboAuthInfo { get; set; }

		[JsonProperty("UTimeZone")]
		public UTimeZone UTimeZone { get; set; }

		[JsonProperty("STimeZone")]
		public STimeZone STimeZone { get; set; }

		[JsonProperty("GDCID")]
		public string GDCID { get; set; }

		[JsonProperty("Gsid")]
		public object Gsid { get; set; }

		[JsonProperty("TRLevel")]
		public int TRLevel { get; set; }

		[JsonProperty("ProductEdition")]
		public int ProductEdition { get; set; }
	}
}