using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PayRoll.Models
{
	public class Attendance
	{
		public string AttendanceId { get; set; }
		public Employee EmployeeId { get; set; }
		public DateTime signInTime { get; set; }
		public DateTime signOutTime { get; set; }
	}
}