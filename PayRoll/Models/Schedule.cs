using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PayRoll.Models
{
	public class Schedule
	{
		public string ShiftId { get; set; }
		public DateTime startTime { get; set; }
		public DateTime endTime { get; set; }
	}
}