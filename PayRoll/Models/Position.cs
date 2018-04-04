using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PayRoll.Models
{
	public class Position
	{
		[Key]
		[MaxLength(100)]
		public string PositionId { get; set; }

		[Required]
		[StringLength(20, MinimumLength = 3)]
		public string PositionName { get; set; }

		public decimal BaseSalary { get; set; } = 0;
        public ICollection<Employee> Employees { get; set; }
    }
}