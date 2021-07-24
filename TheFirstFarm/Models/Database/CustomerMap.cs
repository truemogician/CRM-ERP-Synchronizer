namespace TheFirstFarm.Models.Database {
	public class CustomerMap : IdMap<string, string> {
		public CustomerMap() { }
		public CustomerMap(string fXiaoKeId, string kingdeeId) : base(fXiaoKeId, kingdeeId) { }
	}
}