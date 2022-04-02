using Lombok.NET;
using System.ComponentModel.DataAnnotations;

namespace Time.Models
{
    [AllArgsConstructor]
    public class Hours
    {
        [Key]
        public int hours_id { get; set; }
        public long weekEndingDate { get; set; }
        public int hours { get; set; }
        public int employee_hours_id { get; set; }
        public EmployeeHours EmployeeHours { get; set; }


    }
}