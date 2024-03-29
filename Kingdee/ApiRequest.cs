﻿using System;
using System.IO;
using System.Net;
using System.Text;
using Kingdee.Utilities;
using Newtonsoft.Json.Linq;

namespace Kingdee {
	public class ApiRequest {
		private static bool HasSetSecurityProtocol;

		private HttpWebRequest _httpRequest;

		public ApiRequest(string serverUrl, bool async, Encoding encoder, CookieContainer cookies) {
			ServerUrl = serverUrl;
			Encoder = encoder;
			CookiesContainer = cookies;
			RequestId = Guid.NewGuid().ToString().GetHashCode().ToString();
			AutoGetLastProgress = true;
			IsAsync = async;
			Version = "1.0";
			Body["format"] = 1;
			Body["useragent"] = "ApiClient";
		}

		public bool IsAsync { get; }

		public CookieContainer CookiesContainer { get; }

		public Encoding Encoder { get; }

		public string ServerUrl { get; }

		public virtual Uri ServiceUri => throw new NotImplementedException();

		public string RequestId { get; protected set; }

		public bool AutoGetLastProgress { get; set; }

		public string Version { get; set; }

		public JObject Body { get; } = new();

		internal HttpWebRequest HttpRequest {
			get {
				lock (this)
					return _httpRequest ??= CreateRequest();
			}
		}

		public static void SetSecurityProtocol(Uri uri) {
			if (!HasSetSecurityProtocol)
				try {
					if (uri.Scheme.Equals("https", StringComparison.CurrentCultureIgnoreCase))
						ServicePointManager.SecurityProtocol = (SecurityProtocolType)4080;
				}
				catch (Exception ex) {
					Console.WriteLine(ex.Message);
				}
				finally {
					HasSetSecurityProtocol = true;
				}
			ServicePointManager.ServerCertificateValidationCallback = ServicePointManager.ServerCertificateValidationCallback = (_, _, _, _) => true;
		}

		public virtual string SerializeBody() {
			Body["v"] = Version;
			Body["timestamp"] = DateTime.Now;
			return Body.ToString();
		}

		private HttpWebRequest CreateRequest() {
			var httpWebRequest = WebRequestHelper.Create(ServiceUri);
			httpWebRequest.Method = "POST";
			httpWebRequest.KeepAlive = true;
			httpWebRequest.ContentType = "application/json";
			httpWebRequest.Headers.Add("Accept-Charset", Encoder.HeaderName);
			httpWebRequest.CookieContainer = CookiesContainer;
			httpWebRequest.Pipelined = true;
			return httpWebRequest;
		}

		public void Abort() => _httpRequest.Abort();

		public static string GetCerFile() {
			string getCerFile;
			if (Environment.UserInteractive) {
				getCerFile = TryToGetCerFile(AppDomain.CurrentDomain.BaseDirectory);
				if (!File.Exists(getCerFile))
					getCerFile = TryToGetCerFile(new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).Parent?.FullName);
			}
			else {
				getCerFile = TryToGetCerFile(AppDomain.CurrentDomain.BaseDirectory);
				if (!File.Exists(getCerFile))
					getCerFile = TryToGetCerFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data"));
			}
			return getCerFile;
		}

		private static string TryToGetCerFile(string dirName) {
			string path = Path.Combine(dirName, Environment.MachineName + ".cer");
			return File.Exists(path) ? path : Path.Combine(dirName, "K3CloudCert.cer");
		}

		[Flags]
		internal enum SecurityProtocolTypeEnum {
			Ssl3 = 48,// 0x00000030

			Tls = 192,// 0x000000C0

			Tls11 = 768,// 0x00000300

			Tls12 = 3072// 0x00000C00
		}
	}
}