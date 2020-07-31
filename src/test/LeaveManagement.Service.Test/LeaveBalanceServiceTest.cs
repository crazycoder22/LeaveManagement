using LeaveManagement.Repository.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;


namespace LeaveManagement.Service.Test
{
    [TestClass]
    public class LeaveBalanceServiceTest
    {
        [TestMethod]
        public void GetEmployeeDetails()
        {
            // Arrange
            var leaveBalanceRepositoryMock = new Mock<ILeaveBalanceRepository>();
            var leaveBalanceRepositoryObj = leaveBalanceRepositoryMock.Object;
            var employeeId = "LAK123";

            var leaveBalanceService = new LeaveBalanceService(leaveBalanceRepositoryObj);

            // Act 
            var result = leaveBalanceService.GetAllLeaveBalances(employeeId).Result;

            // Assert
            leaveBalanceRepositoryMock.Verify(x => x.GetAllLeaveBalance(employeeId));

        }
    }
}
