using Shared.Serialization;

namespace TheFirstFarm.Models.Common {
	public enum ProductCategory {
		/// <summary>
		/// 	原材料/生鲜/根茎类
		/// </summary>
		[EnumValue("16", Platform.FXiaoKe)]
		[EnumValue("YCL.SX.01", Platform.Kingdee)]
		Item16,

		/// <summary>
		/// 	原材料/生鲜/茄果类
		/// </summary>
		[EnumValue("17", Platform.FXiaoKe)]
		[EnumValue("YCL.SX.02", Platform.Kingdee)]
		Item17,

		/// <summary>
		/// 	原材料/生鲜/叶菜类
		/// </summary>
		[EnumValue("18", Platform.FXiaoKe)]
		[EnumValue("YCL.SX.03", Platform.Kingdee)]
		Item18,

		/// <summary>
		/// 	原材料/生鲜/水果类
		/// </summary>
		[EnumValue("19", Platform.FXiaoKe)]
		[EnumValue("YCL.SX.04", Platform.Kingdee)]
		Item19,

		/// <summary>
		/// 	原材料/生鲜/西甜瓜类
		/// </summary>
		[EnumValue("20", Platform.FXiaoKe)]
		[EnumValue("YCL.SX.05", Platform.Kingdee)]
		Item20,

		/// <summary>
		/// 	原材料/生鲜/肉禽蛋类
		/// </summary>
		[EnumValue("21", Platform.FXiaoKe)]
		[EnumValue("YCL.SX.06", Platform.Kingdee)]
		Item21,

		/// <summary>
		/// 	原材料/生鲜/水产类
		/// </summary>
		[EnumValue("22", Platform.FXiaoKe)]
		[EnumValue("YCL.SX.07", Platform.Kingdee)]
		Item22,

		/// <summary>
		/// 	原材料/生鲜/香辛料
		/// </summary>
		[EnumValue("23", Platform.FXiaoKe)]
		[EnumValue("YCL.SX.08", Platform.Kingdee)]
		Item23,

		/// <summary>
		/// 	原材料/生鲜/盆栽类
		/// </summary>
		[EnumValue("24", Platform.FXiaoKe)]
		[EnumValue("YCL.SX.09", Platform.Kingdee)]
		Item24,

		/// <summary>
		/// 	原材料/生鲜/菌菇类
		/// </summary>
		[EnumValue("25", Platform.FXiaoKe)]
		[EnumValue("YCL.SX.10", Platform.Kingdee)]
		Item25,

		/// <summary>
		/// 	原材料/生鲜/其他
		/// </summary>
		[EnumValue("26", Platform.FXiaoKe)]
		[EnumValue("YCL.SX.11", Platform.Kingdee)]
		Item26,

		/// <summary>
		/// 	原材料/生鲜
		/// </summary>
		[EnumValue("15", Platform.FXiaoKe)]
		[EnumValue("YCL.SX", Platform.Kingdee)]
		Item15,

		/// <summary>
		/// 	原材料/农资耗材/种子种苗
		/// </summary>
		[EnumValue("28", Platform.FXiaoKe)]
		[EnumValue("YCL.NZ.01", Platform.Kingdee)]
		Item28,

		/// <summary>
		/// 	原材料/农资耗材/药剂
		/// </summary>
		[EnumValue("29", Platform.FXiaoKe)]
		[EnumValue("YCL.NZ.02", Platform.Kingdee)]
		Item29,

		/// <summary>
		/// 	原材料/农资耗材/肥料
		/// </summary>
		[EnumValue("30", Platform.FXiaoKe)]
		[EnumValue("YCL.NZ.03", Platform.Kingdee)]
		Item30,

		/// <summary>
		/// 	原材料/农资耗材/饲料
		/// </summary>
		[EnumValue("31", Platform.FXiaoKe)]
		[EnumValue("YCL.NZ.04", Platform.Kingdee)]
		Item31,

