using System.ComponentModel.DataAnnotations;

namespace Time.Models
{
    public partial class Hours
    {
        [Key]
        public int hours_id { get; set; }
        public long weekEndingDate { get; set; }
        public int hours { get; set; }
        public int employee_hours_id { get; set; }
        public EmployeeHours EmployeeHours { get; set; }


        public Hours()
        {
        }
        public Hours(int hours_id, long weekEndingDate, int hours, int employee_hours_id, EmployeeHours employee)
        {
            this.hours_id = hours_id;
            this.weekEndingDate = weekEndingDate;
            this.hours = hours;
            this.employee_hours_id = employee_hours_id;
            this.EmployeeHours = employee;

        }
    }
}