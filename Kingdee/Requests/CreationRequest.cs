// ReSharper disable StringLiteralTypo
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Shared.Serialization;

namespace Kingdee.Requests {
	public abstract class CreationRequest<T> : RequestBase {
		protected CreationRequest() { }

		protected CreationRequest(T data) => Data = data;

		/// <summary>
		///     需要更新的字段
		/// </summary>
		[JsonProperty("NeedUpDateFields", ItemConverterType = typeof(ToStringConverter<Field>))]
		public List<Field<T>> FieldsToUpdate { get; set; } = new();

		/// <summary>
		///     需返回结果的字段集合
		/// </summary>
		[JsonProperty("NeedReturnFields", ItemConverterType = typeof(ToStringConverter<Field>))]
		public List<Field<T>> FieldsToReturn { get; set; } = new();

		/// <summary>
		///     是否删除已存在的分录，默认为true
		/// </summary>
		[JsonProperty("IsDeleteEntry")]
		[JsonConverter(typeof(BoolConverter))]
		public bool? DeleteExistingEntry { get; set; } = true;

		/// <summary>
		///     表单所在的子系统内码
		/// </summary>
		[JsonProperty("SubSystemId")]
		public string SubsystemId { get; set; }

		/// <summary>
		///     是否批量填充分录，默认为true
		/// </summary>
		[JsonProperty("IsEntryBatchFill")]
		[JsonConverter(typeof(BoolConverter))]
		public bool? BatchFill { get; set; } = true;

		/// <summary>
		///     是否验证标志，默认为true
		/// </summary>
		[JsonProperty("ValidateFlag")]
		[JsonConverter(typeof(BoolConverter))]
		public bool? ValidateFlags { get; set; } = true;

		/// <summary>
		///     是否用编码搜索基础资料，默认为true
		/// </summary>
		[JsonProperty("NumberSearch")]
		[JsonConverter(typeof(BoolConverter))]
		public bool? SearchByNumber { get; set; } = true;

		/// <summary>
		///     交互标志集合
		/// </summary>
		[JsonProperty("InterationFlags")]
		[JsonConverter(typeof(StringCollectionConverter), ';')]
		public List<string> InteractionFlags { get; set; } = new();

		[JsonProperty("Model")]
		[Required]
		public T Data { get; set; }
	}
}