using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace FXiaoKe.Responses {
	public class QueryByIdResponse : QueryByIdResponse<dynamic> { }

	public class QueryByIdResponse<T> : DataResponse<T> { }

	public class QueryByConditionResponse : QueryByConditionResponse<dynamic> { }

	public class QueryByConditionResponse<T> : DataResponse<ListResult<T>> { }

	public class ListResult<T> {
		/// <summary>
		///     实际总记录数
		/// </summary>
		[JsonProperty("total")]
		[Required]
		public int Total { get; set; }

		/// <summary>
		///     偏移量
		/// </summary>
		[JsonProperty("offset")]
		[Required]
		public int Offset { get; set; }

		/// <summary>
		///     查询数据的条数
		/// </summary>
		[JsonProperty("limit")]
		[Required]
		public int Limit { get; set; }

		/// <summary>
		///     数据列表
		/// </summary>
		[JsonProperty("dataList")]
		[Required]
		public List<T> DataList { get; set; }
	}
}