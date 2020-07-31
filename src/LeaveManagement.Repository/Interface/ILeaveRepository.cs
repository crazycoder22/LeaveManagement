using LeaveManagement.Common.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LeaveManagement.Repository.Interface
{
    public interface ILeaveRepository
    {
        Task<List<LeaveRequest>> GetAllRequests(string employeeid);
        Task<string> CreateLeaveRequest(string employeeId, LeaveRequest leaveRequest);
        Task ApproveLeaveRequest(string requestId, string approveComment);
        Task<LeaveRequest> GetRequestDetails(string requestId);
    }
}
