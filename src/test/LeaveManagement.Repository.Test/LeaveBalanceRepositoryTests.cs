using LeaveManagement.DataAccess;
using LeaveManagement.DataAccess.DataModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;


namespace LeaveManagement.Repository.Test
{
    [TestClass]
    public class LeaveBalanceRepositoryTests
    {
        [TestMethod]
        public void GetLeaveBalance()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseInMemoryDatabase("LeaveManagement");
            var applicationDbContext = new ApplicationDbContext(optionsBuilder.Options);
            applicationDbContext.Employees.Add(new Employee
            {
                EmployeeId = "LAK123",
                EmailAddress = "abc@gmail.com",
                DateOfJoining = DateTime.Now
            });

            applicationDbContext.EmployeeLeaveBalances.Add(new EmployeeLeaveBalance
            {
                EmployeeId = "LAK123",
                LeaveType = (int)Common.Enum.LeaveType.Casual,
                NoOfDays = 10
            });

            applicationDbContext.EmployeeLeaveBalances.Add(new EmployeeLeaveBalance
            {
                EmployeeId = "LAK123",
                LeaveType = (int)Common.Enum.LeaveType.Earned,
                NoOfDays = 18
            });

            applicationDbContext.SaveChanges();
            var leaveBalanceRepository = new LeaveBalanceRepository(applicationDbContext);
            var employeeId = "LAK123";

            // Act
            var result = leaveBalanceRepository.GetLeaveBalance(employeeId, Common.Enum.LeaveType.Casual).Result;

            // Assert
            Assert.AreEqual(result.NoOfDays, 10);

        }
    }
}
