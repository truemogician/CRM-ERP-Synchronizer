using FXiaoKe.Models;

namespace TheFirstFarm.Transform.Entities {
	[Map(typeof(Staff))]
	public class StaffMap : FIdMap {
		public StaffMap() { }

		public StaffMap(string openUserId, string number) : base(openUserId) => Number = number;

		[MapReference(FName = nameof(Staff.Number))]
		public string Number { get; set; }
	}
}