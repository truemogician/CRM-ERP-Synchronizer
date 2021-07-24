using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Shared.Serialization {
	public class CustomIgnoreResolver : DefaultContractResolver {
		public CustomIgnoreResolver() : this(Array.Empty<Type>()) { }
		public CustomIgnoreResolver(params Type[] ignoreAttributes) => IgnoreAttributes = ignoreAttributes.ToHashSet();
		public HashSet<Type> IgnoreAttributes { get; }

		protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization) {
			var properties = base.CreateProperties(type, memberSerialization);
			foreach (var prop in properties) {
				var attrs = prop.AttributeProvider!.GetAttributes(true);
				if (!prop.Ignored && attrs.Any(attr => IgnoreAttributes.Contains(attr.GetType())))
					prop.Ignored = true;
			}
			return properties;
		}
	}
}