		/// <summary>
		/// 	原材料/农资耗材/农资耗材
		/// </summary>
		[EnumValue("32", Platform.FXiaoKe)]
		[EnumValue("YCL.NZ.05", Platform.Kingdee)]
		Item32,

		/// <summary>
		/// 	原材料/农资耗材/其他
		/// </summary>
		[EnumValue("33", Platform.FXiaoKe)]
		[EnumValue("YCL.NZ.99", Platform.Kingdee)]
		Item33,

		/// <summary>
		/// 	原材料/农资耗材
		/// </summary>
		[EnumValue("27", Platform.FXiaoKe)]
		[EnumValue("YCL.NZ", Platform.Kingdee)]
		Item27,

		/// <summary>
		/// 	原材料/粮油/米面
		/// </summary>
		[EnumValue("35", Platform.FXiaoKe)]
		[EnumValue("YCL.LY.01", Platform.Kingdee)]
		Item35,

		/// <summary>
		/// 	原材料/粮油/杂粮
		/// </summary>
		[EnumValue("36", Platform.FXiaoKe)]
		[EnumValue("YCL.LY.02", Platform.Kingdee)]
		Item36,

		/// <summary>
		/// 	原材料/粮油/油
		/// </summary>
		[EnumValue("37", Platform.FXiaoKe)]
		[EnumValue("YCL.LY.03", Platform.Kingdee)]
		Item37,

		/// <summary>
		/// 	原材料/粮油/其他
		/// </summary>
		[EnumValue("38", Platform.FXiaoKe)]
		[EnumValue("YCL.LY.99", Platform.Kingdee)]
		Item38,

		/// <summary>
		/// 	原材料/粮油
		/// </summary>
		[EnumValue("34", Platform.FXiaoKe)]
		[EnumValue("YCL.LY", Platform.Kingdee)]
		Item34,

		/// <summary>
		/// 	原材料/食品/儿童零食
		/// </summary>
		[EnumValue("40", Platform.FXiaoKe)]
		[EnumValue("YCL.SP.01", Platform.Kingdee)]
		Item40,

		/// <summary>
		/// 	原材料/食品/即食代餐
		/// </summary>
		[EnumValue("41", Platform.FXiaoKe)]
		[EnumValue("YCL.SP.02", Platform.Kingdee)]
		Item41,

		/// <summary>
		/// 	原材料/食品/节气食品
		/// </summary>
		[EnumValue("42", Platform.FXiaoKe)]
		[EnumValue("YCL.SP.03", Platform.Kingdee)]
		Item42,

		/// <summary>
		/// 	原材料/食品/其他
		/// </summary>
		[EnumValue("43", Platform.FXiaoKe)]
		[EnumValue("YCL.SP.99", Platform.Kingdee)]
		Item43,

		/// <summary>
		/// 	原材料/食品
		/// </summary>
		[EnumValue("39", Platform.FXiaoKe)]
		[EnumValue("YCL.SP", Platform.Kingdee)]
		Item39,

		/// <summary>
		/// 	原材料
		/// </summary>
		[EnumValue("1", Platform.FXiaoKe)]
		[EnumValue("YCL", Platform.Kingdee)]
		Item1,

		/// <summary>
		/// 	半成品/生鲜/根茎类
		/// </summary>
		[EnumValue("45", Platform.FXiaoKe)]
		[EnumValue("BCP.SX.01", Platform.Kingdee)]
		Item45,

		/// <summary>
		/// 	半成品/生鲜/茄果类
		/// </summary>
		[EnumValue("46", Platform.FXiaoKe)]
		[EnumValue("BCP.SX.02", Platform.Kingdee)]
		Item46,

		/// <summary>
		/// 	半成品/生鲜/叶菜类
		/// </summary>
		[EnumValue("47", Platform.FXiaoKe)]
		[EnumValue("BCP.SX.03", Platform.Kingdee)]
		Item47,

		/// <summary>
		/// 	半成品/生鲜/水果类
		/// </summary>
		[EnumValue("48", Platform.FXiaoKe)]
		[EnumValue("BCP.SX.04", Platform.Kingdee)]
		Item48,

