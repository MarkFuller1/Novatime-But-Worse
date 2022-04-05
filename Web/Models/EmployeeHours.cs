using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Time.Models
{
    public partial class EmployeeHours
    {
        [Key]
        public int employee_hours_id { get; set; }
        public String first_name { get; set; }
        public String last_name { get; set; }
        public List<Hours> hours { get; set; }

        public EmployeeHours()
        {
            hours = new List<Hours>();
        }

        public EmployeeHours(int employee_hours_id, String first_name, String last_name, List<Hours> hours)
        {
            this.employee_hours_id = employee_hours_id;
            this.first_name = first_name;
            this.last_name = last_name;
            this.hours = hours;
        }
    }
}
