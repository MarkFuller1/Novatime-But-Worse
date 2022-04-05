using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Time.Interfaces;
using Time.Models;
using Time.Repositories;
using Time.Utilities;

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

        public EmployeeHoursDTO addTime(EmployeeHoursDTO employeeData)
        {
            var employeeEntries = (from e in _context.Employees.Include(e => e.hours)
                                   where e.last_name == employeeData.lastName && e.first_name == employeeData.firstName
                                   select e).ToList();

            if (employeeEntries.Count() == 0)
            {
                return save(Converter.EmployeeHours(employeeData));
            }

            var employeeEntry = employeeEntries.First();

            employeeEntry.hours = mergeHours(employeeData.hours, employeeEntry.hours, employeeEntry);

            _context.SaveChanges();

            return new EmployeeHoursDTO(employeeEntry);

        }

        private List<Hours> mergeHours(List<HoursDTO> newHours, List<Hours> oldHours, EmployeeHours link)
        {
            Dictionary<long, Hours> hours = new Dictionary<long, Hours>();

            oldHours.ForEach(oHours => hours[oHours.weekEndingDate] = oHours);

            newHours.ForEach(nHours => hours[nHours.weekEndingDate] = Converter.Hours(nHours, link));

            return hours.Values.ToList();
        }
    }
}
