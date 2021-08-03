using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Kingdee.Forms;
using Newtonsoft.Json;
using Shared.Serialization;

namespace Kingdee.Requests {
	public class DeleteRequest<T> : RequestBase where T : ErpModelBase {
		private static readonly MemberInfo Key = FormMeta<T>.Key;

		public DeleteRequest(params int[] ids) => Ids = ids.ToList();

		public DeleteRequest(params string[] numbers) => Numbers = numbers.ToList();

		public DeleteRequest(params T[] entities) => AddEntities(entities);

		/// <summary>
		///     创建者组织内码
		/// </summary>
		[JsonProperty("CreateOrgId")]
		public int CreatorOrgId { get; set; }

		/// <summary>
		///     单据编码集合
		/// </summary>
		[JsonProperty("Numbers")]
		public List<string> Numbers { get; set; } = new();

		/// <summary>
		///     单据内码集合
		/// </summary>
		[JsonProperty("Ids")]
		[JsonConverter(typeof(StringCollectionConverter), ',')]
		public List<int> Ids { get; set; } = new();

		/// <summary>
		///     是否启用网控
		/// </summary>
		[JsonProperty("NetworkCtrl")]
		[JsonConverter(typeof(BoolConverter))]
		public bool NetworkControl { get; set; }

		public void AddEntity(T entity) => Ids.Add((int)Key.GetValue(entity));

		public void AddEntities(IEnumerable<T> entities) => Ids.AddRange(entities.Select(entity => (int)Key.GetValue(entity)));
	}
}