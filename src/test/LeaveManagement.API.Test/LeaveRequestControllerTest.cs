using LeaveManagement.API.Controllers;
using LeaveManagement.Common.Exception;
using LeaveManagement.Common.Model;
using LeaveManagement.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveManagement.API.Test
{
    [TestClass]
    public class LeaveRequestControllerTest
    {
        [TestMethod]
        public void Get()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<Controllers.EmployeeController>>();
            var leaveRequestServiceMock = new Mock<ILeaveRequestService>();

            var employeeId = "LAK123";
            var leaveRequest = new LeaveRequest();
            leaveRequestServiceMock.Setup(x => x.GetAllRequests(employeeId)).Returns(Task.FromResult(new List<LeaveRequest> { leaveRequest }));
            var leaveRequestController = new LeaveRequestController(loggerMock.Object, leaveRequestServiceMock.Object);


            // Act 
            var result = leaveRequestController.Get(employeeId).Result;

            // Assert
            Assert.AreEqual(1, result.Count());
        }

        [TestMethod]
        public void Post_InValidData_BadRequest()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<Controllers.EmployeeController>>();
            var leaveRequestServiceMock = new Mock<ILeaveRequestService>();

            var employeeId = "LAK123";
            var leaveRequest = new LeaveRequest();
            leaveRequestServiceMock.Setup(x => x.CreateLeaveRequest(employeeId, It.IsAny<LeaveRequest>())).Throws(new ValidationException(""));
            var leaveRequestController = new LeaveRequestController(loggerMock.Object, leaveRequestServiceMock.Object);


            // Act 
            var result = leaveRequestController.Post(employeeId, leaveRequest).Result as BadRequestObjectResult;

            // Assert
            Assert.AreEqual(400, result.StatusCode);
        }

        [TestMethod]
        public void Post_ReturnOk()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<Controllers.EmployeeController>>();
            var leaveRequestServiceMock = new Mock<ILeaveRequestService>();

            var employeeId = "LAK123";
            var leaveRequest = new LeaveRequest();
            var leaveRequestController = new LeaveRequestController(loggerMock.Object, leaveRequestServiceMock.Object);

            // Act 
            var result = leaveRequestController.Post(employeeId, leaveRequest).Result as OkResult;

            // Assert
            Assert.AreEqual(200, result.StatusCode);
        }

        [TestMethod]
        public void Put_InValidData_BadRequest()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<Controllers.EmployeeController>>();
            var leaveRequestServiceMock = new Mock<ILeaveRequestService>();

            var requestId = Guid.NewGuid().ToString();
            var applicationRequestAction = new ApplicationRequestAction
            {
                RequestId = requestId,
                Comment = ""
            };
            leaveRequestServiceMock.Setup(x => x.ApproveLeaveRequest(requestId, "")).Throws(new ValidationException(""));
            var leaveRequestController = new LeaveRequestController(loggerMock.Object, leaveRequestServiceMock.Object);


            // Act 
            var result = leaveRequestController.Put(applicationRequestAction).Result as BadRequestObjectResult;

            // Assert
            Assert.AreEqual(400, result.StatusCode);
        }

        [TestMethod]
        public void Put_ReturnOk()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<Controllers.EmployeeController>>();
            var leaveRequestServiceMock = new Mock<ILeaveRequestService>();

            var applicationRequestAction = new ApplicationRequestAction();
            var leaveRequestController = new LeaveRequestController(loggerMock.Object, leaveRequestServiceMock.Object);

            // Act 
            var result = leaveRequestController.Put(applicationRequestAction).Result as OkResult;

            // Assert
            Assert.AreEqual(200, result.StatusCode);
        }
    }
}
