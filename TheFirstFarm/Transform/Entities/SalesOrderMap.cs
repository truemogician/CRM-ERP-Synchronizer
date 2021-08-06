namespace TheFirstFarm.Transform.Entities {
	using FSalesOrder = Models.FXiaoKe.SalesOrder;
	using KSalesOrder = Models.Kingdee.SalesOrder;

	[Map(typeof(FSalesOrder), typeof(KSalesOrder))]
	public class SalesOrderMap : KIdMap {
		public SalesOrderMap() { }

		public SalesOrderMap(int kId, string number = null, string fId = null) : base(kId, fId) => Number = number;

		[MapReference(FName = nameof(FSalesOrder.KingdeeNumber), KName = nameof(KSalesOrder.Number))]
		public string Number { get; set; }
	}
}