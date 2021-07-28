using Newtonsoft.Json;

namespace FXiaoKe.Models {
	public class ImageInfo {
		[JsonProperty("path")]
		public string Id { get; set; }

		[JsonProperty("ext")]
		public string FileExtension { get; set; }

		[JsonProperty("filename")]
		public string FileName { get; set; }
	}
}