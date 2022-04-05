using System.Collections.Generic;

namespace Time.Models
{
    public class EmployeeHoursDTO
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public List<HoursDTO> hours { get; set; }
        public EmployeeHoursDTO(EmployeeHours src)
        {
            firstName = src.first_name;
            lastName = src.last_name;

            hours = new List<HoursDTO>();

            if (src.hours != null)
            {
                src.hours.ForEach(hour => hours.Add(new HoursDTO(hour.hours, hour.weekEndingDate)));
            }
        }

        public EmployeeHoursDTO() { }

        public EmployeeHoursDTO(string first_name, string last_name)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.hours = new List<HoursDTO>();
        }
    }

    public class HoursDTO
    {
        public HoursDTO(int hours, long weekEndingDate)
        {
            this.hours = hours;
            this.weekEndingDate = weekEndingDate;
        }
        public HoursDTO() { }


        public int hours { get; set; }
        public long weekEndingDate { get; set; }
    }

}

