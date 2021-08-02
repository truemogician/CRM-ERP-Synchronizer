using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Kingdee.Forms;
using Newtonsoft.Json;
using Shared.Exceptions;
using Shared.Serialization;

namespace Kingdee.Requests {
	public class DeleteRequest<T> : RequestBase where T : ErpModelBase {
		public DeleteRequest(string idName, params T[] entities) {
			IdProperty = typeof(T).GetProperty(idName, typeof(string));
			if (IdProperty is null)
				throw new TypeException(typeof(T), $"Property \"{idName}\" with string type not found");
			Entities = entities.ToList();
		}

		[JsonIgnore]
		public PropertyInfo IdProperty { get; }

		[JsonIgnore]
		public List<T> Entities { get; }

		/// <summary>
		///     创建者组织内码
		/// </summary>
		[JsonProperty("CreateOrgId")]
		public int CreatorOrgId { get; set; }

		/// <summary>
		///     单据编码集合
		/// </summary>
		[JsonProperty("Numbers")]
		public List<object> Numbers { get; set; } = new();

		/// <summary>
		///     单据内码集合
		/// </summary>
		[JsonProperty("Ids")]
		[JsonConverter(typeof(StringCollectionConverter), ',')]
		public IEnumerable<string> Ids => Entities.Select(entity => IdProperty.GetValue(entity) as string);

		/// <summary>
		///     是否启用网控
		/// </summary>
		[JsonProperty("NetworkCtrl")]
		[JsonConverter(typeof(BoolConverter))]
		public bool NetworkControl { get; set; }
	}
}