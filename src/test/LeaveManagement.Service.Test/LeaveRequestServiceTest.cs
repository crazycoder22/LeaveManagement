using LeaveManagement.Common.Helper;
using LeaveManagement.Common.Model;
using LeaveManagement.Repository.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LeaveManagement.Service.Test
{
    [TestClass]
    public class LeaveRequestServiceTest
    {
        [TestMethod]
        public void GetAllRequests()
        {
            // Arrange
            var leaveRepoMock = new Mock<ILeaveRepository>();
            var leaveBalanceRepoMock = new Mock<ILeaveBalanceRepository>();
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            var sqsClientMock = new Mock<ISqsClient>();
            var leaveRepoObj = leaveRepoMock.Object;
            var leaveBalanceRepoObj = leaveBalanceRepoMock.Object;
            var employeeId = "LAK123";
            var leaveRequests = new List<LeaveRequest> { new LeaveRequest(), new LeaveRequest() };
            leaveRepoMock.Setup(x => x.GetAllRequests(employeeId)).Returns(Task.FromResult(leaveRequests));

            var leaveRequestService = new LeaveRequestService(leaveRepoObj, leaveBalanceRepoObj, sqsClientMock.Object, employeeRepositoryMock.Object);

            // Act 
            var result = leaveRequestService.GetAllRequests(employeeId).Result;

            // Assert
            Assert.AreEqual(2, result.Count);

        }

        [TestMethod]
        public void ApproveLeaveRequest()
        {
            // Arrange
            var leaveRepoMock = new Mock<ILeaveRepository>();
            var leaveBalanceRepoMock = new Mock<ILeaveBalanceRepository>();
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            var sqsClientMock = new Mock<ISqsClient>();
            var leaveRepoObj = leaveRepoMock.Object;
            var leaveBalanceRepoObj = leaveBalanceRepoMock.Object;
            var requestId = "9531faa7-7ddb-4434-afdb-16d8b1376f88";
            var employeeId = "LAK123";
            var leaveRequest = new LeaveRequest
            {
                EmployeeId = employeeId,
                From = DateTime.Parse("2020-01-01"),
                To = DateTime.Parse("2020-01-01"),
                ManagerComment = ""
            };

            var employeDetails = new EmployeeDetails
            {
                EmployeeId = employeeId,
                ManagerEmail = "MNGR@gmail.com"
            };

            leaveRepoMock.Setup(x => x.GetRequestDetails(requestId)).Returns(Task.FromResult(leaveRequest));
            employeeRepositoryMock.Setup(x => x.GetEmployeeDetailsWithId("LAK123")).Returns(Task.FromResult(employeDetails));
            var leaveRequestService = new LeaveRequestService(leaveRepoObj, leaveBalanceRepoObj, sqsClientMock.Object, employeeRepositoryMock.Object); ;
            var eventBody = "{\"TemplateId\":\"D4B7B0C1-B85A-4EAD-8A4A-A535A107F310\",\"EmailAddress\":null,\"Subject\":\"Leave Approval\",\"Parameter\":{\"EmployeeName\":\"LAK123\",\"From\":\"1/01/2020 12:00:00 AM\",\"To\":\"1/01/2020 12:00:00 AM\",\"RequestId\":\"9531faa7-7ddb-4434-afdb-16d8b1376f88\"}}";
            // Act 
            leaveRequestService.ApproveLeaveRequest(requestId, "").Wait();

            // Assert
            leaveRepoMock.Verify(x => x.ApproveLeaveRequest(requestId, ""));
            sqsClientMock.Verify(x => x.SendMessage(eventBody));
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public void CreateLeaveRequest_IncorrectDates_ValidationException()
        {
            // Arrange
            var leaveRepoMock = new Mock<ILeaveRepository>();
            var leaveBalanceRepoMock = new Mock<ILeaveBalanceRepository>();
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            var sqsClientMock = new Mock<ISqsClient>();
            var leaveRepoObj = leaveRepoMock.Object;
            var leaveBalanceRepoObj = leaveBalanceRepoMock.Object;
            var employeeId = "LAK123";
            var leaveRequest = new LeaveRequest
            {
                From = DateTime.Parse("2020-01-01"),
                To = DateTime.Parse("2019-01-01")
            };

            var leaveRequestService = new LeaveRequestService(leaveRepoObj, leaveBalanceRepoObj, sqsClientMock.Object, employeeRepositoryMock.Object);

            // Act 
            leaveRequestService.CreateLeaveRequest(employeeId, leaveRequest).Wait();
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public void CreateLeaveRequest_NotEnoughBalance_ValidationException()
        {
            // Arrange
            var leaveRepoMock = new Mock<ILeaveRepository>();
            var leaveBalanceRepoMock = new Mock<ILeaveBalanceRepository>();
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            var sqsClientMock = new Mock<ISqsClient>();
            var leaveRepoObj = leaveRepoMock.Object;
            var leaveBalanceRepoObj = leaveBalanceRepoMock.Object;
            var employeeId = "LAK123";
            var leaveRequest = new LeaveRequest
            {
                From = DateTime.Parse("2020-01-01"),
                To = DateTime.Parse("2020-01-20"),
                LeaveType = Common.Enum.LeaveType.Casual
            };

            leaveBalanceRepoMock.Setup(x => x.GetLeaveBalance(employeeId, Common.Enum.LeaveType.Casual)).Returns(Task.FromResult(new LeaveBalance { EmployeeId = employeeId, LeaveType = Common.Enum.LeaveType.Casual, NoOfDays = 5 }));
            var leaveRequestService = new LeaveRequestService(leaveRepoObj, leaveBalanceRepoObj, sqsClientMock.Object, employeeRepositoryMock.Object);

            // Act 
            leaveRequestService.CreateLeaveRequest(employeeId, leaveRequest).Wait();
        }

        [TestMethod]
        public void CreateLeaveRequest_Success()
        {
            // Arrange
            var leaveRepoMock = new Mock<ILeaveRepository>();
            var leaveBalanceRepoMock = new Mock<ILeaveBalanceRepository>();
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            var sqsClientMock = new Mock<ISqsClient>();
            var leaveRepoObj = leaveRepoMock.Object;
            var leaveBalanceRepoObj = leaveBalanceRepoMock.Object;
            var employeeId = "LAK123";
            var leaveRequest = new LeaveRequest
            {
                From = DateTime.Parse("2020-01-01"),
                To = DateTime.Parse("2020-01-05"),
                LeaveType = Common.Enum.LeaveType.Casual,
                FromFullDay = true,
                ToFullDay = false,
                EmployeeComment = ""
            };

            var employeDetails = new EmployeeDetails
            {
                EmployeeId = employeeId,
                ManagerEmail = "MNGR@gmail.com"
            };

            var eventBody = "{\"TemplateId\":\"BDD2927B-5F9A-4C62-82C0-6547EE416B27\",\"EmailAddress\":\"MNGR@gmail.com\",\"Subject\":\"Leave Approval\",\"Parameter\":{\"EmployeeName\":\"LAK123\",\"From\":\"1/01/2020 12:00:00 AM\",\"To\":\"5/01/2020 12:00:00 AM\",\"RequestId\":\"c861beba-1ace-4674-929c-3417c8364d27\"}}";

            var requestId = "c861beba-1ace-4674-929c-3417c8364d27";
            leaveBalanceRepoMock.Setup(x => x.GetLeaveBalance(employeeId, Common.Enum.LeaveType.Casual)).Returns(Task.FromResult(new LeaveBalance { EmployeeId = employeeId, LeaveType = Common.Enum.LeaveType.Casual, NoOfDays = 5 }));
            leaveRepoMock.Setup(x => x.CreateLeaveRequest(employeeId, It.Is<LeaveRequest>(x => x.NoOfDays == 4.5 && x.EmployeeComment == ""))).Returns(Task.FromResult(requestId));
            employeeRepositoryMock.Setup(x => x.GetEmployeeDetailsWithId(employeeId)).Returns(Task.FromResult(employeDetails));
            var leaveRequestService = new LeaveRequestService(leaveRepoObj, leaveBalanceRepoObj, sqsClientMock.Object, employeeRepositoryMock.Object);

            // Act 
            leaveRequestService.CreateLeaveRequest(employeeId, leaveRequest).Wait();

            // Assert
            leaveRepoMock.Verify(x => x.CreateLeaveRequest(employeeId, It.Is<LeaveRequest>(x => x.NoOfDays == 4.5 && x.EmployeeComment == "")));
            sqsClientMock.Verify(x => x.SendMessage(eventBody));
        }
    }
}
