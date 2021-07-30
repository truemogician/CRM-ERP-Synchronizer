using System.ComponentModel.DataAnnotations;
using Kingdee.Forms;
using Newtonsoft.Json;
using Shared.Serialization;
using TheFirstFarm.Models.Common;
using TheFirstFarm.Models.FXiaoKe;

namespace TheFirstFarm.Models.Kingdee {
	[Form("BD_MATERIAL")]
	public class Material : FormBase {
		[JsonProperty("FMaterialId")]
		[Key]
		[Required]
		public int Id { get; set; }

		/// <summary>
		///		名称
		/// </summary>
		[JsonProperty("FName")]
		[Required]
		public string Name { get; set; }

		/// <summary>
		///		编码
		/// </summary>
		[JsonProperty("FNumber")]
		public string Number { get; set; }

		/// <summary>
		///		规格型号
		/// </summary>
		[JsonProperty("FSpecification")]
		public string Specification { get; set; }

		/// <summary>
		///		条码
		/// </summary>
		[JsonProperty("FBarCode")]
		public string BarCode { get; set; }

		/// <summary>
		///		物料分组
		/// </summary>
		[JsonProperty("FMaterialGroup")]
		public NumberWrapper Group { get; set; }

		/// <summary>
		///		物料属性
		/// </summary>
		[JsonProperty("FErpClsID")]
		public ProductProperty MaterialProperty { get; set; }

		/// <summary>
		///		基本单位
		/// </summary>
		[JsonProperty("FBaseUnitId")]
		[Required]
		public NameWrapper Unit { get; set; }

		/// <summary>
		///		长
		/// </summary>
		[JsonProperty("FLength")]
		public decimal? Length { get; set; }

		/// <summary>
		///		宽
		/// </summary>
		[JsonProperty("FWidth")]
		public decimal? Width { get; set; }

		/// <summary>
		///		高
		/// </summary>
		[JsonProperty("FHeight")]
		public decimal? Height { get; set; }

		/// <summary>
		///		保质期单位
		/// </summary>
		[JsonProperty("FExpUnit")]
		[JsonConverter(typeof(EnumValueConverter), Platform.Kingdee)]
		public ShelfLifeUnit ShelfLifeUnit { get; set; }

		/// <summary>
		///		保质期
		/// </summary>
		[JsonProperty("FExpPeriod")]
		public decimal? ShelfLife { get; set; }

		/// <summary>
		///		起订量
		/// </summary>
		[JsonProperty("FOrderQty")]
		public decimal? MinOrderQuantity { get; set; }

		/// <summary>
		///		允许退货
		/// </summary>
		[JsonProperty("FIsReturn")]
		public bool? AllowReturn { get; set; }
	}
}