using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PayRoll.Models
{
	public class Employee
	{
		public string EmployeeID { get; set; }
		public string password { get; set; }
		public string fName { get; set; }
		public string lName { get; set; }
		public string address { get; set; }
		public string phone { get; set; }
		public string fullOrPartTime { get; set; }
		public Position PositionId { get; set; }
		public int seniority { get; set; }
		public string departmentType { get; set; }
	}
}