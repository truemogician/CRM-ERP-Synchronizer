using System;
using Kingdee.Responses;
using Shared.Exceptions;

namespace Kingdee.Exceptions {
	#nullable enable
	public class LoginFailedException : ExceptionWithDefaultMessage {
		public LoginFailedException(string? message = null, Exception? innerException = null) : base(message, innerException) { }

		public LoginFailedException(LoginResponse response, string? message = null, Exception? innerException = null) : base(message, innerException) => Set(nameof(LoginResponse), response);

		public LoginResponse? LoginResponse => Get<LoginResponse>(nameof(LoginResponse));
	}
}