		/// <summary>
		/// 	半成品/生鲜/西甜瓜类
		/// </summary>
		[EnumValue("49", Platform.FXiaoKe)]
		[EnumValue("BCP.SX.05", Platform.Kingdee)]
		Item49,

		/// <summary>
		/// 	半成品/生鲜/肉禽蛋类
		/// </summary>
		[EnumValue("50", Platform.FXiaoKe)]
		[EnumValue("BCP.SX.06", Platform.Kingdee)]
		Item50,

		/// <summary>
		/// 	半成品/生鲜/其他
		/// </summary>
		[EnumValue("51", Platform.FXiaoKe)]
		[EnumValue("BCP.SX.99", Platform.Kingdee)]
		Item51,

		/// <summary>
		/// 	半成品/生鲜
		/// </summary>
		[EnumValue("44", Platform.FXiaoKe)]
		[EnumValue("BCP.SX", Platform.Kingdee)]
		Item44,

		/// <summary>
		/// 	半成品/粮油/大米
		/// </summary>
		[EnumValue("53", Platform.FXiaoKe)]
		[EnumValue("BCP.LY.01", Platform.Kingdee)]
		Item53,

		/// <summary>
		/// 	半成品/粮油/杂粮
		/// </summary>
		[EnumValue("54", Platform.FXiaoKe)]
		[EnumValue("BCP.LY.02", Platform.Kingdee)]
		Item54,

		/// <summary>
		/// 	半成品/粮油/油
		/// </summary>
		[EnumValue("55", Platform.FXiaoKe)]
		[EnumValue("BCP.LY.03", Platform.Kingdee)]
		Item55,

		/// <summary>
		/// 	半成品/粮油/其他
		/// </summary>
		[EnumValue("56", Platform.FXiaoKe)]
		[EnumValue("BCP.LY.99", Platform.Kingdee)]
		Item56,

		/// <summary>
		/// 	半成品/粮油
		/// </summary>
		[EnumValue("52", Platform.FXiaoKe)]
		[EnumValue("BCP.LY", Platform.Kingdee)]
		Item52,

		/// <summary>
		/// 	半成品/食品/儿童零食
		/// </summary>
		[EnumValue("58", Platform.FXiaoKe)]
		[EnumValue("BCP.SP.01", Platform.Kingdee)]
		Item58,

		/// <summary>
		/// 	半成品/食品/即食代餐
		/// </summary>
		[EnumValue("59", Platform.FXiaoKe)]
		[EnumValue("BCP.SP.02", Platform.Kingdee)]
		Item59,

		/// <summary>
		/// 	半成品/食品/节气食品
		/// </summary>
		[EnumValue("60", Platform.FXiaoKe)]
		[EnumValue("BCP.SP.03", Platform.Kingdee)]
		Item60,

		/// <summary>
		/// 	半成品/食品/其他
		/// </summary>
		[EnumValue("61", Platform.FXiaoKe)]
		[EnumValue("BCP.SP.99", Platform.Kingdee)]
		Item61,

		/// <summary>
		/// 	半成品/食品
		/// </summary>
		[EnumValue("57", Platform.FXiaoKe)]
		[EnumValue("BCP.SP", Platform.Kingdee)]
		Item57,

		/// <summary>
		/// 	半成品
		/// </summary>
		[EnumValue("2", Platform.FXiaoKe)]
		[EnumValue("BCP", Platform.Kingdee)]
		Item2,

		/// <summary>
		/// 	产成品/生鲜/挑拣菜
		/// </summary>
		[EnumValue("63", Platform.FXiaoKe)]
		[EnumValue("CCP.SX.01", Platform.Kingdee)]
		Item63,

		/// <summary>
		/// 	产成品/生鲜/包装菜
		/// </summary>
		[EnumValue("64", Platform.FXiaoKe)]
		[EnumValue("CCP.SX.02", Platform.Kingdee)]
		Item64,

