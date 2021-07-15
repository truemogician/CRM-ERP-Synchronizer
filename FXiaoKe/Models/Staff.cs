// ReSharper disable StringLiteralTypo
using System.Collections.Generic;
using Newtonsoft.Json;

namespace FXiaoKe.Models {
	public class Staff : ModelBase {
		[JsonProperty("openUserId")]
		[PrimaryKey]
		public string Id { get; set; }

		[JsonProperty("enterpriseId")]
		public int EnterpriseId { get; set; }

		[JsonProperty("account")]
		public string Account { get; set; }

		[JsonProperty("fullName")]
		public string FullName { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("status")]
		public string Status { get; set; }

		[JsonProperty("mobile")]
		public string Mobile { get; set; }

		[JsonProperty("leaderId")]
		public string LeaderId { get; set; }

		[JsonProperty("telephone")]
		public string Telephone { get; set; }

		[JsonProperty("role")]
		public string Role { get; set; }

		[JsonProperty("post")]
		public string Post { get; set; }

		[JsonProperty("qq")]
		// ReSharper disable once InconsistentNaming
		public string QQ { get; set; }

		[JsonProperty("email")]
		public string Email { get; set; }

		[JsonProperty("gender")]
		public string Gender { get; set; }

		[JsonProperty("profileImage")]
		public string ProfileImage { get; set; }

		[JsonProperty("description")]
		public string Description { get; set; }

		[JsonProperty("weixin")]
		public string WeChat { get; set; }

		[JsonProperty("msn")]
		public string Msn { get; set; }

		[JsonProperty("extensionNumber")]
		public string ExtensionNumber { get; set; }

		[JsonProperty("mobileSetting")]
		public MobileSetting MobileSetting { get; set; }

		[JsonProperty("workingState")]
		public string WorkingState { get; set; }

		[JsonProperty("isActive")]
		public bool IsActive { get; set; }

		[JsonProperty("mainDepartmentIds")]
		public List<int> MainDepartmentIds { get; set; }

		[JsonProperty("departmentIds")]
		public List<int> DepartmentIds { get; set; }

		[JsonProperty("departmentAsteriskIds")]
		public List<object> DepartmentAsteriskIds { get; set; }

		[JsonProperty("employeeAsteriskIds")]
		public List<object> EmployeeAsteriskIds { get; set; }

		[JsonProperty("birthDate")]
		public string BirthDate { get; set; }

		[JsonProperty("hireDate")]
		public string HireDate { get; set; }

		[JsonProperty("empNum")]
		public string EmpNum { get; set; }

		[JsonProperty("startWorkDate")]
		public string StartWorkDate { get; set; }

		[JsonProperty("stopTime")]
		public int StopTime { get; set; }

		[JsonProperty("createTime")]
		public long CreateTime { get; set; }

		[JsonProperty("updateTime")]
		public long UpdateTime { get; set; }

		[JsonProperty("nameSpell")]
		public string NameSpell { get; set; }

		[JsonProperty("nameOrder")]
		public string NameOrder { get; set; }
	}

	public class MobileSetting {
		[JsonProperty("mobileStatus")]
		public string MobileStatus { get; set; }

		[JsonProperty("departmentIds")]
		public List<int> DepartmentIds { get; set; }

		[JsonProperty("employeeIds")]
		public List<int> EmployeeIds { get; set; }
	}
}