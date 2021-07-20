using System.Collections.Generic;
using Newtonsoft.Json;
using Shared;

namespace FXiaoKe.Models {
	[JsonObject(MemberSerialization.OptIn)]
	public class DepartmentInfo : TreeNodeBase<DepartmentInfo> {
		[JsonProperty("id")]
		public int Id { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("parentId")]
		public int ParentId { get; set; }

		[JsonProperty("order")]
		public int Order { get; set; }

		[JsonProperty("isStop")]
		public bool Suspended { get; set; }
	}

	[JsonObject(MemberSerialization.OptIn)]
	public class Department : TreeNodeBase<Department> {
		[JsonProperty("departmentId")]
		public int Id { get; set; }

		[JsonProperty("enterpriseId")]
		public int EnterpriseId { get; set; }

		[JsonProperty("parentDepartmentId")]
		public int ParentDepartmentId { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("nameSpell")]
		public string NameSpell { get; set; }

		[JsonProperty("nameOrder")]
		public string NameOrder { get; set; }

		[JsonProperty("departmentOrder")]
		public int DepartmentOrder { get; set; }

		[JsonProperty("isStop")]
		public bool Suspended { get; set; }

		[JsonProperty("stopTime")]
		public long SuspendedTime { get; set; }

		[JsonProperty("description")]
		public string Description { get; set; }

		[JsonProperty("keywords")]
		public List<string> Keywords { get; set; }

		[JsonProperty("principalId")]
		public string PrincipalId { get; set; }

		[JsonProperty("createTime")]
		public long CreateTime { get; set; }

		[JsonProperty("updateTime")]
		public long UpdateTime { get; set; }

		[JsonProperty("status")]
		public DepartmentStatus Status { get; set; }

		[JsonProperty("hideSuperWorkInfo")]
		public bool HideSuperWorkInfo { get; set; }

		public List<Staff> Staffs { get; set; }
	}

	public enum DepartmentStatus : byte {
		Normal = 1,
		Suspended = 2,
		Deleted = 3
	}
}