		/// <summary>
		/// 	产成品/生鲜/即用菜
		/// </summary>
		[EnumValue("65", Platform.FXiaoKe)]
		[EnumValue("CCP.SX.03", Platform.Kingdee)]
		Item65,

		/// <summary>
		/// 	产成品/生鲜/即食菜
		/// </summary>
		[EnumValue("66", Platform.FXiaoKe)]
		[EnumValue("CCP.SX.04", Platform.Kingdee)]
		Item66,

		/// <summary>
		/// 	产成品/生鲜/盆栽类
		/// </summary>
		[EnumValue("67", Platform.FXiaoKe)]
		[EnumValue("CCP.SX.05", Platform.Kingdee)]
		Item67,

		/// <summary>
		/// 	产成品/生鲜/其他
		/// </summary>
		[EnumValue("68", Platform.FXiaoKe)]
		[EnumValue("CCP.SX.99", Platform.Kingdee)]
		Item68,

		/// <summary>
		/// 	产成品/生鲜
		/// </summary>
		[EnumValue("62", Platform.FXiaoKe)]
		[EnumValue("CCP.SX", Platform.Kingdee)]
		Item62,

		/// <summary>
		/// 	产成品/粮油/大米
		/// </summary>
		[EnumValue("70", Platform.FXiaoKe)]
		[EnumValue("CCP.LY.01", Platform.Kingdee)]
		Item70,

		/// <summary>
		/// 	产成品/粮油/杂粮
		/// </summary>
		[EnumValue("71", Platform.FXiaoKe)]
		[EnumValue("CCP.LY.02", Platform.Kingdee)]
		Item71,

		/// <summary>
		/// 	产成品/粮油/油
		/// </summary>
		[EnumValue("72", Platform.FXiaoKe)]
		[EnumValue("CCP.LY.03", Platform.Kingdee)]
		Item72,

		/// <summary>
		/// 	产成品/粮油/其他
		/// </summary>
		[EnumValue("73", Platform.FXiaoKe)]
		[EnumValue("CCP.LY.99", Platform.Kingdee)]
		Item73,

		/// <summary>
		/// 	产成品/粮油
		/// </summary>
		[EnumValue("69", Platform.FXiaoKe)]
		[EnumValue("CCP.LY", Platform.Kingdee)]
		Item69,

		/// <summary>
		/// 	产成品/食品/儿童零食
		/// </summary>
		[EnumValue("75", Platform.FXiaoKe)]
		[EnumValue("CCP.SP.01", Platform.Kingdee)]
		Item75,

		/// <summary>
		/// 	产成品/食品/轻食代餐
		/// </summary>
		[EnumValue("76", Platform.FXiaoKe)]
		[EnumValue("CCP.SP.02", Platform.Kingdee)]
		Item76,

		/// <summary>
		/// 	产成品/食品/节气食品
		/// </summary>
		[EnumValue("77", Platform.FXiaoKe)]
		[EnumValue("CCP.SP.03", Platform.Kingdee)]
		Item77,

		/// <summary>
		/// 	产成品/食品/其他
		/// </summary>
		[EnumValue("78", Platform.FXiaoKe)]
		[EnumValue("CCP.SP.99", Platform.Kingdee)]
		Item78,

		/// <summary>
		/// 	产成品/食品
		/// </summary>
		[EnumValue("74", Platform.FXiaoKe)]
		[EnumValue("CCP.SP", Platform.Kingdee)]
		Item74,

		/// <summary>
		/// 	产成品/旅游相关/商品类
		/// </summary>
		[EnumValue("80", Platform.FXiaoKe)]
		[EnumValue("CCP.LV.01", Platform.Kingdee)]
		Item80,

		/// <summary>
		/// 	产成品/旅游相关/服务类
		/// </summary>
		[EnumValue("81", Platform.FXiaoKe)]
		[EnumValue("CCP.LV.02", Platform.Kingdee)]
		Item81,

		/// <summary>
		/// 	产成品/旅游相关
		/// </summary>
		[EnumValue("79", Platform.FXiaoKe)]
		[EnumValue("CCP.LV", Platform.Kingdee)]
		Item79,

