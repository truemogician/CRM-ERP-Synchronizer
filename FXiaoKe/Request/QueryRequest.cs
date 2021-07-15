// ReSharper disable StringLiteralTypo
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using FXiaoKe.Models;
using FXiaoKe.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Shared.Utilities;

namespace FXiaoKe.Request {
	[Request("/cgi/crm/custom/v2/data/get", typeof(QueryByConditionResponse))]
	public class QueryCustomByIdRequest<T> : QueryByIdRequest<T> where T : ModelBase {
		public QueryCustomByIdRequest() { }
		public QueryCustomByIdRequest(Client client) : base(client) { }
		public QueryCustomByIdRequest(string id, Client client) : base(id, client) { }
	}

	[Request("/cgi/crm/v2/data/get", typeof(QueryByConditionResponse))]
	public class QueryByIdRequest<T> : CrmRequest<IdInfo<T>> where T : ModelBase {
		public QueryByIdRequest() { }
		public QueryByIdRequest(Client client) : base(client) { }
		public QueryByIdRequest(string id, Client client) : base(new IdInfo<T>(id), client) { }
	}

	public class IdInfo<T> : DataBase<T> where T : ModelBase {
		public IdInfo(string id) => Id = id;

		/// <summary>
		///     数据Id
		/// </summary>
		[JsonProperty("objectDataId")]
		[Required]
		public string Id { get; set; }
	}

	[Request("/cgi/crm/custom/v2/data/query", typeof(QueryByConditionResponse))]
	public class QueryCustomByConditionRequest<T> : QueryByConditionRequest<T> where T : ModelBase {
		public QueryCustomByConditionRequest() { }
		public QueryCustomByConditionRequest(Client client) : base(client) { }
	}

	[Request("/cgi/crm/v2/data/query", typeof(QueryByConditionResponse))]
	public class QueryByConditionRequest<T> : CrmRequest<ConditionInfo<T>> where T : ModelBase {
		public QueryByConditionRequest() { }
		public QueryByConditionRequest(Client client) : base(client) { }
	}

	public class ConditionInfo<T> : DataBase<T> where T : ModelBase {
		/// <summary>
		///     查询条件列表
		/// </summary>
		[JsonProperty("search_query_info")]
		[Required]
		public QueryCondition<T> Condition { get; set; }

		/// <summary>
		///     true->返回total,false->不返回total总数。默认true。设置为false可以加快接口响应速度
		/// </summary>
		[JsonProperty("find_explicit_total_num")]
		public bool ReturnTotal { get; set; } = true;

		public static explicit operator QueryCondition<T>(ConditionInfo<T> self) => self.Condition;

		public static explicit operator ConditionInfo<T>(QueryCondition<T> other)
			=> new() {
				Condition = other
			};
	}

	public class QueryCondition<T> where T : ModelBase {
		public QueryCondition() { }

		public QueryCondition(params ModelFilter<T>[] filters) => Filters = filters.ToList();

		/// <summary>
		///     获取数据条数, 最大值为100
		/// </summary>
		[JsonProperty("limit")]
		[Required]
		public int Limit { get; set; } = 100;

		/// <summary>
		///     偏移量，从0开始、数值必须为limit的整数倍
		/// </summary>
		[JsonProperty("offset")]
		[Required]
		public int Offset { get; set; }

		/// <summary>
		///     过滤条件列表
		/// </summary>
		[JsonProperty("filters")]
		[Required]
		public List<ModelFilter<T>> Filters { get; set; } = new();

		/// <summary>
		///     排序
		/// </summary>
		[JsonProperty("orders")]
		[Required]
		public List<ModelOrder<T>> Orders { get; set; } = new();

		/// <summary>
		///     返回字段列表
		/// </summary>
		[JsonProperty("fieldProjection")]
		public List<string> FieldProjection { get; set; }
	}

	public class ModelFilter<T> where T : ModelBase {
		/// <summary>
		/// </summary>
		/// <param name="propertyName">属性名称，建议使用nameof获取</param>
		public ModelFilter(string propertyName) => Property = typeof(T).GetProperty(propertyName);

		public ModelFilter(string propertyName, QueryOperator @operator, params object[] values) : this(propertyName) {
			Operator = @operator;
			Values = values.ToList();
		}

		[JsonIgnore]
		[Required]
		public PropertyInfo Property { get; set; }

