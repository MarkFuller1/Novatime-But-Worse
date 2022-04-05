using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Time.Interfaces;
using Time.Repositories;


namespace Time.Services
{
    public class ExcelService : IExcelService
    {
        private readonly ILogger<ExcelService> _logger;
        private readonly EmployeeTimesContext _hoursContext;

        public ExcelService(ILogger<ExcelService> logger, EmployeeTimesContext context)
        {
            _logger = logger;
            _hoursContext = context;
        }
        public String buildExcelFileFromDBEntries()
        {
            var result = from employees in _hoursContext.Employees.Include(e => e.hours)
                         select employees;

            if (result.Any())
            {
                var hours = result.ToList();

                StringBuilder sb = new StringBuilder();
                sb.Append("last name,first name,");

                // build list of dates
                HashSet<long> dates = new HashSet<long>();
                hours.ForEach(employee =>
                    employee.hours.Select(hour =>
                        hour.weekEndingDate).ToList()
                        .ForEach(date =>
                            dates.Add(date)));

                var sortedDates = dates.Select(date => Epoch2UTCNow(date)).OrderBy(d => d.Ticks).ToList();

                // add dates as columns names
                sortedDates.ForEach(date =>
                {
                    sb.Append(date.ToString() + ",");
                });
                sb.Append("\r\n");

                _logger.LogInformation(sortedDates.ToString());

                // iterate over each user and print their hours in the same order as the above dates
                hours.ForEach(employee =>
                {
                    sb.Append(employee.last_name + ",")
                        .Append(employee.first_name + ",");
                    employee.hours.OrderBy(e =>
                        e.weekEndingDate)
                    .ToList()
                    .ForEach(entry =>
                            sb.Append(entry.hours.ToString() + ","));
                    sb.Append("\r\n");
                });

                var filePath =
                    Directory.GetCurrentDirectory() + Path.PathSeparator + "Data.csv";

                _logger.LogInformation("File Path:" + filePath);


                File.WriteAllTextAsync(filePath, sb.ToString());

                _logger.LogInformation("File is written");

                return filePath;
            }
            return null;
        }

        private DateTime Epoch2UTCNow(long epoch)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(epoch);
        }
    }

}
