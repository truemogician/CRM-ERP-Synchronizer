// ReSharper disable StringLiteralTypo
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Shared.Serialization;

namespace FXiaoKe.Models {
	public class Staff : ModelBase {
		/// <summary>
		/// 开放平台Id
		/// </summary>
		[JsonProperty("openUserId")]
		public string Id { get; set; }

		/// <summary>
		/// 账号
		/// </summary>
		[JsonProperty("account")]
		public string Account { get; set; }

		/// <summary>
		/// 姓名
		/// </summary>
		[JsonProperty("name")]
		public string Name { get; set; }

		/// <summary>
		/// 昵称
		/// </summary>
		[JsonProperty("nickName")]
		public string NickName { get; set; }

		/// <summary>
		/// 是否离职
		/// </summary>
		[JsonProperty("isStop")]
		public bool Suspended { get; set; }

		/// <summary>
		/// 邮箱
		/// </summary>
		[JsonProperty("email")]
		public string Email { get; set; }

		/// <summary>
		/// QQ
		/// </summary>
		[JsonProperty("qq")]
		// ReSharper disable once InconsistentNaming
		public string QQ { get; set; }

		/// <summary>
		/// 微信
		/// </summary>
		[JsonProperty("weixin")]
		public string WeChat { get; set; }

		/// <summary>
		/// 手机号
		/// </summary>
		[JsonProperty("mobile")]
		public string PhoneNumber { get; set; }

		/// <summary>
		/// 性别
		/// </summary>
		[JsonProperty("gender")]
		public Gender Gender { get; set; }

		/// <summary>
		/// 职位
		/// </summary>
		[JsonProperty("position")]
		public string Position { get; set; }

		/// <summary>
		/// 头像文件Id
		/// </summary>
		[JsonProperty("profileImageUrl")]
		public string AvatarId { get; set; }

		/// <summary>
		/// 所属部门及其父部门Id列表
		/// </summary>
		[JsonProperty("departmentIds")]
		public List<int> DepartmentIds { get; set; }

		/// <summary>
		/// 员工主属部门ID
		/// </summary>
		[JsonProperty("mainDepartmentId")]
		[JsonConverter(typeof(ReferenceIdConverter<Department>), nameof(Models.Department.Id))]
		public Department Department { get; set; }

		/// <summary>
		/// 员工附属部门ID列表
		/// </summary>
		[JsonProperty("attachingDepartmentIds")]
		public List<int> AttachingDepartmentIds { get; set; }

		/// <summary>
		/// 编号
		/// </summary>
		[JsonProperty("employeeNumber")]
		public string Number { get; set; }

		/// <summary>
		/// 入职日期
		/// </summary>
		[JsonProperty("hireDate")]
		public string HireDate { get; set; }

		/// <summary>
		/// 生日
		/// </summary>
		[JsonProperty("birthDate")]
		public string Birthday { get; set; }

		/// <summary>
		/// 参加工作日期
		/// </summary>
		[JsonProperty("startWorkDate")]
		public string StartDate { get; set; }

		/// <summary>
		/// 创建时间
		/// </summary>
		[JsonProperty("createTime")]
		public long CreateTime { get; set; }

		/// <summary>
		/// 汇报对象Id
		/// </summary>
		[JsonProperty("leaderId")]
		public string LeaderId { get; set; }
	}

	[JsonConverter(typeof(StringEnumConverter))]
	public enum Gender : byte {
		[EnumMember(Value = "M")]
		Male,

		[EnumMember(Value = "F")]
		Female
	}
}