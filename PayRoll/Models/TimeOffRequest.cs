﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PayRoll.Models
{
	public class TimeOffRequest
	{
		[Required]
		public Employee SenderId { get; set; }

		[Required]
		public string Type { get; set; }

		[Required]
		public DateTime StartDate { get; set; }

		[Required]
		public DateTime EndDate { get; set; }

		[MaxLength(512)]
		public string Reason { get; set; }

		public DateTime WhenSent { get; set; } = DateTime.Now;
	}
}