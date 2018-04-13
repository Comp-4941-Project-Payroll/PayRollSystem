using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PayRoll.Models
{
	public class Schedule
	{
		[Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ShiftId { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }
		public ICollection<Employee> Employees { get; set; } = new List<Employee>();
	}
}