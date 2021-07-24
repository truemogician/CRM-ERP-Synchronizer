using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using JsonIgnoreAttribute = Newtonsoft.Json.JsonIgnoreAttribute;

namespace Shared.Serialization {
	public class JsonIncludeResolver : DefaultContractResolver {
		protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization) {
			var members = type.GetMembers(MemberTypes.Field | MemberTypes.Property);
			bool blackList = members.Any(member => member.IsDefined(typeof(JsonIgnoreAttribute)));
			bool whiteList = members.Any(member => member.IsDefined(typeof(JsonIncludeAttribute)));
			if (blackList && whiteList)
				throw new InvalidOperationException($"Mixed use of {nameof(JsonIgnoreAttribute)} and {nameof(JsonIncludeAttribute)}");
			var properties = base.CreateProperties(type, memberSerialization);
			if (!whiteList)
				return properties;
			return properties.Select(
					prop => {
						if (prop.AttributeProvider!.GetAttributes(typeof(JsonIncludeAttribute), true).Count == 0)
							prop.Ignored = true;
						return prop;
					}
				)
				.AsList();
		}
	}
}