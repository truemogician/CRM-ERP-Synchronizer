// ReSharper disable StringLiteralTypo
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using FXiaoKe.Models;
using FXiaoKe.Responses;
using FXiaoKe.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Shared;
using Shared.Exceptions;
using Shared.Utilities;

namespace FXiaoKe.Requests {
	[Request("/cgi/crm/custom/v2/data/get", typeof(QueryByConditionResponse))]
	public class CustomQueryByIdRequest<T> : QueryByIdRequest<T> where T : ModelBase {
		public CustomQueryByIdRequest() { }
		public CustomQueryByIdRequest(string id) : base(id) { }
	}

	[Request("/cgi/crm/v2/data/get", typeof(QueryByConditionResponse))]
	public class QueryByIdRequest<T> : CrmRequest<IdInfo<T>> where T : ModelBase {
		public QueryByIdRequest() { }
		public QueryByIdRequest(string id) : base(new IdInfo<T>(id)) { }
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

	public class QueryCustomByConditionRequest<T> : QueryCustomByConditionRequest where T : ModelBase {
		public QueryCustomByConditionRequest() { }
		public QueryCustomByConditionRequest(ConditionInfo<T> data) => Data = data;
		public override ConditionInfo<T> Data { get; }
	}

	[Request("/cgi/crm/custom/v2/data/query", typeof(QueryByConditionResponse))]
	public class QueryCustomByConditionRequest : QueryByConditionRequest {
		public QueryCustomByConditionRequest() { }
		public QueryCustomByConditionRequest(ConditionInfo data) : base(data) { }
	}

	public class QueryByConditionRequest<T> : QueryByConditionRequest where T : ModelBase {
		public QueryByConditionRequest() { }
		public QueryByConditionRequest(ConditionInfo<T> data) => Data = data;

		public override ConditionInfo<T> Data { get; }
	}

	[Request("/cgi/crm/v2/data/query", typeof(QueryByConditionResponse))]
	public class QueryByConditionRequest : CovariantCrmRequest<ConditionInfo> {
		public QueryByConditionRequest() { }
		public QueryByConditionRequest(ConditionInfo data) : base(data) { }
	}

	public class ConditionInfo<T> : ConditionInfo where T : ModelBase {
		public ConditionInfo() { }

		public ConditionInfo(QueryCondition<T> condition) {
			Condition = condition;
			ObjectName = typeof(T).GetModelName();
		}

		public override QueryCondition<T> Condition { get; }

		public override Type Type => typeof(T);

		public static implicit operator QueryCondition<T>(ConditionInfo<T> self) => self.Condition;

		public static implicit operator ConditionInfo<T>(QueryCondition<T> other) => new(other);

		public static implicit operator ConditionInfo<T>(ModelFilter<T> filter) => new(filter);

		public static implicit operator ConditionInfo<T>(ModelFilter<T>[] filters) => new(filters);

		public static implicit operator ConditionInfo<T>(List<ModelFilter<T>> filters) => new(filters.ToArray());
	}

	public class ConditionInfo : DataBase, IType {
		public ConditionInfo() { }

		public ConditionInfo(QueryCondition condition) {
			Condition = condition;
			ObjectName = condition.Type.GetModelName();
		}

		/// <summary>
		///     查询条件列表
		/// </summary>
		[JsonProperty("search_query_info")]
		[Required]
		public virtual QueryCondition Condition { get; }

		/// <summary>
		///     true->返回total,false->不返回total总数。默认true。设置为false可以加快接口响应速度
		/// </summary>
		[JsonProperty("find_explicit_total_num")]
		public bool ReturnTotal { get; set; }

		public virtual Type Type => Condition.Type;

		public static implicit operator QueryCondition(ConditionInfo self) => self.Condition;

		public static implicit operator ConditionInfo(QueryCondition other) => new(other);

		public static implicit operator ConditionInfo(ModelFilter filter) => new(filter);

		public static implicit operator ConditionInfo(ModelFilter[] filters) => new(filters);

		public static implicit operator ConditionInfo(List<ModelFilter> filters) => new(filters.ToArray());
	}

	public class QueryCondition<T> : QueryCondition where T : ModelBase {
		public QueryCondition() { }

		public QueryCondition(params ModelFilter<T>[] filters) => Filters = filters;

		public QueryCondition(IEnumerable<ModelFilter<T>> filters, IEnumerable<ModelOrder<T>> orders) {
			Filters = filters.AsList();
			Orders = orders.AsList();
		}

		[JsonIgnore]
		public override IReadOnlyList<ModelFilter<T>> Filters { get; }

		[JsonIgnore]
		public override IReadOnlyList<ModelOrder<T>> Orders { get; }

		public override Type Type {
			get => typeof(T);
			init {
				if (value != typeof(T))
					throw new InvalidOperationException($"{nameof(Type)} must equals to generic type parameter");
			}
		}

		public static implicit operator QueryCondition<T>(ModelFilter<T> filter) => new(filter);

		public static implicit operator QueryCondition<T>(ModelFilter<T>[] filters) => new(filters);

		public static implicit operator QueryCondition<T>(List<ModelFilter<T>> filters) => new(filters.ToArray());
	}

	public class QueryCondition : IType {
		private readonly Type _type;

		public QueryCondition() { }

		public QueryCondition(Type type) => _type = type;

