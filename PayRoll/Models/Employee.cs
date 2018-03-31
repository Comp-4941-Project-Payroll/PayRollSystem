using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PayRoll.Models
{
	public class Employee
	{
		[Key]
		[StringLength(70, MinimumLength = 10)]
		public string EmployeeId { get; set; }

		[Required]
		[StringLength(100, MinimumLength = 8)]
		public string Password { get; set; }

		[MaxLength(20)]
		public string FName { get; set; }

		[MaxLength(20)]
		public string LName { get; set; }

		[MaxLength(100)]
		public string Address { get; set; }

		[StringLength(15, MinimumLength = 10)]
		public string Phone { get; set; }

		[Required]
		[MaxLength(8)]
		public string FullOrPartTime { get; set; }

		[Required]
		public Position PositionId { get; set; }

		public int Seniority { get; set; } = 0;

		[MaxLength(20)]
		public string DepartmentType { get; set; }
	}
}