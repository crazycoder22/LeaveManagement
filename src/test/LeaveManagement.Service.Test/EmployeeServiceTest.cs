using LeaveManagement.Repository.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace LeaveManagement.Service.Test
{
    [TestClass]
    public class EmployeeServiceTest
    {
        [TestMethod]
        public void GetEmployeeDetails()
        {
            // Arrange
            var employeeRepoMock = new Mock<IEmployeeRepository>();
            var employeeRepoObj = employeeRepoMock.Object;
            var employeeId = "LAK123";

            var leaveRequestService = new EmployeeService(employeeRepoObj);

            // Act 
            var result = leaveRequestService.GetEmployeeDetails(employeeId).Result;

            // Assert
            employeeRepoMock.Verify(x => x.GetEmployeeDetails(employeeId));

        }
    }
}
