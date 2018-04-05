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
		[MaxLength(15)]
		public string EmployeeId { get; set; }

		[Required]
		[StringLength(100, MinimumLength = 8)]
		public string Password { get; set; }

		[MaxLength(20)]
		[Display(Name = "First Name")]
		public string FName { get; set; }

		[MaxLength(20)]
		[Display(Name = "Last Name")]
		public string LName { get; set; }

		[MaxLength(100)]
		public string Address { get; set; }

		[StringLength(15, MinimumLength = 10)]
		public string Phone { get; set; }

		[MaxLength(10)]
		[Display(Name = "Full or Part Time")]
		[RegularExpression("Full-Time|Part-Time")]
		public string FullOrPartTime { get; set; }

		public int Seniority { get; set; } = 0;

		[MaxLength(20)]
		public string DepartmentType { get; set; }
		public decimal HourlyRate { get; set; }

		public ICollection<TimeOffRequest> TimeOffRequests { get; set; } = new List<TimeOffRequest>();
        public ICollection<Payroll> Payrolls { get; set; } = new List<Payroll>();
    }
}