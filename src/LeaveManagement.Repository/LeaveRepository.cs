using LeaveManagement.Common.Enum;
using LeaveManagement.Common.Model;
using LeaveManagement.DataAccess;
using LeaveManagement.DataAccess.DataModel;
using LeaveManagement.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveManagement.Repository
{
    public class LeaveRepository : ILeaveRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public LeaveRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<string> CreateLeaveRequest(string employeeId, LeaveRequest leaveRequest)
        {
            var emplaoyeeDetails = await _applicationDbContext.Employees.FirstOrDefaultAsync(x => x.EmployeeId == employeeId);
            if (emplaoyeeDetails == default(Employee))
            {
                throw new Exception("Emplyoee data is missing");
            }

            var requestId = Guid.NewGuid();
            var request = new ApplicationRequest
            {
                Id = requestId,
                RequesterId = employeeId,
                ApproverId = emplaoyeeDetails.ManagerId,
                RequestDetails = JsonConvert.SerializeObject(leaveRequest),
                RequesterComment = leaveRequest.EmployeeComment,
                RequestType = (int)RequestType.Leave,
                Status = (int)ApplicationStatus.Submitted
            };

            var leaveBalance = await _applicationDbContext.EmployeeLeaveBalances.FirstOrDefaultAsync(x => x.EmployeeId == employeeId && x.LeaveType == (int)leaveRequest.LeaveType);
            if (leaveBalance == default(EmployeeLeaveBalance))
            {
                throw new Exception("Emplyoee leave balance data is not configured");
            }

            leaveBalance.NoOfDays -= leaveRequest.NoOfDays;

            _applicationDbContext.ApplicationRequests.Add(request);
            await _applicationDbContext.SaveChangesAsync();

            return requestId.ToString();
        }

        public async Task<List<LeaveRequest>> GetAllRequests(string employeeid)
        {
            var leaveRequests = new List<LeaveRequest>();
            var leaveRequestHistory = await _applicationDbContext.ApplicationRequests.Where(x => x.RequesterId == employeeid && x.RequestType == (int)RequestType.Leave).ToListAsync();
            foreach (var leaveRequest in leaveRequestHistory)
            {
                var requestDetails = JsonConvert.DeserializeObject<LeaveRequest>(leaveRequest.RequestDetails);
                requestDetails.Status = (ApplicationStatus)leaveRequest.Status;
                requestDetails.EmployeeComment = leaveRequest.RequesterComment;
                requestDetails.ManagerComment = leaveRequest.ApproverComment;
                leaveRequests.Add(requestDetails);
            }

            return leaveRequests;
        }

        public async Task<LeaveRequest> GetRequestDetails(string requestId)
        {
            var leaveRequestFromDb = await _applicationDbContext.ApplicationRequests.FirstOrDefaultAsync(x => x.Id == Guid.Parse(requestId));
            var requestDetails = JsonConvert.DeserializeObject<LeaveRequest>(leaveRequestFromDb.RequestDetails);
            requestDetails.Status = (ApplicationStatus)leaveRequestFromDb.Status;
            requestDetails.EmployeeComment = leaveRequestFromDb.RequesterComment;
            requestDetails.ManagerComment = leaveRequestFromDb.ApproverComment;
            requestDetails.EmployeeId = leaveRequestFromDb.RequesterId;

            return requestDetails;
        }

        public async Task ApproveLeaveRequest(string requestId, string approveComment)
        {
            var request = await _applicationDbContext.ApplicationRequests.FirstOrDefaultAsync(x => x.Id == Guid.Parse(requestId));
            request.ApproverComment = approveComment;
            request.Status = (int)ApplicationStatus.Approved;

            await _applicationDbContext.SaveChangesAsync();
        }
    }
}
