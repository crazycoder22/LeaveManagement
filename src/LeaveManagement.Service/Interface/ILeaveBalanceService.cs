using LeaveManagement.Common.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LeaveManagement.Service.Interface
{
    public interface ILeaveBalanceService
    {
        Task<List<LeaveBalance>> GetAllLeaveBalances(string employeeId);
    }
}
