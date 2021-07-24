namespace TheFirstFarm.Models.Database {
	public class StaffMap : IdMap<string, string> {
		public StaffMap() { }
		public StaffMap(string fXiaoKeId, string kingdeeId) : base(fXiaoKeId, kingdeeId) { }
	}
}