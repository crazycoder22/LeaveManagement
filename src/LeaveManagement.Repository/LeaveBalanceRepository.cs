using LeaveManagement.Common.Enum;
using LeaveManagement.Common.Model;
using LeaveManagement.DataAccess;
using LeaveManagement.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveManagement.Repository
{
    public class LeaveBalanceRepository : ILeaveBalanceRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public LeaveBalanceRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<LeaveBalance> GetLeaveBalance(string employeeid, LeaveType leaveType)
        {
            var leaveBalancesFromDb = await _applicationDbContext.EmployeeLeaveBalances.FirstOrDefaultAsync(x => x.EmployeeId == employeeid && x.LeaveType == (int)leaveType);
            return new LeaveBalance
            {
                EmployeeId = leaveBalancesFromDb.EmployeeId,
                LeaveType = (LeaveType)leaveBalancesFromDb.LeaveType,
                NoOfDays = leaveBalancesFromDb.NoOfDays
            };
        }

        public async Task<List<LeaveBalance>> GetAllLeaveBalance(string employeeid)
        {
            var leaveBalanceResult = new List<LeaveBalance>();
            var leaveBalancesFromDb = await _applicationDbContext.EmployeeLeaveBalances.Where(x => x.EmployeeId == employeeid).ToListAsync();
            foreach (var leaveBalance in leaveBalancesFromDb)
            {
                leaveBalanceResult.Add(new LeaveBalance
                {
                    EmployeeId = leaveBalance.EmployeeId,
                    LeaveType = (LeaveType)leaveBalance.LeaveType,
                    NoOfDays = leaveBalance.NoOfDays
                });
            }

            return leaveBalanceResult;
        }
    }
}