		/// <summary>
		/// 	产成品/卡券/卡券
		/// </summary>
		[EnumValue("83", Platform.FXiaoKe)]
		[EnumValue("CCP.KQ.01", Platform.Kingdee)]
		Item83,

		/// <summary>
		/// 	产成品/卡券
		/// </summary>
		[EnumValue("82", Platform.FXiaoKe)]
		[EnumValue("CCP.KQ", Platform.Kingdee)]
		Item82,

		/// <summary>
		/// 	产成品
		/// </summary>
		[EnumValue("3", Platform.FXiaoKe)]
		[EnumValue("CCP", Platform.Kingdee)]
		Item3,

		/// <summary>
		/// 	包装类/包装盒/物流箱
		/// </summary>
		[EnumValue("85", Platform.FXiaoKe)]
		[EnumValue("BZL.HZ.01", Platform.Kingdee)]
		Item85,

		/// <summary>
		/// 	包装类/包装盒/包装礼盒
		/// </summary>
		[EnumValue("86", Platform.FXiaoKe)]
		[EnumValue("BZL.HZ.02", Platform.Kingdee)]
		Item86,

		/// <summary>
		/// 	包装类/包装盒/塑料盒
		/// </summary>
		[EnumValue("87", Platform.FXiaoKe)]
		[EnumValue("BZL.HZ.03", Platform.Kingdee)]
		Item87,

		/// <summary>
		/// 	包装类/包装盒
		/// </summary>
		[EnumValue("84", Platform.FXiaoKe)]
		[EnumValue("BZL.HZ", Platform.Kingdee)]
		Item84,

		/// <summary>
		/// 	包装类/标签/有机标
		/// </summary>
		[EnumValue("89", Platform.FXiaoKe)]
		[EnumValue("BZL.BQ.01", Platform.Kingdee)]
		Item89,

		/// <summary>
		/// 	包装类/标签/信息标
		/// </summary>
		[EnumValue("90", Platform.FXiaoKe)]
		[EnumValue("BZL.BQ.02", Platform.Kingdee)]
		Item90,

		/// <summary>
		/// 	包装类/标签
		/// </summary>
		[EnumValue("88", Platform.FXiaoKe)]
		[EnumValue("BZL.BQ", Platform.Kingdee)]
		Item88,

		/// <summary>
		/// 	包装类/包装袋/防雾袋
		/// </summary>
		[EnumValue("92", Platform.FXiaoKe)]
		[EnumValue("BZL.DZ.01", Platform.Kingdee)]
		Item92,

		/// <summary>
		/// 	包装类/包装袋/卷膜袋
		/// </summary>
		[EnumValue("93", Platform.FXiaoKe)]
		[EnumValue("BZL.DZ.02", Platform.Kingdee)]
		Item93,

		/// <summary>
		/// 	包装类/包装袋/牛皮纸袋
		/// </summary>
		[EnumValue("94", Platform.FXiaoKe)]
		[EnumValue("BZL.DZ.03", Platform.Kingdee)]
		Item94,

		/// <summary>
		/// 	包装类/包装袋/真空袋
		/// </summary>
		[EnumValue("95", Platform.FXiaoKe)]
		[EnumValue("BZL.DZ.04", Platform.Kingdee)]
		Item95,

		/// <summary>
		/// 	包装类/包装袋/辅助包材
		/// </summary>
		[EnumValue("96", Platform.FXiaoKe)]
		[EnumValue("BZL.DZ.05", Platform.Kingdee)]
		Item96,

		/// <summary>
		/// 	包装类/包装袋
		/// </summary>
		[EnumValue("91", Platform.FXiaoKe)]
		[EnumValue("BZL.DZ", Platform.Kingdee)]
		Item91,

		/// <summary>
		/// 	包装类
		/// </summary>
		[EnumValue("4", Platform.FXiaoKe)]
		[EnumValue("BZL", Platform.Kingdee)]
		Item4,

