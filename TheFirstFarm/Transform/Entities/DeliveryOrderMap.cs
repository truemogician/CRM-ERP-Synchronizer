using TheFirstFarm.Models.FXiaoKe;
using TheFirstFarm.Models.Kingdee;

namespace TheFirstFarm.Transform.Entities {
	[Map(typeof(DeliveryOrder), typeof(OutboundOrder))]
	public class DeliveryOrderMap : KIdMap {
		public DeliveryOrderMap() { }

		public DeliveryOrderMap(int kId, string number = null, string fId = null) : base(kId, fId) => Number = number;

		[MapReference(FName = nameof(DeliveryOrder.Number), KName = nameof(OutboundOrder.Number))]
		public string Number { get; set; }
	}
}