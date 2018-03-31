using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PayRoll.Models
{
	public class Branch
	{
		[Key]
		public string BranchId { get; set; }

		[MaxLength(20)]
		public string name { get; set; }

		public Employee managerId { get; set; }

		[MaxLength(20)]
		public string type { get; set; }

		[MaxLength(100)]
		public string address { get; set; }
	}
}