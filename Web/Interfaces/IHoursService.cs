using Time.Models;

namespace Time.Interfaces
{
    public interface IHoursService
    {
        public EmployeeHoursDTO getEmployee(string firstName, string lastName);
        EmployeeHoursDTO save(EmployeeHours employeeHours);
        EmployeeHoursDTO addTime(EmployeeHoursDTO employeeData);
    }
}
