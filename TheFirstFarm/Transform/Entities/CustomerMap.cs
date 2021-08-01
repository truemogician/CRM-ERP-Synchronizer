using FCustomer = TheFirstFarm.Models.FXiaoKe.Customer;
using KCustomer = TheFirstFarm.Models.Kingdee.Customer;

namespace TheFirstFarm.Transform.Entities {
	[Map(typeof(FCustomer), typeof(KCustomer), FExtKeyName = nameof(FCustomer.KingdeeId))]
	public class CustomerMap : FIdMap {
		public CustomerMap() { }

		public CustomerMap(string fId, string number = null, int? kId = null) : base(fId, kId) => Number = number;

		[MapReference(FName = nameof(FCustomer.Number), KName = nameof(KCustomer.Number))]
		public string Number { get; set; }
	}
}