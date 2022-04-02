using Microsoft.EntityFrameworkCore;
using Time.Models;

namespace Time.Repositories

{
    public class EmployeeTimesContext : DbContext
    {
        public EmployeeTimesContext(DbContextOptions<EmployeeTimesContext> options)
            : base(options)
        {
            this.ChangeTracker.LazyLoadingEnabled = false;
        }

        public DbSet<EmployeeHours> Employees { get; set; }
        public DbSet<Hours> Hours { get; set; }
    }
}
