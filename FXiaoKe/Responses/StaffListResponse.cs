﻿// ReSharper disable StringLiteralTypo
using System.Collections.Generic;
using FXiaoKe.Models;
using Newtonsoft.Json;

namespace FXiaoKe.Responses {
	public class StaffListResponse : BasicResponse {
		[JsonProperty("userlist")]
		public List<Staff> Staffs { get; set; }
	}
}