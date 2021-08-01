using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;

namespace Kingdee {
	public class WebRequestHelper {
		[Flags]
		public enum SecurityProtocolTypeEnum {
			Ssl3 = 48,  // 0x00000030
			Tls = 192,  // 0x000000C0
			Tls11 = 768,// 0x00000300
			Tls12 = 3072// 0x00000C00
		}

		private static bool HasSetSecurityProtocol;

		private static void SetSecurityProtocol(Uri uri) {
			if (!HasSetSecurityProtocol)
				try {
					if (uri.Scheme.Equals("https", StringComparison.CurrentCultureIgnoreCase))
						ServicePointManager.SecurityProtocol = (SecurityProtocolType)4080;
				}
				catch (Exception) {
					// ignored
				}
				finally {
					HasSetSecurityProtocol = true;
				}
			ServicePointManager.ServerCertificateValidationCallback = ServicePointManager.ServerCertificateValidationCallback = (_, _, _, _) => true;
		}

		public static HttpWebRequest Create(string uri) => Create(new Uri(uri));

		public static HttpWebRequest Create(Uri uri) {
			if (!HasSetSecurityProtocol)
				SetSecurityProtocol(uri);
			var httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
			httpWebRequest.UserAgent = $"Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.123; WOW64; Trident/5.0; .NET4.0E; Kingdee/{typeof(WebRequestHelper).Assembly.FullName} MANM)";
			string uriString = ConfigurationManager.AppSettings.Get("ProxyHost");
			if (uriString != null && Uri.TryCreate(uriString, UriKind.RelativeOrAbsolute, out var result))
				httpWebRequest.Proxy = new WebProxy(result);
			if (uri.Scheme.Equals("https", StringComparison.CurrentCultureIgnoreCase)) {
				string cerFile = GetCerFile();
				if (File.Exists(cerFile)) {
					var x509Certificate = new X509Certificate(cerFile);
					httpWebRequest.ClientCertificates.Add(x509Certificate);
				}
				else {
					var x509Store = new X509Store(StoreName.My);
					if (x509Store.Certificates.Count == 1)
						httpWebRequest.ClientCertificates.Add(x509Store.Certificates[0]);
					else if (x509Store.Certificates.Count > 0) {
						var certificate2Collection = x509Store.Certificates.Find(X509FindType.FindBySubjectName, Environment.MachineName, true);
						if (certificate2Collection.Count > 0)
							httpWebRequest.ClientCertificates.Add(certificate2Collection[0]);
						httpWebRequest.ClientCertificates.Add(x509Store.Certificates[0]);
					}
				}
			}
			return httpWebRequest;
		}

		private static string GetCerFile() {
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

		public static bool PortIsOpen(string hostName, int port) {
			try {
				var tcpClient = new TcpClient {
					SendTimeout = 20000000,
					ReceiveTimeout = 20000000
				};
				tcpClient.Connect(hostName, port);
				if (!tcpClient.Connected)
					return false;
				tcpClient.Close();
				return true;
			}
			catch {
				return false;
			}
		}
	}
}