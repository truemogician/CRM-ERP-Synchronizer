﻿// ReSharper disable StringLiteralTypo
using System.Collections.Generic;
using FXiaoKe.Models;
using Newtonsoft.Json;

namespace FXiaoKe.Response {
	public class StaffListResponse : BasicResponseWithDescription {
		[JsonProperty("userlist")]
		public List<Staff> Staffs { get; set; }
	}
}