		public QueryCondition(params ModelFilter[] filters) {
			Filters = filters;
			_type = filters.SameOrDefault(filter => filter.Type);
		}

		public QueryCondition(IEnumerable<ModelFilter> filters, IEnumerable<ModelOrder> orders) {
			var (filterList, orderList) = (filters.AsList(), orders.AsList());
			Filters = filterList;
			Orders = orderList;
			_type = ((IEnumerable<IType>)filterList).Concat(orderList).SameOrDefault(t => t.Type);
		}

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
		public virtual IReadOnlyList<ModelFilter> Filters { get; } = new List<ModelFilter>();

		/// <summary>
		///     排序
		/// </summary>
		[JsonProperty("orders")]
		[Required]
		public virtual IReadOnlyList<ModelOrder> Orders { get; } = new List<ModelOrder>();

		/// <summary>
		///     返回字段列表
		/// </summary>
		[JsonProperty("fieldProjection")]
		public List<string> FieldProjection { get; set; }

		public virtual Type Type {
			get => _type;
			init => _type = value;
		}

		public static implicit operator QueryCondition(ModelFilter filter) => new(filter);

		public static implicit operator QueryCondition(ModelFilter[] filters) => new(filters);

		public static implicit operator QueryCondition(List<ModelFilter> filters) => new(filters.ToArray());
	}

	public class ModelFilter<T> : ModelFilter where T : ModelBase {
		public ModelFilter(string propertyName) : base(typeof(T), propertyName) { }
		public ModelFilter(string propertyName, QueryOperator @operator, params object[] values) : base(typeof(T), propertyName, @operator, values) { }

		public ModelFilter(ModelFilter source) : this(source.Property.Names.Single(), source.Operator, source.Values) {
			if (source.Type != typeof(T))
				throw new TypeNotMatchException(typeof(T), source.Type);
		}
	}

	public class ModelFilter : IType {
		/// <summary>
		/// </summary>
		/// <param name="type"></param>
		/// <param name="propertyName">属性名称，建议使用nameof获取</param>
		public ModelFilter(Type type, string propertyName) => Property = new PropertyChain(propertyName) {StartingType = type};

		public ModelFilter(Type type, string propertyName, QueryOperator @operator, params object[] values) : this(type, propertyName) {
			Operator = @operator;
			Values = values.ToList();
		}

		[JsonIgnore]
		internal PropertyChain Property { get; }

		/// <summary>
		///     筛选字段名，对象字段详情请参考对象描述
		/// </summary>
		[JsonProperty("field_name")]
		public string JsonPropertyName {
			get => Property.ToString("json");
			set => Property.FromString(value, "json");
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

		public Type Type {
			get => Property.StartingType;
			set => Property.StartingType = value;
		}
	}

	public class ModelEqualityFilter<T> : ModelFilter<T> where T : ModelBase {
		public ModelEqualityFilter(string propertyName, object value) : base(propertyName, QueryOperator.Equal, value) { }
	}

	public class ModelEqualityFilter : ModelFilter {
		public ModelEqualityFilter(Type type, string propertyName, object value) : base(type, propertyName, QueryOperator.Equal, value) { }
	}

	public class ModelOrder<T> : ModelOrder where T : ModelBase {
		/// <summary>
		/// </summary>
		/// <param name="propertyName">属性名称，建议使用nameof获取</param>
		/// <param name="ascending"></param>
		public ModelOrder(string propertyName, bool ascending) : base(typeof(T), propertyName, ascending) { }

		public ModelOrder(ModelOrder source) : this(source.Property.Names.Single(), source.Ascending) {
			if (source.Type != typeof(T))
				throw new TypeNotMatchException(typeof(T), source.Type);
		}
	}

	public class ModelOrder : IType {
		/// <summary>
		/// </summary>
		/// <param name="propertyName">属性名称，建议使用nameof获取</param>
		/// <param name="ascending"></param>
		public ModelOrder(Type type, string propertyName, bool ascending) {
			Property = new PropertyChain(propertyName) {StartingType = type};
			Ascending = ascending;
		}

		[JsonIgnore]
		internal PropertyChain Property { get; }

		/// <summary>
		///     字段名
		/// </summary>
		[JsonProperty("fieldName")]
		public string JsonPropertyName {
			get => Property.ToString("json");
			set => Property.FromString(value, "json");
		}

		/// <summary>
		///     如果是true，按照升序排列，如果是false，则按照倒序排列
		/// </summary>
		[JsonProperty("isAsc")]
		[Required]
		public bool Ascending { get; set; }

		public Type Type {
			get => Property.StartingType;
			set => Property.StartingType = value;
		}
	}

	public class AscendingModelOrder<T> : ModelOrder<T> where T : ModelBase {
		public AscendingModelOrder(string propertyName) : base(propertyName, true) { }
	}

	public class AscendingModelOrder : ModelOrder {
		public AscendingModelOrder(Type type, string propertyName) : base(type, propertyName, true) { }
	}

	public class DescendingModelOrder<T> : ModelOrder<T> where T : ModelBase {
		public DescendingModelOrder(string propertyName) : base(propertyName, false) { }
	}

	public class DescendingModelOrder : ModelOrder {
		public DescendingModelOrder(Type type, string propertyName) : base(type, propertyName, false) { }
	}

	internal interface IType {
		[JsonIgnore]
		public Type Type { get; }
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