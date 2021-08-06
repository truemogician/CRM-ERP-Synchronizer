using TheFirstFarm.Models.FXiaoKe;
using TheFirstFarm.Models.Kingdee;

namespace TheFirstFarm.Transform.Entities {
	[Map(typeof(Invoice), typeof(ReceivableBill))]
	public class InvoiceMap : KIdMap {
		public InvoiceMap() { }

		public InvoiceMap(int kId, string number = null, string fId = null) : base(kId, fId) => Number = number;

		[MapReference(FName = nameof(Invoice.Number), KName = nameof(ReceivableBill.Number))]
		public string Number { get; set; }
	}
}