		/// <summary>
		/// 	固定资产/房屋建筑物
		/// </summary>
		[EnumValue("97", Platform.FXiaoKe)]
		Item97,

		/// <summary>
		/// 	固定资产/机器设备
		/// </summary>
		[EnumValue("98", Platform.FXiaoKe)]
		Item98,

		/// <summary>
		/// 	固定资产/电子设备
		/// </summary>
		[EnumValue("99", Platform.FXiaoKe)]
		Item99,

		/// <summary>
		/// 	固定资产/家具设备
		/// </summary>
		[EnumValue("100", Platform.FXiaoKe)]
		Item100,

		/// <summary>
		/// 	固定资产/运输设备
		/// </summary>
		[EnumValue("101", Platform.FXiaoKe)]
		Item101,

		/// <summary>
		/// 	固定资产
		/// </summary>
		[EnumValue("5", Platform.FXiaoKe)]
		Item5,

		/// <summary>
		/// 	费用类/工程物料
		/// </summary>
		[EnumValue("102", Platform.FXiaoKe)]
		[EnumValue("FYL.GC.01", Platform.Kingdee)]
		Item102,

		/// <summary>
		/// 	费用类/清洁物料
		/// </summary>
		[EnumValue("103", Platform.FXiaoKe)]
		[EnumValue("FYL.QJ.01", Platform.Kingdee)]
		Item103,

		/// <summary>
		/// 	费用类/办公用品
		/// </summary>
		[EnumValue("104", Platform.FXiaoKe)]
		[EnumValue("FYL.BG.01", Platform.Kingdee)]
		Item104,

		/// <summary>
		/// 	费用类/营销物料
		/// </summary>
		[EnumValue("105", Platform.FXiaoKe)]
		[EnumValue("FYL.YX.01", Platform.Kingdee)]
		Item105,

		/// <summary>
		/// 	费用类/福利物料
		/// </summary>
		[EnumValue("106", Platform.FXiaoKe)]
		[EnumValue("FYL.FL.01", Platform.Kingdee)]
		Item106,

		/// <summary>
		/// 	费用类/低值易耗品
		/// </summary>
		[EnumValue("107", Platform.FXiaoKe)]
		[EnumValue("FYL.DZ.01", Platform.Kingdee)]
		Item107,

		/// <summary>
		/// 	费用类
		/// </summary>
		[EnumValue("7", Platform.FXiaoKe)]
		[EnumValue("FYL", Platform.Kingdee)]
		Item7,

		/// <summary>
		/// 	厨房原材料/干货
		/// </summary>
		[EnumValue("108", Platform.FXiaoKe)]
		[EnumValue("CFY.GH", Platform.Kingdee)]
		Item108,

		/// <summary>
		/// 	厨房原材料/副食调料
		/// </summary>
		[EnumValue("109", Platform.FXiaoKe)]
		[EnumValue("CFY.TL", Platform.Kingdee)]
		Item109,

		/// <summary>
		/// 	厨房原材料/水果甜品
		/// </summary>
		[EnumValue("110", Platform.FXiaoKe)]
		[EnumValue("CFY.SG", Platform.Kingdee)]
		Item110,

		/// <summary>
		/// 	厨房原材料/蔬菜
		/// </summary>
		[EnumValue("111", Platform.FXiaoKe)]
		[EnumValue("CFY.SC", Platform.Kingdee)]
		Item111,

		/// <summary>
		/// 	厨房原材料/肉类
		/// </summary>
		[EnumValue("112", Platform.FXiaoKe)]
		[EnumValue("CFY.RL", Platform.Kingdee)]
		Item112,

		/// <summary>
		/// 	厨房原材料/家禽
		/// </summary>
		[EnumValue("113", Platform.FXiaoKe)]
		[EnumValue("CFY.JQ", Platform.Kingdee)]
		Item113,

		/// <summary>
		/// 	厨房原材料/河海鲜
		/// </summary>
		[EnumValue("114", Platform.FXiaoKe)]
		[EnumValue("CFY.HX", Platform.Kingdee)]
		Item114,

