using LeaveManagement.Common.Enum;
using LeaveManagement.Common.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LeaveManagement.Repository.Interface
{
    public interface ILeaveBalanceRepository
    {
        Task<LeaveBalance> GetLeaveBalance(string employeeid, LeaveType leaveType);
        Task<List<LeaveBalance>> GetAllLeaveBalance(string employeeid);
    }
}
