using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PayRoll.Models
{
    public class TypeOfTimeOff
    {
        [Key]
        [MaxLength(32)]
        public string Type { get; set; }

        public ICollection<TimeOffRequest> TimeOffRequests { get; set; } = new List<TimeOffRequest>();
    }
}