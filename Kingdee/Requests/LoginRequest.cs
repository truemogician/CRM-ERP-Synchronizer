using System;
using Kingdee.Utilities;
using Newtonsoft.Json;

namespace Kingdee.Requests {
	[Request("Kingdee.BOS.WebApi.ServicesStub.AuthService.ValidateUserEnDeCode")]
	public class LoginRequest : RequestBase {
		public string DatabaseId { get; set; }

		public string Username { get; set; }

		public string Password { get; set; }

		public Language Language { get; set; } = Language.ChineseChina;

		public static implicit operator object[](LoginRequest self) => new object[] {self.DatabaseId, EnDecode.Encode(self.Username), EnDecode.Encode(self.Password), (int)self.Language};
	}
}