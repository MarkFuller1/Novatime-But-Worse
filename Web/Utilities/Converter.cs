using System.Collections.Generic;
using Time.Models;

namespace Time.Utilities
{
    public static class Converter
    {
        public static Hours Hours(HoursDTO hour, EmployeeHours link)
        {
            Hours h = new Hours();
            h.weekEndingDate = hour.weekEndingDate;
            h.hours = hour.hours;
            h.EmployeeHours = link;
            h.employee_hours_id = link.employee_hours_id;

            return h;
        }

        public static EmployeeHours EmployeeHours(EmployeeHoursDTO employeeData)
        {
            EmployeeHours eh = new EmployeeHours();
            eh.first_name = employeeData.firstName;
            eh.last_name = employeeData.lastName;
            eh.hours = new List<Hours>();

            employeeData.hours.ForEach(hour =>
            {
                eh.hours.Add(Hours(hour, eh));
            });

            return eh;
        }
    }
}
