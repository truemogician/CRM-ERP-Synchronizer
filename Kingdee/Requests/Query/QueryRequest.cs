using System;
using System.Collections.Generic;
using System.Linq;
using Kingdee.Forms;
using Kingdee.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shared.Exceptions;
using Shared.JsonConverters;
using Shared.Utilities;

namespace Kingdee.Requests.Query {
	public class QueryRequest<T> : RequestBase where T : FormBase {
		public QueryRequest() : this(typeof(T).GetJsonPropertyNames()) { }

		public QueryRequest(IEnumerable<string> fields) {
			FormName = typeof(T).GetFormName();
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
		public IEnumerable<string> Fields { get; set; }

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

	public class FieldsConverter : JsonConverter<IEnumerable<string>> {
		public override void WriteJson(JsonWriter writer, IEnumerable<string> value, JsonSerializer serializer) {
			writer.WriteValue(string.Join(',', value));
		}

		public override IEnumerable<string> ReadJson(JsonReader reader, Type objectType, IEnumerable<string> existingValue, bool hasExistingValue, JsonSerializer serializer) {
			var token = JToken.Load(reader);
			if (token.Type is JTokenType.Null or JTokenType.Undefined)
				return null;
			if (token.Type != JTokenType.String)
				throw new JTokenTypeException(token, JTokenType.String);
			string str = token.Value<string>();
			return str?.Split(',').Select(field => field.Trim());
		}
	}
}