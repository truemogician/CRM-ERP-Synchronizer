using System;
using System.Collections.Generic;
using System.Linq;
using FXiaoKe.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Shared.Serialization;

namespace FXiaoKe {
	public class ContractResolver : CustomIgnoreResolver {
		public ContractResolver() : base(typeof(SubModelAttribute)) { }

		public bool IgnoreGenerated { get; set; }

		protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization) {
			var props = base.CreateProperties(type, memberSerialization);
			if (!IgnoreGenerated)
				return props;
			foreach (var prop in props.Where(prop => !prop.Ignored))
				if (prop.IsDefined<GeneratedAttribute>())
					prop.Ignored = true;
			return props;
		}
	}
}