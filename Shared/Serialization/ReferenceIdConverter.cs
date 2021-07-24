using System;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace Shared.Serialization {
	public class ReferenceIdConverter<T> : JsonConverter<T> where T : new() {
		public ReferenceIdConverter(string idMemberName) => IdInfo = typeof(T).GetMember(idMemberName).Single();
		public MemberInfo IdInfo { get; }

		public override void WriteJson(JsonWriter writer, T value, JsonSerializer serializer) {
			writer.WriteValue(IdInfo.GetValue(value));
		}

		public override T ReadJson(JsonReader reader, Type objectType, T existingValue, bool hasExistingValue, JsonSerializer serializer) {
			var result = new T();
			object id = serializer.Deserialize(reader, IdInfo.GetValueType());
			IdInfo.SetValue(result, id);
			return result;
		}
	}
}