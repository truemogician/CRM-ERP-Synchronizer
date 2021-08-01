using TheFirstFarm.Models.FXiaoKe;
using TheFirstFarm.Models.Kingdee;

namespace TheFirstFarm.Transform.Entities {
	[Map(typeof(Product), typeof(Material))]
	public class ProductMap : KIdMap {
		public ProductMap() { }

		public ProductMap(int kId, string number = null, string fId = null) : base(kId, fId) => Number = number;

		[MapReference(FName = nameof(Product.Number), KName = nameof(Material.Number))]
		public string Number { get; set; }
	}
}