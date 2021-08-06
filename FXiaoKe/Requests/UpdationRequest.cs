using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using FXiaoKe.Models;
using FXiaoKe.Responses;
using FXiaoKe.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shared.Exceptions;

namespace FXiaoKe.Requests {
	[Request("/cgi/crm/custom/v2/data/update")]
	public class CustomUpdationRequest<T> : UpdationRequest<T> where T : CrmModelBase { }

	[Request("/cgi/crm/v2/data/update", typeof(BasicResponse), ErrorMessage = "更新对象时发生错误")]
	public class UpdationRequest<T> : CrmRequest<UpdationData<T>> where T : CrmModelBase {
		/// <summary>
		///     是否触发工作流（不传时默认为true, 表示触发），该参数对所有对象均有效
		/// </summary>
		[JsonProperty("triggerWorkFlow")]
		public bool TriggerWorkFlow { get; set; }
	}

	public class UpdationData<T> where T : CrmModelBase {
		public UpdationData() { }

		public UpdationData(IUpdater<T> model) => Model = model;

		/// <summary>
		///     主对象数据map(和对象描述中字段一一对应)
		/// </summary>
		[JsonProperty("object_data")]
		[Required]
		public IUpdater<T> Model { get; set; }
	}

	[JsonConverter(typeof(UpdaterConverter))]
	public class Updater<T> : IUpdater<T> where T : CrmModelBase {
		public Updater(T model) => Model = model;

		public T Model { get; }

		public HashSet<string> UpdationCollection { get; } = new();

		public void Update(MemberInfo member, object value) {
			if (!member.GetValueType().IsInstanceOfType(value))
				throw new InvariantTypeException(member.GetValueType(), value?.GetType());
			member.SetValue(Model, value);
			UpdationCollection.Add(member.Name);
		}

		public void Update(string memberName, object value) => Update(typeof(T).GetMostDerivedMember(memberName), value);
	}

	public interface IUpdater<out T> where T : CrmModelBase {
		public T Model { get; }

		public HashSet<string> UpdationCollection { get; }

		public void Update(MemberInfo member, object value);
	}

	public class UpdaterConverter : JsonConverter<IUpdater<CrmModelBase>> {
		public override void WriteJson(JsonWriter writer, IUpdater<CrmModelBase> value, JsonSerializer serializer) {
			if (value is null) {
				writer.WriteNull();
				return;
			}
			string json;
			if (serializer.ContractResolver is ContractResolver resolver) {
				var list = resolver.UpdationList;
				resolver.UpdationList = value.UpdationCollection.ToList();
				json = serializer.Serialize(value.Model);
				resolver.UpdationList = list;
			}
			else {
				var originalResolver = serializer.ContractResolver;
				serializer.ContractResolver = new ContractResolver {UpdationList = value.UpdationCollection.ToList()};
				json = serializer.Serialize(value.Model);
				serializer.ContractResolver = originalResolver;
			}
			var obj = JToken.Parse(json) as JObject;
			obj!.Add("_id", value.Model.DataId);
			obj.Add("dataObjectApiName", value.Model.GetType().GetModelName());
			writer.WriteValue(obj, serializer);
		}

		public override IUpdater<CrmModelBase> ReadJson(JsonReader reader, Type objectType, IUpdater<CrmModelBase> existingValue, bool hasExistingValue, JsonSerializer serializer) {
			if (!objectType.IsGenericType || objectType.GetGenericTypeDefinition() != typeof(Updater<>))
				throw new TypeNotMatchException(typeof(Updater<>), objectType);
			var modelType = objectType.GetGenericArguments()[0];
			return objectType.Construct(serializer.Deserialize(reader, modelType)) as IUpdater<CrmModelBase>;
		}
	}
}