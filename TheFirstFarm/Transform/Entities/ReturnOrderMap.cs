using FReturnOrder = TheFirstFarm.Models.FXiaoKe.ReturnOrder;
using KReturnOrder = TheFirstFarm.Models.Kingdee.ReturnOrder;

namespace TheFirstFarm.Transform.Entities {
	[Map(typeof(FReturnOrder), typeof(KReturnOrder))]
	public class ReturnOrderMap : IdMap {
		public ReturnOrderMap() { }

		public ReturnOrderMap(string kId, string number = null, int? fId = null) : base(kId, fId) => Number = number;

		[MapReference(FName = nameof(FReturnOrder.Number), KName = nameof(KReturnOrder.Number))]
		public string Number { get; set; }
	}
}