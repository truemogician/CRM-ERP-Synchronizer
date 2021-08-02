using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FXiaoKe.Models;
using Newtonsoft.Json;

namespace FXiaoKe.Responses {
	public class StaffQueryResponse : BasicResponse {
		/// <summary>
		///     员工信息列表
		/// </summary>
		[JsonProperty("empList")]
		[Required]
		public List<Staff> Staffs { get; set; }
	}
}