using LeaveManagement.Common.Model;
using LeaveManagement.Repository.Interface;
using LeaveManagement.Service.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LeaveManagement.Service
{
    public class LeaveBalanceService : ILeaveBalanceService
    {
        private readonly ILeaveBalanceRepository _leaveBalanceRepository;

        public LeaveBalanceService(ILeaveBalanceRepository leaveBalanceRepository)
        {
            _leaveBalanceRepository = leaveBalanceRepository;
        }
        public Task<List<LeaveBalance>> GetAllLeaveBalances(string employeId)
        {
            return _leaveBalanceRepository.GetAllLeaveBalance(employeId);
        }
    }
}
