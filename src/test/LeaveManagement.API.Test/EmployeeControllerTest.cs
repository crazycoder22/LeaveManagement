using LeaveManagement.API.Controllers;
using LeaveManagement.Common.Model;
using LeaveManagement.Service.Interface;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace LeaveManagement.API.Test
{
    [TestClass]
    public class EmployeeControllerTest
    {
        [TestMethod]
        public void Get()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<Controllers.EmployeeController>>();
            var employeeServiceMock = new Mock<IEmployeeService>();

            var employeeEmail = "abc@gmail.com";
            var employeeDetails = new EmployeeDetails { EmailAddress = employeeEmail };
            employeeServiceMock.Setup(x => x.GetEmployeeDetails(employeeEmail)).Returns(Task.FromResult(employeeDetails));
            var employeeController = new EmployeeController(loggerMock.Object, employeeServiceMock.Object);


            // Act 
            var result = employeeController.Get(employeeEmail).Result;

            // Assert
            Assert.AreEqual(employeeEmail, result.EmailAddress);
        }
    }
}
