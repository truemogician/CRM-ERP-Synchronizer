using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Kingdee.Utilities {
	public static class EnDecode {
		public static string Encode(object data) {
			const string s1 = "KingdeeK";
			const string s2 = "KingdeeK";
			try {
				byte[] bytes1 = Encoding.ASCII.GetBytes(s1);
				byte[] bytes2 = Encoding.ASCII.GetBytes(s2);
				using var cryptoServiceProvider = new DESCryptoServiceProvider();
				using var memoryStream = new MemoryStream();
				using var cryptoStream = new CryptoStream(memoryStream, cryptoServiceProvider.CreateEncryptor(bytes1, bytes2), CryptoStreamMode.Write);
				using var streamWriter = new StreamWriter(cryptoStream);
				streamWriter.Write(data);
				streamWriter.Flush();
				cryptoStream.FlushFinalBlock();
				streamWriter.Flush();
				byte[] inArray = memoryStream.GetBuffer();
				var length = (int)memoryStream.Length;
				return Convert.ToBase64String(inArray, 0, length);
			}
			catch (Exception ex) {
				return ex.Message;
			}
		}
	}
}