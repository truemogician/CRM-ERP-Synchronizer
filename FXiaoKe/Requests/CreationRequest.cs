using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FXiaoKe.Models;
using FXiaoKe.Responses;
using FXiaoKe.Utilities;
using Newtonsoft.Json;
using Shared.Serialization;

namespace FXiaoKe.Requests {
	[Request("/cgi/crm/custom/v2/data/create")]
	public class CustomCreationRequest<T> : CreationRequest<T> where T : CrmModelBase { }

	[Request("/cgi/crm/v2/data/create")]
	public class CreationRequest<T> : CreationRequestBase<CreationData<T>> where T : CrmModelBase { }

	[Request(null, typeof(CreationResponse), ErrorMessage = "创建对象时发生错误")]
	public abstract class CreationRequestBase<T> : CrmRequest<T> {
		/// <summary>
		///     是否触发工作流（不传时默认为true, 表示触发），该参数对所有对象均有效
		/// </summary>
		[JsonProperty("triggerWorkFlow")]
		public bool TriggerWorkFlow { get; set; }

		/// <summary>
		///     是否指定创建时间（不传时默认为false,表示忽略参数中的create_time创建时间字段）
		/// </summary>
		[JsonProperty("hasSpecifyTime")]
		public bool SpecifyCreationTime { get; set; }

		/// <summary>
		///     是否指定创建人（不传时默认为false,表示忽略参数中的created_by创建时间字段）
		/// </summary>
		[JsonProperty("hasSpecifyCreatedBy")]
		public bool SpecifyCreator { get; set; }

		/// <summary>
		///     主从对象一起创建时，是否返回从对象id列表，true返回，false不返回，默认不返回
		/// </summary>
		[JsonProperty("includeDetailIds")]
		public bool ReturnSubObjectIds { get; set; }
	}

	public class CreationData<T> where T : CrmModelBase {
		public CreationData() { }

		public CreationData(T model) => Model = model;

		/// <summary>
		///     主对象数据map(和对象描述中字段一一对应)
		/// </summary>
		[JsonProperty("object_data")]
		[JsonConverter(typeof(CreationDataModelConverter))]
		[Required]
		public T Model { get; set; }

		/// <summary>
		///     明细对象数据map(和对象描述中字段一一对应)
		/// </summary>
		[JsonProperty("details")]
		public Dictionary<string, IEnumerable<CrmModelBase>> Details { get; } = new();

		public static implicit operator CreationData<T>(T model) => new(model);

		public static implicit operator T(CreationData<T> data) => data.Model;
	}

	public class CreationDataModelConverter : AdditionalPropertyConverter<CrmModelBase> {
		public CreationDataModelConverter() => AdditionalProperties.Add("dataObjectApiName", value => value.GetType().GetModelName());
	}
}