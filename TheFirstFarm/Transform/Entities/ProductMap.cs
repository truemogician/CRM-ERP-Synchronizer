using TheFirstFarm.Models.FXiaoKe;
using TheFirstFarm.Models.Kingdee;

namespace TheFirstFarm.Transform.Entities {
	[Map(typeof(Product), typeof(Material))]
	public class ProductMap : IdMap {
		public ProductMap() { }

		public ProductMap(string fId, string number = null, int? kId = null) : base(fId, kId) => Number = number;

		[MapReference(FName = nameof(Product.Number), KName = nameof(Material.Number))]
		public string Number { get; set; }
	}
}