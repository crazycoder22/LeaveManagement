using LeaveManagement.DataAccess.DataModel;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagement.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        //public ApplicationDbContext()
        //{
        //}
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Data Source=localhost\\SQLEXPRESS01;Initial Catalog=LeaveManagement;Integrated Security=SSPI;Application Name=LeaveApplication");
        //}

        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeLeaveBalance> EmployeeLeaveBalances { get; set; }
        public DbSet<ApplicationRequest> ApplicationRequests { get; set; }
        public DbSet<LeaveType> LeaveTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EmployeeLeaveBalance>()
                .HasKey(c => new { c.EmployeeId, c.LeaveType });
            base.OnModelCreating(modelBuilder);
        }
    }
}
