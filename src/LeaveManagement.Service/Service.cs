using LeaveManagement.Service.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace LeaveManagement.Service
{
    public class Service
    {
        public static void Register(IServiceCollection services)
        {
            services.AddTransient<IEmployeeService, EmployeeService>();
            services.AddTransient<ILeaveRequestService, LeaveRequestService>();
            services.AddTransient<ILeaveBalanceService, LeaveBalanceService>();
        }
    }
}
