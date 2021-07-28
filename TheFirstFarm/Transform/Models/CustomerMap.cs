namespace TheFirstFarm.Transform.Models {
	public class CustomerMap : IdMap<string, string> {
		public CustomerMap() { }
		public CustomerMap(string fXiaoKeId, string kingdeeId) : base(fXiaoKeId, kingdeeId) { }
	}
}