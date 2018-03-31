using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PayRoll.Models
{
	public class Branch
	{
		public string BranchId { get; set; }
		public string name { get; set; }
		public Employee managerId { get; set; }
		public string type { get; set; }
		public string address { get; set; }
	}
}