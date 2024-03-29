﻿// ReSharper disable StringLiteralTypo
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using FXiaoKe.Models;
using FXiaoKe.Responses;
using FXiaoKe.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Shared;
using Shared.Exceptions;
using Shared.Serialization;

namespace FXiaoKe.Requests {
	[Request("/cgi/crm/custom/v2/data/get", typeof(QueryByConditionResponse))]
	public class CustomQueryByIdRequest<T> : QueryByIdRequest<T> where T : CrmModelBase {
		public CustomQueryByIdRequest() { }

		public CustomQueryByIdRequest(string id) : base(id) { }
	}

	[Request("/cgi/crm/v2/data/get", typeof(QueryByConditionResponse))]
	public class QueryByIdRequest<T> : CrmRequest<IdInfo<T>> where T : CrmModelBase {
		public QueryByIdRequest() { }

		public QueryByIdRequest(string id) : base(new IdInfo<T>(id)) { }
	}

	public class IdInfo<T> : DataBase<T> where T : CrmModelBase {
		public IdInfo(string id) => Id = id;

		/// <summary>
		///     数据Id
		/// </summary>
		[JsonProperty("objectDataId")]
		[Required]
		public string Id { get; set; }
	}

	public class QueryCustomByConditionRequest<T> : QueryCustomByConditionRequest where T : CrmModelBase {
		public QueryCustomByConditionRequest() { }

		public QueryCustomByConditionRequest(ConditionInfo<T> data) => Data = data;

		public override ConditionInfo<T> Data { get; }
	}

	[Request("/cgi/crm/custom/v2/data/query", typeof(QueryByConditionResponse))]
	public class QueryCustomByConditionRequest : QueryByConditionRequest {
		public QueryCustomByConditionRequest() { }

		public QueryCustomByConditionRequest(ConditionInfo data) : base(data) { }
	}

	public class QueryByConditionRequest<T> : QueryByConditionRequest where T : CrmModelBase {
		public QueryByConditionRequest() { }

		public QueryByConditionRequest(ConditionInfo<T> data) => Data = data;

		public override ConditionInfo<T> Data { get; }
	}

	[Request("/cgi/crm/v2/data/query", typeof(QueryByConditionResponse))]
	public class QueryByConditionRequest : CovariantCrmRequest<ConditionInfo> {
		public QueryByConditionRequest() { }

		public QueryByConditionRequest(ConditionInfo data) : base(data) { }
	}

	public class ConditionInfo<T> : ConditionInfo where T : CrmModelBase {
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

	public class QueryCondition<T> : QueryCondition where T : CrmModelBase {
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
		public int Limit { get; set; }

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

	public class ModelFilter<T> : ModelFilter where T : CrmModelBase {
		public ModelFilter(string propertyName) : base(typeof(T), propertyName) { }

		public ModelFilter(string propertyName, QueryOperator @operator, params object[] values) : base(typeof(T), propertyName, @operator, values) { }

		public ModelFilter(ModelFilter source) : this(source.Property.Names.Single(), source.Operator, source.Values) {
			if (source.Type != typeof(T))
				throw new TypeNotMatchException(typeof(T), source.Type);
		}
		#nullable enable
		public static ModelFilter<T> Equal(string propertyName, [NotNull] object value) => new(propertyName, QueryOperator.Equal, value);

		public static ModelFilter<T> NotEqual(string propertyName, [NotNull] object value) => new(propertyName, QueryOperator.NotEqual, value);

		public static ModelFilter<T> Is(string propertyName, object? value) => new(propertyName, QueryOperator.Is, value);

		public static ModelFilter<T> IsNot(string propertyName, object? value) => new(propertyName, QueryOperator.IsNot, value);

		public static ModelFilter<T> In(string propertyName, params object?[] values) => new(propertyName, QueryOperator.In, values);

		public static ModelFilter<T> NotIn(string propertyName, params object?[] values) => new(propertyName, QueryOperator.NotIn, values);

		public static ModelFilter<T> Greater(string propertyName, [NotNull] object value) => new(propertyName, QueryOperator.Greater, value);

		public static ModelFilter<T> Less(string propertyName, [NotNull] object value) => new(propertyName, QueryOperator.Less, value);

		public static ModelFilter<T> GreaterEqual(string propertyName, [NotNull] object value) => new(propertyName, QueryOperator.GreaterEqual, value);

		public static ModelFilter<T> LessEqual(string propertyName, [NotNull] object value) => new(propertyName, QueryOperator.LessEqual, value);

		public static ModelFilter<T> Like(string propertyName, [NotNull] string value) => new(propertyName, QueryOperator.Like, value);

		public static ModelFilter<T> NotLike(string propertyName, [NotNull] string value) => new(propertyName, QueryOperator.NotLike, value);

		public static ModelFilter<T> StartWith(string propertyName, [NotNull] string value) => new(propertyName, QueryOperator.StartWith, value);

		public static ModelFilter<T> EndWith(string propertyName, [NotNull] string value) => new(propertyName, QueryOperator.EndWith, value);

