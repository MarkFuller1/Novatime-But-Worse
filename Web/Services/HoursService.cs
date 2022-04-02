using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Time.Interfaces;
using Time.Models;
using Time.Repositories;
namespace Time.Services
{
    public class HoursService : IHoursService
    {
        private readonly EmployeeTimesContext _context;

        public HoursService(EmployeeTimesContext context)
        {
            _context = context;
        }

        public EmployeeHoursDTO getEmployee(string firstName, string lastName)
        {
            var result = from employees in _context.Employees.Include(e => e.hours)
                         where employees.last_name == lastName && employees.first_name == firstName
                         select employees;

            if (result.Any())
            {
                var singelResult = result.Single();

                return new EmployeeHoursDTO(singelResult);
            }
            return null;
        }

        public EmployeeHoursDTO save(EmployeeHours employee)
        {
            var inserted = _context.Employees.Add(employee);
            _context.SaveChanges();
            return new EmployeeHoursDTO(inserted.Entity);
        }

        public EmployeeHoursDTO addTime(string firstName, string lastName, int hours, int epoch)
        {
            var employeeEntries = from e in _context.Employees.Include(e => e.hours)
                                  where e.last_name == lastName && e.first_name == firstName
                                  select e;

            var employeeEntry = employeeEntries.ToList().First();

            var newHours = new Hours();
            newHours.hours = hours;
            newHours.weekEndingDate = epoch;
            newHours.employee_hours_id = employeeEntry.employee_hours_id;
            newHours.EmployeeHours = employeeEntry;

            if (employeeEntry.hours == null)
            {
                employeeEntry.hours = new List<Hours>();
            }

            employeeEntry.hours.Add(newHours);

            _context.Update(employeeEntry);
            _context.SaveChanges();

            return new EmployeeHoursDTO(employeeEntry);
        }
    }
}
