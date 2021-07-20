namespace FXiaoKe.Responses {
	public enum ErrorCode {
		/// <summary>
		///     非通用错误
		/// </summary>
		Unknown = int.MinValue,

		/// <summary>
		///     系统错误
		/// </summary>
		SystemError = -2,

		/// <summary>
		///     系统繁忙
		/// </summary>
		Busy = -1,

		/// <summary>
		///     请求成功
		/// </summary>
		Success = 0,

		/// <summary>
		///     缺少参数appId
		/// </summary>
		AppIdMissing = 10001,

		/// <summary>
		///     缺少参数appSecret
		/// </summary>
		AppSecretMissing = 10002,

		/// <summary>
		///     缺少参数appAccessToken
		/// </summary>
		AppAccessTokenMissing = 10003,

		/// <summary>
		///     缺少参数redirectUri
		/// </summary>
		RedirectUriMissing = 10004,

		/// <summary>
		///     缺少参数responseType
		/// </summary>
		ResponseTypeMissing = 10005,

		/// <summary>
		///     缺少参数scope
		/// </summary>
		ScopeMissing = 10006,

		/// <summary>
		///     缺少参数state
		/// </summary>
		StateMissing = 10007,

		/// <summary>
		///     缺少参数code
		/// </summary>
		CodeMissing = 10008,

		/// <summary>
		///     缺少参数appAccount
		/// </summary>
		AppAccountMissing = 10009,

		/// <summary>
		///     缺少参数openUserId
		/// </summary>
		OpenUserIdMissing = 10010,

		/// <summary>
		///     缺少参数permanentCode
		/// </summary>
		PermanentCodeMissing = 10012,

		/// <summary>
		///     缺少参数corpAccessToken
		/// </summary>
		CorpAccessTokenMissing = 10013,

		/// <summary>
		///     缺少参数corpId
		/// </summary>
		CorpIdMissing = 10014,

		/// <summary>
		///     缺少参数toUser
		/// </summary>
		ToUserMissing = 10015,

		/// <summary>
		///     参数appId不合法
		/// </summary>
		InvalidAppId = 11001,

		/// <summary>
		///     参数appSecret不合法
		/// </summary>
		InvalidAppSecret = 11002,

		/// <summary>
		///     参数appAccessToken不合法
		/// </summary>
		InvalidAppAccessToken = 11003,

		/// <summary>
		///     参数redirectUri不合法
		/// </summary>
		InvalidRedirectUri = 11004,

		/// <summary>
		///     参数responseType不合法
		/// </summary>
		InvalidResponseType = 11005,

		/// <summary>
		///     参数scope不合法
		/// </summary>
		InvalidScope = 11006,

		/// <summary>
		///     参数state不合法
		/// </summary>
		InvalidState = 11007,

		/// <summary>
		///     参数openUserId不合法
		/// </summary>
		InvalidOpenUserId = 11008,

		/// <summary>
		///     参数departmentId不合法
		/// </summary>
		InvalidDepartmentId = 11009,

		/// <summary>
		///     参数code不合法
		/// </summary>
		InvalidCode = 11010,

		/// <summary>
		///     参数appAccount不合法
		/// </summary>
		InvalidAppAccount = 11011,

		/// <summary>
		///     参数permanentCode不合法
		/// </summary>
		InvalidPermanentCode = 11013,

		/// <summary>
		///     参数corpAccessToken不合法
		/// </summary>
		InvalidCorpAccessToken = 11014,

		/// <summary>
		///     参数corpId不合法
		/// </summary>
		InvalidCorpId = 11015,

		/// <summary>
		///     参数toUser不合法
		/// </summary>
		InvalidToUser = 11016,

		/// <summary>
		///     参数msgType不合法
		/// </summary>
		InvalidMessageType = 11017,

		/// <summary>
		///     参数templateId不合法
		/// </summary>
		InvalidTemplateId = 11018,

		/// <summary>
		///     登录状态错误
		/// </summary>
		LogStatusError = 12002,

		/// <summary>
		///     未支持的消息类型
		/// </summary>
		UnsupportedMessageType = 12003,

		/// <summary>
		///     POST的数据包为空
		/// </summary>
		EmptyPostData = 12004,

		/// <summary>
		///     文本消息内容为空
		/// </summary>
		EmptyTextContent = 12005,

		/// <summary>
		///     接口调用超过限制
		/// </summary>
		CallLimitExceeded = 14001,

		/// <summary>
		///     参数不合法
		/// </summary>
		InvalidParameter = 15002,

		/// <summary>
		///     APP没有访问权限
		/// </summary>
		Unauthorized = 15003,

		/// <summary>
		///     accessToken不存在或者已经过期
		/// </summary>
		AccessTokenNotExistedOrExpired = 20005,

		/// <summary>
		///     appId或appSecret错误
		/// </summary>
		AppIdOrAppSecretError = 20006,

		/// <summary>
		///     CODE不存在或者已经过期
		/// </summary>
		CodeNotExistedOrExpired = 20010,

		/// <summary>
		///     openUserId未找到
		/// </summary>
		OpenUserIdNotFound = 20012,

		/// <summary>
		///     应用没有获取该员工的数据的权限
		/// </summary>
		UnauthorizedToStaff = 20014,

		/// <summary>
		///     永久授权码错误
		/// </summary>
		PermanentCodeError = 20015,

		/// <summary>
		///     corpAccessToken不存在或者已经过期
		/// </summary>
		CorpAccessTokenNotExistedOrExpired = 20016,

		/// <summary>
		///     corpId未找到
		/// </summary>
		CorpIdNotFound = 20017,

		/// <summary>
		///     应用没有获取该企业的数据的权限
		/// </summary>
		UnauthorizedToEnterpriseData = 20020,

		/// <summary>
		///     在当前企业下，该app的状态为停用
		/// </summary>
		AppSuspended = 20021,

		/// <summary>
		///     企业没有对该app授权
		/// </summary>
		AppNotAuthorized = 20022,

		/// <summary>
		///     APP没有访问department的权限
		/// </summary>
		UnauthorizedToDepartment = 20023,

		/// <summary>
		///     客户没有购买openapi配额，需要提open api订单进行购买
		/// </summary>
		OpenApiQuotaNotPurchased = 30003,

		/// <summary>
		///     部门不存在
		/// </summary>
		DepartmentNotExisted = 30007,

		/// <summary>
		///     员工不存在
		/// </summary>
		StaffNotExisted = 30027,

		/// <summary>
		///     参数错误
		/// </summary>
		InvalidArgument = 32000,

		/// <summary>
		///     templateId不合法
		/// </summary>
		IllegalTemplateId = 40010
	}
}