		public static ModelFilter<T> Between(string propertyName, [NotNull] object from, [NotNull] object to) => new(propertyName, QueryOperator.Between, from, to);

		public static ModelFilter<T> NotBetween(string propertyName, [NotNull] object from, [NotNull] object to) => new(propertyName, QueryOperator.NotBetween, from, to);
		#nullable disable
	}

	[JsonConverter(typeof(ModelFilterConverter))]
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

		public static ModelFilter Equal(Type type, string propertyName, object value) => new(type, propertyName, QueryOperator.Equal, value);

		public static ModelFilter NotEqual(Type type, string propertyName, object value) => new(type, propertyName, QueryOperator.NotEqual, value);

		public static ModelFilter In(Type type, string propertyName, params object[] values) => new(type, propertyName, QueryOperator.In, values);

		public static ModelFilter NotIn(Type type, string propertyName, params object[] values) => new(type, propertyName, QueryOperator.NotIn, values);
	}

	public class ModelOrder<T> : ModelOrder where T : CrmModelBase {
		/// <summary>
		/// </summary>
		/// <param name="propertyName">属性名称，建议使用nameof获取</param>
		/// <param name="ascending"></param>
		public ModelOrder(string propertyName, bool ascending) : base(typeof(T), propertyName, ascending) { }

		public ModelOrder(ModelOrder source) : this(source.Property.Names.Single(), source.Ascending) {
			if (source.Type != typeof(T))
				throw new TypeNotMatchException(typeof(T), source.Type);
		}

		public static ModelOrder<T> Asc(string propertyName) => new(propertyName, true);

		public static ModelOrder<T> Desc(string propertyName) => new(propertyName, false);
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

	internal interface IType {
		[JsonIgnore]
		public Type Type { get; }
	}

	#nullable enable
	internal class ModelFilterConverter : JsonConverter {
		public override bool CanRead => false;

		public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer) {
			if (value is not ModelFilter filter) {
				writer.WriteNull();
				return;
			}
			var contract = serializer.ContractResolver.ResolveContract(filter.GetType()) as JsonObjectContract;
			writer.WriteStartObject();
			foreach (var prop in contract!.Properties.Where(p => !p.Ignored)) {
				writer.WritePropertyName(prop.PropertyName!);
				if (prop.UnderlyingName != nameof(ModelFilter.Values))
					writer.WriteValue(prop.ValueProvider!.GetValue(filter), serializer);
				else {
					writer.WriteStartArray();
					var attr = filter.Property.StartingInfo.GetCustomAttribute<JsonConverterAttribute>();
					if (attr is not null) {
						var converter = (attr.ConverterType.Construct(attr.ConverterParameters) as JsonConverter)!;
						foreach (var v in filter.Values)
							converter.WriteJson(writer, v, serializer);
					}
					else
						foreach (var v in filter.Values)
							writer.WriteValue(v, serializer);
					writer.WriteEndArray();
				}
			}
			writer.WriteEndObject();
		}

		public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer) => throw new NotSupportedException();

		public override bool CanConvert(Type objectType) => objectType.IsAssignableTo(typeof(ModelFilter));
	}
	#nullable disable

	[JsonConverter(typeof(EnumValueConverter))]
	public enum QueryOperator {
		/// <summary>
		///     =
		/// </summary>
		[EnumValue("EQ")]
		Equal,

		/// <summary>
		///     >
		/// </summary>
		[EnumValue("GT")]
		Greater,

		/// <summary>
		///     <
		/// </summary>
		[EnumValue("LT")]
		Less,

		/// <summary>
		///     >=
		/// </summary>
		[EnumValue("GTE")]
		GreaterEqual,

		/// <summary>
		///     <=
		/// </summary>
		[EnumValue("LTE")]
		LessEqual,

		/// <summary>
		///     !=
		/// </summary>
		[EnumValue("N")]
		NotEqual,

		/// <summary>
		///     LIKE
		/// </summary>
		[EnumValue("LIKE")]
		Like,

		/// <summary>
		///     NOT LIKE
		/// </summary>
		[EnumValue("NLIKE")]
		NotLike,

		/// <summary>
		///     IS
		/// </summary>
		[EnumValue("IS")]
		Is,

		/// <summary>
		///     IS NOT
		/// </summary>
		[EnumValue("ISN")]
		IsNot,

		/// <summary>
		///     IN
		/// </summary>
		[EnumValue("IN")]
		In,

		/// <summary>
		///     NOT IN
		/// </summary>
		[EnumValue("NIN")]
		NotIn,

		/// <summary>
		///     BETWEEN
		/// </summary>
		[EnumValue("BETWEEN")]
		Between,

		/// <summary>
		///     NOT BETWEEN
		/// </summary>
		[EnumValue("NBETWEEN")]
		NotBetween,

		/// <summary>
		///     LIKE%
		/// </summary>
		[EnumValue("STARTWITH")]
		StartWith,

		/// <summary>
		///     %LIKE
		/// </summary>
		[EnumValue("ENDWITH")]
		EndWith,

		/// <summary>
		///     Array包含
		/// </summary>
		[EnumValue("CONTAINS")]
		Contains
	}
}