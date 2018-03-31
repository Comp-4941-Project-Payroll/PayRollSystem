using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PayRoll.Models
{
	public class Payroll
	{
		public Employee EmployeeId { get; set; }
		public int regularHours { get; set; }
		public int overTimeHours { get; set; }
		public decimal hourlyRate { get; set; }
		public decimal earnings { get; set; }
		public int beginningVacation { get; set; }
		public int vacationHrsTaken { get; set; }
		public decimal CPP { get; set; }
		public decimal EI { get; set; }
		public decimal incomeTaxes { get; set; }
	}
}