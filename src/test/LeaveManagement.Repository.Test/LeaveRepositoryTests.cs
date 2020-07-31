using LeaveManagement.Common.Enum;
using LeaveManagement.Common.Model;
using LeaveManagement.DataAccess;
using LeaveManagement.DataAccess.DataModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace LeaveManagement.Repository.Test
{
    [TestClass]
    public class LeaveRepositoryTests
    {
        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public void CreateLeaveRequest_EmployeeDoesntExist_Exception()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseInMemoryDatabase("LeaveManagement1");
            var applicationDbContext = new ApplicationDbContext(optionsBuilder.Options);
            applicationDbContext.Employees.Add(new Employee
            {
                EmployeeId = "LAK123",
                EmailAddress = "abc@gmail.com",
                DateOfJoining = DateTime.Now
            });

            applicationDbContext.SaveChanges();
            var leaveRequestRepository = new LeaveRepository(applicationDbContext);
            var employeeId = "ABC123";
            var leaveRequest = new LeaveRequest();

            // Act
            leaveRequestRepository.CreateLeaveRequest(employeeId, leaveRequest).Wait();
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public void CreateLeaveRequest_LeaveBalanceNotCofigured_Exception()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseInMemoryDatabase("LeaveManagement2");
            var applicationDbContext = new ApplicationDbContext(optionsBuilder.Options);
            applicationDbContext.Employees.Add(new Employee
            {
                EmployeeId = "LAK123",
                EmailAddress = "abc@gmail.com",
                DateOfJoining = DateTime.Now
            });

            applicationDbContext.SaveChanges();
            var leaveRequestRepository = new LeaveRepository(applicationDbContext);
            var employeeId = "LAK123";
            var leaveRequest = new LeaveRequest();

            // Act
            leaveRequestRepository.CreateLeaveRequest(employeeId, leaveRequest).Wait();
        }

        [TestMethod]
        public void CreateLeaveRequest_ValidData_RequestSubmittedSuccessfully()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseInMemoryDatabase("LeaveManagement3");
            var applicationDbContext = new ApplicationDbContext(optionsBuilder.Options);

            var employeeId = "LAK123";
            applicationDbContext.Employees.Add(new Employee
            {
                EmployeeId = employeeId,
                EmailAddress = "abc@gmail.com",
                DateOfJoining = DateTime.Now
            });
            applicationDbContext.EmployeeLeaveBalances.Add(
                new EmployeeLeaveBalance
                {
                    EmployeeId = employeeId,
                    LeaveType = (int)Common.Enum.LeaveType.Casual,
                    NoOfDays = 10
                }
            );

            applicationDbContext.SaveChanges();
            var leaveRequestRepository = new LeaveRepository(applicationDbContext);
            var leaveRequest = new LeaveRequest
            {
                From = DateTime.Parse("2020-01-01"),
                FromFullDay = true,
                To = DateTime.Parse("2020-01-05"),
                ToFullDay = true,
                LeaveType = Common.Enum.LeaveType.Casual,
                EmployeeComment = "Leave",
                NoOfDays = 6
            };

            // Act
            leaveRequestRepository.CreateLeaveRequest(employeeId, leaveRequest).Wait();
            var leaveRequestFromDb = applicationDbContext.ApplicationRequests.Where(x => x.RequesterId == employeeId && x.Status == (int)ApplicationStatus.Submitted).ToList();
            Assert.AreEqual(leaveRequestFromDb.Count(), 1);
            var leaveBalance = applicationDbContext.EmployeeLeaveBalances.Where(x => x.EmployeeId == employeeId && x.LeaveType == (int)Common.Enum.LeaveType.Casual).FirstOrDefault();
            Assert.AreEqual(leaveBalance.NoOfDays, 4);
        }

        [TestMethod]
        public void GetAllRequests_ReturnsAllRequests()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseInMemoryDatabase("LeaveManagement4");
            var applicationDbContext = new ApplicationDbContext(optionsBuilder.Options);

            var employeeId = "LAK123";
            applicationDbContext.Employees.Add(new Employee
            {
                EmployeeId = employeeId,
                EmailAddress = "abc@gmail.com",
                DateOfJoining = DateTime.Now
            });
            applicationDbContext.EmployeeLeaveBalances.Add(
                new EmployeeLeaveBalance
                {
                    EmployeeId = employeeId,
                    LeaveType = (int)Common.Enum.LeaveType.Casual,
                    NoOfDays = 10
                }
            );
            applicationDbContext.ApplicationRequests.Add(
                new ApplicationRequest
                {
                    RequesterId = employeeId,
                    RequesterComment = "",
                    RequestDetails = JsonConvert.SerializeObject(new LeaveRequest()),
                    RequestType = (int)RequestType.Leave,
                    Status = (int)ApplicationStatus.Submitted
                }
            );

            applicationDbContext.SaveChanges();
            var leaveRequestRepository = new LeaveRepository(applicationDbContext);

            // Act
            var result = leaveRequestRepository.GetAllRequests(employeeId).Result;
            Assert.AreEqual(result.Count(), 1);
        }

        [TestMethod]
        public void ApproveLeaveRequest()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseInMemoryDatabase("LeaveManagement5");
            var applicationDbContext = new ApplicationDbContext(optionsBuilder.Options);

            var employeeId = "LAK123";
            applicationDbContext.Employees.Add(new Employee
            {
                EmployeeId = employeeId,
                EmailAddress = "abc@gmail.com",
                DateOfJoining = DateTime.Now
            });
            applicationDbContext.EmployeeLeaveBalances.Add(
                new EmployeeLeaveBalance
                {
                    EmployeeId = employeeId,
                    LeaveType = (int)Common.Enum.LeaveType.Casual,
                    NoOfDays = 10
                }
            );

            var requestId = Guid.NewGuid();
            applicationDbContext.ApplicationRequests.Add(
                new ApplicationRequest
                {
                    Id = requestId,
                    RequesterId = employeeId,
                    RequesterComment = "",
                    RequestDetails = "",
                    RequestType = (int)RequestType.Leave
                }
            );

            applicationDbContext.SaveChanges();
            var leaveRequestRepository = new LeaveRepository(applicationDbContext);

            // Act
            leaveRequestRepository.ApproveLeaveRequest(requestId.ToString(), "Approver Comment").Wait();
            var requestObj = applicationDbContext.ApplicationRequests.Where(x => x.RequesterId == employeeId).FirstOrDefault();
            Assert.AreEqual(requestObj.Status, (int)ApplicationStatus.Approved);
        }
    }
}
