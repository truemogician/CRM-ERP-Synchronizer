using System;
using System.Collections.Generic;
using System.Linq;
using Kingdee.Forms;
using Kingdee.Requests.Query;
using Newtonsoft.Json;
using Shared.Serialization;

namespace Kingdee.Requests {
	public class QueryRequest<T> : RequestBase where T : FormBase {
		public QueryRequest(int limit = 0, int offset = 0) : this(FormMeta<T>.QueryFields) {
			Limit = limit;
			Offset = offset;
		}

		public QueryRequest(Sentence<T> filter, int limit = 0, int offset = 0) : this(limit, offset) => Filter = filter;

		public QueryRequest(List<Field<T>> fields) {
			FormName = FormMeta<T>.Name;
			fields.Sort((a, b) => string.CompareOrdinal(a.ToString(), b.ToString()));
			Fields = fields;
		}

		/// <summary>
		/// </summary>
		[JsonProperty("FormId")]
		public string FormName { get; }

		/// <summary>
		/// </summary>
		[JsonProperty("FieldKeys")]
		[JsonConverter(typeof(FieldsConverter))]
		public IEnumerable<Field> Fields { get; set; }

		/// <summary>
		/// </summary>
		[JsonProperty("FilterString")]
		[JsonConverter(typeof(ToStringConverter<Sentence>))]
		public Sentence<T> Filter { get; set; }

		/// <summary>
		/// </summary>
		[JsonProperty("OrderString")]
		public string Order { get; set; }

		/// <summary>
		/// </summary>
		[JsonProperty("TopRowCount")]
		[JsonIgnore]
		public int TopRowCount => Limit + Offset;

		/// <summary>
		/// </summary>
		[JsonProperty("StartRow")]
		public int Offset { get; set; }

		/// <summary>
		/// </summary>
		[JsonProperty("Limit")]
		public int Limit { get; set; }
	}

	public class FieldsConverter : JsonConverter<IEnumerable<Field>> {
		public static readonly StringCollectionConverter CollectionConverter = new(',');

		public override void WriteJson(JsonWriter writer, IEnumerable<Field> value, JsonSerializer serializer) {
			serializer.Converters.Add(CollectionConverter);
			serializer.Serialize(writer, value.Select(field => field.ToString("json")));
			serializer.Converters.Remove(CollectionConverter);
		}

		public override IEnumerable<Field> ReadJson(JsonReader reader, Type objectType, IEnumerable<Field> existingValue, bool hasExistingValue, JsonSerializer serializer) => throw new NotImplementedException();
	}
}