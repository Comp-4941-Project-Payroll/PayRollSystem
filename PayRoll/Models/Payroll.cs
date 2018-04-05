using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PayRoll.Models
{
	public class Payroll
	{
		[Key]
		public string PayrollId { get; set; }
		public int year { get; set; }
		public int RegularHours { get; set; }
		public int OverTimeHours { get; set; }
		public decimal Earnings { get; set; }
		public int BeginningVacation { get; set; }
		public int VacationHrsTaken { get; set; }
		public decimal CPP { get; set; }
		public decimal EI { get; set; }
		public decimal IncomeTaxes { get; set; }
	}
}