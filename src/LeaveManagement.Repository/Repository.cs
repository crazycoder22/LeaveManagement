using LeaveManagement.Repository.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace LeaveManagement.Repository
{
    public class Repository
    {
        public static void Register(IServiceCollection services)
        {
            services.AddTransient<IEmployeeRepository, EmployeeRepository>();
            services.AddTransient<ILeaveRepository, LeaveRepository>();
            services.AddTransient<ILeaveBalanceRepository, LeaveBalanceRepository>();
        }
    }
}
