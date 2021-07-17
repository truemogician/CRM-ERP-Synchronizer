using System;

namespace Kingdee {
	[Serializable]
	public class DataCenter {
		public string Id { get; set; }

		public string Number { get; set; }

		public string Name { get; set; }

		public override string ToString() => Name;
	}
}