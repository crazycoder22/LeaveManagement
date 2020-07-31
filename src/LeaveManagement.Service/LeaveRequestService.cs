using LeaveManagement.Common.Exception;
using LeaveManagement.Common.Helper;
using LeaveManagement.Common.Model;
using LeaveManagement.Repository.Interface;
using LeaveManagement.Service.Interface;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LeaveManagement.Service
{
    public class LeaveRequestService : ILeaveRequestService
    {
        private readonly ILeaveRepository _leaveRepository;
        private readonly ILeaveBalanceRepository _leaveBalanceRepository;
        private readonly IEmployeeRepository _employeeRepository;

        private readonly ISqsClient _sqsClient;


        // To be moved to configuration file
        private const string EMAIL_TEMPLATE_ID = "BDD2927B-5F9A-4C62-82C0-6547EE416B27";
        private const string EMAIL_APPROVED_TEMPLATE_ID = "D4B7B0C1-B85A-4EAD-8A4A-A535A107F310";

        public LeaveRequestService(ILeaveRepository leaveRepository, ILeaveBalanceRepository leaveBalanceRepository, ISqsClient sqsClient, IEmployeeRepository employeeRepository)
        {
            _leaveRepository = leaveRepository;
            _leaveBalanceRepository = leaveBalanceRepository;
            _sqsClient = sqsClient;
            _employeeRepository = employeeRepository;
        }

        public async Task<List<LeaveRequest>> GetAllRequests(string employeeid)
        {
            return await _leaveRepository.GetAllRequests(employeeid);
        }

        public async Task ApproveLeaveRequest(string requestId, string approveComment)
        {
            await _leaveRepository.ApproveLeaveRequest(requestId, approveComment);
            var requestDetails = await _leaveRepository.GetRequestDetails(requestId);
            var employeeDetails = await _employeeRepository.GetEmployeeDetailsWithId(requestDetails.EmployeeId);
            var eventMessage = new EventModel
            {
                TemplateId = EMAIL_APPROVED_TEMPLATE_ID,
                EmailAddress = employeeDetails.EmailAddress,
                Subject = "Leave Approval",
                Parameter = new Parameter
                {
                    EmployeeName = employeeDetails.EmployeeId,
                    From = requestDetails.From.ToString(),
                    To = requestDetails.To.ToString(),
                    RequestId = requestId
                }
            };

            var message = JsonConvert.SerializeObject(eventMessage);
            await _sqsClient.SendMessage(message);
        }

        public async Task CreateLeaveRequest(string employeeId, LeaveRequest leaveRequest)
        {
            if (leaveRequest.From > leaveRequest.To)
            {
                throw new ValidationException("From Date should be greater than To Date"); ;
            }

            var noOfDays = (leaveRequest.To - leaveRequest.From).TotalDays + 1;
            if (!leaveRequest.FromFullDay)
            {
                noOfDays -= 0.5;
            }

            if (!leaveRequest.ToFullDay)
            {
                noOfDays -= 0.5;
            }

            var leaveBalance = await _leaveBalanceRepository.GetLeaveBalance(employeeId, leaveRequest.LeaveType);
            var noOfDaysEligible = leaveBalance.NoOfDays;

            if (noOfDaysEligible < noOfDays)
            {
                throw new ValidationException("Number of leaves not enough");
            }

            leaveRequest.NoOfDays = (float)noOfDays;
            var requestId = await _leaveRepository.CreateLeaveRequest(employeeId, leaveRequest);

            var employeeDetails = await _employeeRepository.GetEmployeeDetailsWithId(employeeId);
            var eventMessage = new EventModel
            {
                TemplateId = EMAIL_TEMPLATE_ID,
                EmailAddress = employeeDetails.ManagerEmail,
                Subject = "Leave Approval",
                Parameter = new Parameter
                {
                    EmployeeName = employeeDetails.EmployeeId,
                    From = leaveRequest.From.ToString(),
                    To = leaveRequest.To.ToString(),
                    RequestId = requestId
                }
            };

            var message = JsonConvert.SerializeObject(eventMessage);
            await _sqsClient.SendMessage(message);
        }
    }
}
