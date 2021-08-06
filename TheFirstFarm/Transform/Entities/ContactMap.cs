using FContact = TheFirstFarm.Models.FXiaoKe.Contact;
using KContact = TheFirstFarm.Models.Kingdee.Contact;

namespace TheFirstFarm.Transform.Entities {
	[Map(typeof(FContact), typeof(KContact))]
	public class ContactMap : FIdMap {
		public ContactMap() { }

		public ContactMap(string fId, string number = null, int? kId = null) : base(fId, kId) => Number = number;

		[MapReference(FName = nameof(FContact.Number), KName = nameof(KContact.Number))]
		public string Number { get; set; }
	}
}