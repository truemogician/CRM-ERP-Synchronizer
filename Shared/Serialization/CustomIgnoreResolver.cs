using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Shared.Serialization {
	public class CustomIgnoreResolver : DefaultContractResolver {
		public CustomIgnoreResolver() : this(Array.Empty<Type>()) { }

		public CustomIgnoreResolver(params Type[] ignoreAttributes) => IgnoreAttributes = ignoreAttributes.ToHashSet();

		public HashSet<Type> IgnoreAttributes { get; }

		protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization) {
			var result = base.CreateProperty(member, memberSerialization);
			if (!result.Ignored && IgnoreAttributes.Any(member.IsDefined))
				result.Ignored = true;
			return result;
		}
	}
}