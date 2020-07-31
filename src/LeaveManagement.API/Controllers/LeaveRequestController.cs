using LeaveManagement.Common.Exception;
using LeaveManagement.Common.Model;
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
    public class LeaveRequestController : ControllerBase
    {
        private readonly ILogger<EmployeeController> _logger;
        private readonly ILeaveRequestService _leaveRequestService;

        public LeaveRequestController(ILogger<EmployeeController> logger, ILeaveRequestService leaveRequestService)
        {
            _logger = logger;
            _leaveRequestService = leaveRequestService;
        }

        [HttpGet]
        public async Task<IEnumerable<LeaveRequest>> Get(string employeeid)
        {
            return await _leaveRequestService.GetAllRequests(employeeid);
        }

        [HttpPost]
        public async Task<IActionResult> Post(string employeeId, [FromBody] LeaveRequest leaveRequest)
        {
            try
            {
                await _leaveRequestService.CreateLeaveRequest(employeeId, leaveRequest);
                return Ok();
            }
            catch (ValidationException validationException)
            {
                return BadRequest(validationException.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put(ApplicationRequestAction request)
        {
            try
            {
                await _leaveRequestService.ApproveLeaveRequest(request.RequestId, request.Comment);
                return Ok();
            }
            catch (ValidationException validationException)
            {
                return BadRequest(validationException.Message);
            }
        }

    }
}