		/// <summary>
		/// 	厨房原材料/燕鲍翅
		/// </summary>
		[EnumValue("115", Platform.FXiaoKe)]
		[EnumValue("CFY.YB", Platform.Kingdee)]
		Item115,

		/// <summary>
		/// 	厨房原材料/豆制品
		/// </summary>
		[EnumValue("116", Platform.FXiaoKe)]
		[EnumValue("CFY.DZ", Platform.Kingdee)]
		Item116,

		/// <summary>
		/// 	厨房原材料/粮油
		/// </summary>
		[EnumValue("117", Platform.FXiaoKe)]
		[EnumValue("CFY.LY", Platform.Kingdee)]
		Item117,

		/// <summary>
		/// 	厨房原材料/冷冻品
		/// </summary>
		[EnumValue("118", Platform.FXiaoKe)]
		[EnumValue("CFY.LD", Platform.Kingdee)]
		Item118,

		/// <summary>
		/// 	厨房原材料
		/// </summary>
		[EnumValue("8", Platform.FXiaoKe)]
		[EnumValue("CFY", Platform.Kingdee)]
		Item8,

		/// <summary>
		/// 	日常物料/客耗品
		/// </summary>
		[EnumValue("119", Platform.FXiaoKe)]
		[EnumValue("RCL.KH", Platform.Kingdee)]
		Item119,

		/// <summary>
		/// 	日常物料/小商品
		/// </summary>
		[EnumValue("120", Platform.FXiaoKe)]
		[EnumValue("RCL.XS", Platform.Kingdee)]
		Item120,

		/// <summary>
		/// 	日常物料
		/// </summary>
		[EnumValue("9", Platform.FXiaoKe)]
		[EnumValue("RCL", Platform.Kingdee)]
		Item9,

		/// <summary>
		/// 	酒水饮料/酒水
		/// </summary>
		[EnumValue("121", Platform.FXiaoKe)]
		[EnumValue("JSL.JS", Platform.Kingdee)]
		Item121,

		/// <summary>
		/// 	酒水饮料/饮料
		/// </summary>
		[EnumValue("122", Platform.FXiaoKe)]
		[EnumValue("JSL.YL", Platform.Kingdee)]
		Item122,

		/// <summary>
		/// 	酒水饮料/香烟
		/// </summary>
		[EnumValue("123", Platform.FXiaoKe)]
		[EnumValue("JSL.XY", Platform.Kingdee)]
		Item123,

		/// <summary>
		/// 	酒水饮料/茶叶
		/// </summary>
		[EnumValue("124", Platform.FXiaoKe)]
		[EnumValue("JSL.CY", Platform.Kingdee)]
		Item124,

		/// <summary>
		/// 	酒水饮料
		/// </summary>
		[EnumValue("10", Platform.FXiaoKe)]
		[EnumValue("JSL", Platform.Kingdee)]
		Item10,

		/// <summary>
		/// 	餐具厨具/餐具厨具
		/// </summary>
		[EnumValue("125", Platform.FXiaoKe)]
		[EnumValue("CJL.CC", Platform.Kingdee)]
		Item125,

		/// <summary>
		/// 	餐具厨具
		/// </summary>
		[EnumValue("11", Platform.FXiaoKe)]
		[EnumValue("CJL", Platform.Kingdee)]
		Item11,

		/// <summary>
		/// 	布草服装/布草
		/// </summary>
		[EnumValue("126", Platform.FXiaoKe)]
		[EnumValue("BCF.BC", Platform.Kingdee)]
		Item126,

		/// <summary>
		/// 	布草服装/服装
		/// </summary>
		[EnumValue("127", Platform.FXiaoKe)]
		[EnumValue("BCF.FZ", Platform.Kingdee)]
		Item127,

		/// <summary>
		/// 	布草服装
		/// </summary>
		[EnumValue("12", Platform.FXiaoKe)]
		[EnumValue("BCF", Platform.Kingdee)]
		Item12
	}
}