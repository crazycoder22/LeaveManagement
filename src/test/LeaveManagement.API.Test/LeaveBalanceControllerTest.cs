using LeaveManagement.API.Controllers;
using LeaveManagement.Common.Model;
using LeaveManagement.Service.Interface;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LeaveManagement.API.Test
{
    [TestClass]
    public class LeaveBalanceControllerTest
    {
        [TestMethod]
        public void Get()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<LeaveBalanceController>>();
            var leaveBalanceServiceMock = new Mock<ILeaveBalanceService>();

            var employeeId = "LAK123";
            var leaveBalances = new List<LeaveBalance>
            {
                new LeaveBalance
                {
                    LeaveType = Common.Enum.LeaveType.Casual , NoOfDays = 10
                }
            };

            leaveBalanceServiceMock.Setup(x => x.GetAllLeaveBalances(employeeId)).Returns(Task.FromResult(leaveBalances));
            var employeeController = new LeaveBalanceController(loggerMock.Object, leaveBalanceServiceMock.Object);


            // Act 
            var result = employeeController.Get(employeeId).Result;

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Casual", result[0].LeaveTypeDescription);
        }
    }
}
