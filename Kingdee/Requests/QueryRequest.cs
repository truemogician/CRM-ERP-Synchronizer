using System;
using System.Collections.Generic;
using System.Linq;
using Kingdee.Forms;
using Kingdee.Requests.Query;
using Kingdee.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shared.Exceptions;
using Shared.JsonConverters;
using Shared.Utilities;

namespace Kingdee.Requests {
	public class QueryRequest<T> : RequestBase where T : FormBase {
		public QueryRequest() : this(FormMeta<T>.QueryFields) { }

		public QueryRequest(IEnumerable<Field<T>> fields) {
			FormName = FormMeta<T>.Name;
			Fields = fields;
		}

		/// <summary>
		/// </summary>
		[JsonProperty("FormId")]
		public string FormName { get; set; }

		/// <summary>
		/// </summary>
		[JsonProperty("FieldKeys")]
		[JsonConverter(typeof(FieldsConverter))]
		public IEnumerable<Field> Fields { get; set; }

		/// <summary>
		/// </summary>
		[JsonProperty("FilterString")]
		[JsonConverter(typeof(ToStringConverter<Sentence>))]
		public Sentence<T> Filters { get; set; }

		/// <summary>
		/// </summary>
		[JsonProperty("OrderString")]
		public string Orders { get; set; }

		/// <summary>
		/// </summary>
		[JsonProperty("TopRowCount")]
		public int TopRowCount { get; set; }

		/// <summary>
		/// </summary>
		[JsonProperty("StartRow")]
		public int StartRow { get; set; }

		/// <summary>
		/// </summary>
		[JsonProperty("Limit")]
		public int Limit { get; set; }
	}

	public class FieldsConverter : JsonConverter<IEnumerable<Field>> {
		public static readonly StringCollectionConverter CollectionConverter = new(',');

		public override void WriteJson(JsonWriter writer, IEnumerable<Field> value, JsonSerializer serializer) {
			serializer.Converters.Add(CollectionConverter);
			serializer.Serialize(writer, value.Select(field => field.ToString()));
			serializer.Converters.Remove(CollectionConverter);
		}

		public override IEnumerable<Field> ReadJson(JsonReader reader, Type objectType, IEnumerable<Field> existingValue, bool hasExistingValue, JsonSerializer serializer) => throw new NotImplementedException();
	}
}