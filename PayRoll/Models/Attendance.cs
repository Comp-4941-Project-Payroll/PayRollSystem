using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PayRoll.Models
{
	public class Attendance
	{
		[Key]
		public int AttendanceId { get; set; }

		//public Employee EmployeeId { get; set; }
		public DateTime SignInTime { get; set; }
		public DateTime SignOutTime { get; set; }
		public Employee Employee { get; set; }
    }
}