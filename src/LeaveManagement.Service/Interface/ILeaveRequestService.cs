using LeaveManagement.Common.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LeaveManagement.Service.Interface
{
    public interface ILeaveRequestService
    {
        Task<List<LeaveRequest>> GetAllRequests(string employeeid);
        Task CreateLeaveRequest(string employeeId, LeaveRequest leaveRequest);
        Task ApproveLeaveRequest(string requestId, string approveComment);
    }
}
