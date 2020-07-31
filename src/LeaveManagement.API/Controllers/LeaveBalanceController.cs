using LeaveManagement.API.ViewModel;
using LeaveManagement.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LeaveManagement.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class LeaveBalanceController : ControllerBase
    {
        private readonly ILogger<LeaveBalanceController> _logger;
        private readonly ILeaveBalanceService _leaveBalanceService;

        public LeaveBalanceController(ILogger<LeaveBalanceController> logger, ILeaveBalanceService leaveBalanceService)
        {
            _logger = logger;
            _leaveBalanceService = leaveBalanceService;
        }

        [HttpGet]
        public async Task<List<LeaveBalanceViewModel>> Get(string employeeId)
        {
            var result = new List<LeaveBalanceViewModel>();
            var leaveBalances = await _leaveBalanceService.GetAllLeaveBalances(employeeId);
            foreach (var leaveBalance in leaveBalances)
            {
                result.Add(new LeaveBalanceViewModel
                {
                    EmployeeId = leaveBalance.EmployeeId,
                    LeaveType = leaveBalance.LeaveType,
                    LeaveTypeDescription = leaveBalance.LeaveType.ToString(),
                    NoOfDays = leaveBalance.NoOfDays
                });
            }
            return result;
        }
    }
}
