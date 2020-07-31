using LeaveManagement.DataAccess;
using LeaveManagement.DataAccess.DataModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace LeaveManagement.Repository.Test
{
    [TestClass]
    public class EmployeeRepositoryTests
    {
        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public void GetEmployeeDetails_EmployeeDoesntExist_Exception()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseInMemoryDatabase("LeaveManagement6");
            var applicationDbContext = new ApplicationDbContext(optionsBuilder.Options);
            applicationDbContext.Employees.Add(new Employee
            {
                EmployeeId = "LAK123",
                EmailAddress = "abc@gmail.com",
                DateOfJoining = DateTime.Now
            });

            applicationDbContext.SaveChanges();
            var employeeRepository = new EmployeeRepository(applicationDbContext);
            var employeeId = "ABC123";

            // Act
            employeeRepository.GetEmployeeDetails(employeeId).Wait();
        }

        [TestMethod]
        public void GetEmployeeDetails_EmployeeExist_EmployeeDetailsReturned()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseInMemoryDatabase("LeaveManagement7");
            var applicationDbContext = new ApplicationDbContext(optionsBuilder.Options);
            applicationDbContext.Employees.Add(new Employee
            {
                EmployeeId = "LAK123",
                EmailAddress = "abc@gmail.com",
                DateOfJoining = DateTime.Now
            });

            applicationDbContext.SaveChanges();
            var employeeRepository = new EmployeeRepository(applicationDbContext);
            var employeeId = "abc@gmail.com";

            // Act
            var result = employeeRepository.GetEmployeeDetails(employeeId).Result;

            // Assert
            Assert.AreEqual(result.EmailAddress, employeeId);
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public void GetEmployeeDetailsWithId_EmployeeDoesntExist_Exception()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseInMemoryDatabase("LeaveManagement9");
            var applicationDbContext = new ApplicationDbContext(optionsBuilder.Options);
            applicationDbContext.Employees.Add(new Employee
            {
                EmployeeId = "LAK123",
                EmailAddress = "abc@gmail.com",
                DateOfJoining = DateTime.Now
            });

            applicationDbContext.SaveChanges();
            var employeeRepository = new EmployeeRepository(applicationDbContext);
            var employeeId = "ABC123";

            // Act
            employeeRepository.GetEmployeeDetailsWithId(employeeId).Wait();
        }

        [TestMethod]
        public void GetEmployeeDetailsWithId_EmployeeExist_EmployeeDetailsReturned()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseInMemoryDatabase("LeaveManagement10");
            var applicationDbContext = new ApplicationDbContext(optionsBuilder.Options);
            applicationDbContext.Employees.Add(new Employee
            {
                EmployeeId = "MN123",
                EmailAddress = "abcd@gmail.com",
                DateOfJoining = DateTime.Now
            });
            applicationDbContext.Employees.Add(new Employee
            {
                EmployeeId = "LAK123",
                EmailAddress = "abc@gmail.com",
                DateOfJoining = DateTime.Now,
                ManagerId = "MN123"
            });

            applicationDbContext.SaveChanges();
            var employeeRepository = new EmployeeRepository(applicationDbContext);
            var employeeId = "LAK123";

            // Act
            var result = employeeRepository.GetEmployeeDetailsWithId(employeeId).Result;

            // Assert
            Assert.AreEqual(result.EmployeeId, employeeId);
        }
    }
}
