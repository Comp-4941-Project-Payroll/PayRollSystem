using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PayRoll.Models
{
	public class TimeOffRequest
	{
		public Employee senderId { get; set; }
		public string type { get; set; }
		public DateTime startDate { get; set; }
		public DateTime endDate { get; set; }
		public string reason { get; set; }
		public DateTime whenSent { get; set; }
	}
}