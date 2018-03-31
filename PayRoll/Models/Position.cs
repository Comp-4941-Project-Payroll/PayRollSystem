using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PayRoll.Models
{
	public class Position
	{
		public string PositionId { get; set; }
		public string PositionName { get; set; }
		public decimal BaseSalary { get; set; }
	}
}