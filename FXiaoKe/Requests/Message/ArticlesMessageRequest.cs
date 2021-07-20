using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Shared.Utilities;

namespace FXiaoKe.Requests.Message {
	public class ArticlesMessageRequest : MessageRequest {
		public ArticlesMessageRequest() { }
		public ArticlesMessageRequest(params Article[] articles) => Articles = articles.AsList();
		public override MessageType Type => MessageType.Articles;

		[JsonProperty("articles")]
		[MaxLength(7)]
		public List<Article> Articles { get; set; }
	}

	public class Article {
		/// <summary>
		///     标题，最长45个字符
		/// </summary>
		[JsonProperty("title")]
		[Required]
		[MaxLength(45)]
		public string Title { get; set; }

		/// <summary>
		///     作者，最长8个字符
		/// </summary>
		[JsonProperty("author")]
		[MaxLength(8)]
		public string Author { get; set; }

		/// <summary>
		///     摘要，最长140字符
		/// </summary>
		[JsonProperty("description")]
		[MaxLength(140)]
		public string Description { get; set; }

		/// <summary>
		///     封面图片对应的mediaId
		/// </summary>
		[JsonProperty("coverImage")]
		[Required]
		public string CoverImageId { get; set; }

		/// <summary>
		///     当articles.type为"TEXT"时，内容中是否显示图片(false不包含，true包含，默认false)
		/// </summary>
		[JsonProperty("coverImageInContent")]
		public bool CoverImageInContent { get; set; }

		/// <summary>
		///     图文消息内容类型（"TEXT","URL"）
		/// </summary>
		[JsonProperty("type")]
		[Required]
		public ArticleType Type { get; set; }

		/// <summary>
		///     图文消息内容，当imageTextType为"URL"时表示链接地址。
		/// </summary>
		[JsonProperty("content")]
		[Required]
		public string Content { get; set; }
	}

	[JsonConverter(typeof(StringEnumConverter))]
	public enum ArticleType : byte {
		[EnumMember(Value = "TEXT")]
		Text,

		[EnumMember(Value = "URL")]
		Link
	}
}