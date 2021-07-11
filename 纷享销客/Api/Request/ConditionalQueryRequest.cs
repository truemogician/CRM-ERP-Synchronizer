using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using FXiaoKe.Utilities;
using Newtonsoft.Json;

namespace FXiaoKe.Api.Request {
	[Request("/cgi/crm/v2/data/query", HttpMethod.Post, typeof(ConditionalQueryRequest))]
	public class ConditionalQueryRequest : CrmBasicRequest<QueryCondition> { }

	public class QueryCondition {
		/// <summary>
		/// 对象的api_name
		/// </summary>
		[JsonProperty("dataObjectApiName")]
		[Required]
		public string ObjectName { get; set; }

		/// <summary>
		/// 查询条件列表
		/// </summary>
		[JsonProperty("search_query_info")]
		[Required]
		public QueryInfo QueryInfo { get; set; }

		/// <summary>
		/// true->返回total,false->不返回total总数。默认true。设置为false可以加快接口响应速度
		/// </summary>
		[JsonProperty("find_explicit_total_num")]
		public bool ReturnTotal { get; set; }
	}

	public class QueryInfo {
		/// <summary>
		/// 获取数据条数, 最大值为100
		/// </summary>
		[JsonProperty("limit")]
		[Required]
		public int Limit { get; set; }

		/// <summary>
		/// 偏移量，从0开始、数值必须为limit的整数倍
		/// </summary>
		[JsonProperty("offset")]
		[Required]
		public int Offset { get; set; }

		/// <summary>
		/// 过滤条件列表
		/// </summary>
		[JsonProperty("filters")]
		[Required]
		public List<QueryFilter> Filters { get; set; }

		/// <summary>
		/// 排序
		/// </summary>
		[JsonProperty("orders")]
		[Required]
		public List<QueryOrder> Orders { get; set; }

		/// <summary>
		/// 返回字段列表
		/// </summary>
		[JsonProperty("fieldProjection")]
		public List<string> FieldProjection { get; set; }
	}

	public class QueryFilter {
		/// <summary>
		/// 筛选字段名，对象字段详情请参考对象描述
		/// </summary>
		[JsonProperty("field_name")]
		[Required]
		public string FieldName { get; set; }

		/// <summary>
		/// 取值范围
		/// </summary>
		[JsonProperty("field_values")]
		[Required]
		public List<object> FieldValues { get; set; }

		/// <summary>
		/// 支持操作
		/// </summary>
		[JsonProperty("operator")]
		[Required]
		public QueryOperator Operator { get; set; }
	}

	public class QueryOrder {
		/// <summary>
		/// 字段名
		/// </summary>
		[JsonProperty("fieldName")]
		[Required]
		public string FieldName { get; set; }

		/// <summary>
		/// 如果是true，按照升序排列，如果是false，则按照倒序排列
		/// </summary>
		[JsonProperty("isAsc")]
		[Required]
		public bool Ascending { get; set; }
	}

	public enum QueryOperator {
		/// <summary>
		/// =
		/// </summary>
		[EnumMember(Value = "EQ")]
		Equal,

		/// <summary>
		/// >
		/// </summary>
		[EnumMember(Value = "GT")]
		Greater,

		/// <summary>
		/// <
		/// </summary>
		[EnumMember(Value = "LT")]
		Less,

		/// <summary>
		/// >=
		/// </summary>
		[EnumMember(Value = "GTE")]
		GreaterEqual,

		/// <summary>
		/// <=
		/// </summary>
		[EnumMember(Value = "LTE")]
		LessEqual,

		/// <summary>
		/// !=
		/// </summary>
		[EnumMember(Value = "N")]
		NotEqual,

		/// <summary>
		/// LIKE
		/// </summary>
		[EnumMember(Value = "LIKE")]
		Like,

		/// <summary>
		/// NOT LIKE
		/// </summary>
		[EnumMember(Value = "NLIKE")]
		NotLike,

		/// <summary>
		/// IS
		/// </summary>
		[EnumMember(Value = "IS")]
		Is,

		/// <summary>
		/// IS NOT
		/// </summary>
		[EnumMember(Value = "ISN")]
		IsNot,

		/// <summary>
		/// IN
		/// </summary>
		[EnumMember(Value = "IN")]
		In,

		/// <summary>
		/// NOT IN
		/// </summary>
		[EnumMember(Value = "NIN")]
		NotIn,

		/// <summary>
		/// BETWEEN
		/// </summary>
		[EnumMember(Value = "BETWEEN")]
		Between,

		/// <summary>
		/// NOT BETWEEN
		/// </summary>
		[EnumMember(Value = "NBETWEEN")]
		NotBetween,

		/// <summary>
		/// LIKE%
		/// </summary>
		[EnumMember(Value = "STARTWITH")]
		StartWith,

		/// <summary>
		/// %LIKE
		/// </summary>
		[EnumMember(Value = "ENDWITH")]
		EndWith,

		/// <summary>
		/// Array包含
		/// </summary>
		[EnumMember(Value = "CONTAINS")]
		Contains
	}
}