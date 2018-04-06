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
		[StringLength(20, MinimumLength = 3)]
		public string PositionId { get; set; }
		
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}