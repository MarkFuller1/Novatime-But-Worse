using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Time.Models
{
    public class EmployeeHours
    {
        [Key]
        public int employee_hours_id { get; set; }
        public String first_name { get; set; }
        public String last_name { get; set; }
        public List<Hours> hours { get; set; }

    }
}