		/// <summary>
		///     筛选字段名，对象字段详情请参考对象描述
		/// </summary>
		[JsonProperty("field_name")]
		public string JsonPropertyName {
			get => Property.GetJsonProperty() is { } attr ? attr.PropertyName : Property.Name;
			set => Property = typeof(T).GetPropertyFromJsonPropertyName(value);
		}

		/// <summary>
		///     取值范围
		/// </summary>
		[JsonProperty("field_values")]
		[Required]
		public List<object> Values { get; set; }

		/// <summary>
		///     支持操作
		/// </summary>
		[JsonProperty("operator")]
		[Required]
		public QueryOperator Operator { get; set; }
	}

	public class ModelEqualityFilter<T> : ModelFilter<T> where T : ModelBase {
		public ModelEqualityFilter(string propertyName, object value) : base(propertyName, QueryOperator.Equal, value) { }
	}

	public class ModelOrder<T> where T : ModelBase {
		/// <summary>
		/// </summary>
		/// <param name="propertyName">属性名称，建议使用nameof获取</param>
		/// <param name="ascending"></param>
		public ModelOrder(string propertyName, bool ascending) {
			Property = typeof(T).GetProperty(propertyName);
			Ascending = ascending;
		}

		[JsonIgnore]
		[Required]
		public PropertyInfo Property { get; set; }

		/// <summary>
		///     字段名
		/// </summary>
		[JsonProperty("fieldName")]
		public string JsonPropertyName {
			get => Property.GetJsonProperty() is { } attr ? attr.PropertyName : Property.Name;
			set => Property = typeof(T).GetPropertyFromJsonPropertyName(value);
		}

		/// <summary>
		///     如果是true，按照升序排列，如果是false，则按照倒序排列
		/// </summary>
		[JsonProperty("isAsc")]
		[Required]
		public bool Ascending { get; set; }
	}

	public class AscendingModelOrder<T> : ModelOrder<T> where T : ModelBase {
		public AscendingModelOrder(string propertyName) : base(propertyName, true) { }
	}

	public class DescendingModelOrder<T> : ModelOrder<T> where T : ModelBase {
		public DescendingModelOrder(string propertyName) : base(propertyName, false) { }
	}

	[JsonConverter(typeof(StringEnumConverter))]
	public enum QueryOperator {
		/// <summary>
		///     =
		/// </summary>
		[EnumMember(Value = "EQ")]
		Equal,

		/// <summary>
		///     >
		/// </summary>
		[EnumMember(Value = "GT")]
		Greater,

		/// <summary>
		///     <
		/// </summary>
		[EnumMember(Value = "LT")]
		Less,

		/// <summary>
		///     >=
		/// </summary>
		[EnumMember(Value = "GTE")]
		GreaterEqual,

		/// <summary>
		///     <=
		/// </summary>
		[EnumMember(Value = "LTE")]
		LessEqual,

		/// <summary>
		///     !=
		/// </summary>
		[EnumMember(Value = "N")]
		NotEqual,

		/// <summary>
		///     LIKE
		/// </summary>
		[EnumMember(Value = "LIKE")]
		Like,

		/// <summary>
		///     NOT LIKE
		/// </summary>
		[EnumMember(Value = "NLIKE")]
		NotLike,

		/// <summary>
		///     IS
		/// </summary>
		[EnumMember(Value = "IS")]
		Is,

		/// <summary>
		///     IS NOT
		/// </summary>
		[EnumMember(Value = "ISN")]
		IsNot,

		/// <summary>
		///     IN
		/// </summary>
		[EnumMember(Value = "IN")]
		In,

		/// <summary>
		///     NOT IN
		/// </summary>
		[EnumMember(Value = "NIN")]
		NotIn,

		/// <summary>
		///     BETWEEN
		/// </summary>
		[EnumMember(Value = "BETWEEN")]
		Between,

		/// <summary>
		///     NOT BETWEEN
		/// </summary>
		[EnumMember(Value = "NBETWEEN")]
		NotBetween,

		/// <summary>
		///     LIKE%
		/// </summary>
		[EnumMember(Value = "STARTWITH")]
		StartWith,

		/// <summary>
		///     %LIKE
		/// </summary>
		[EnumMember(Value = "ENDWITH")]
		EndWith,

		/// <summary>
		///     Array包含
		/// </summary>
		[EnumMember(Value = "CONTAINS")]
		Contains
	}
}