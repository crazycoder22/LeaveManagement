using LeaveManagement.Common.Model;
using LeaveManagement.DataAccess;
using LeaveManagement.DataAccess.DataModel;
using LeaveManagement.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace LeaveManagement.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public EmployeeRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<EmployeeDetails> GetEmployeeDetails(string employeeEmailAddress)
        {
            var employeeDetails = await _applicationDbContext.Employees.FirstOrDefaultAsync(x => x.EmailAddress == employeeEmailAddress);
            if (employeeDetails == default(Employee))
            {
                throw new Exception("Emplyoee data is missing");
            }

            return new EmployeeDetails
            {
                EmployeeId = employeeDetails.EmployeeId,
                EmailAddress = employeeDetails.EmailAddress,
                DateOfJoining = employeeDetails.DateOfJoining
            };
        }

        public async Task<EmployeeDetails> GetEmployeeDetailsWithId(string employeeId)
        {
            var employeeDetails = await _applicationDbContext.Employees.Include(x => x.Manager).FirstOrDefaultAsync(x => x.EmployeeId == employeeId);
            if (employeeDetails == default(Employee))
            {
                throw new Exception("Emplyoee data is missing");
            }

            return new EmployeeDetails
            {
                EmployeeId = employeeDetails.EmployeeId,
                EmailAddress = employeeDetails.EmailAddress,
                DateOfJoining = employeeDetails.DateOfJoining,
                ManagerEmail = employeeDetails.Manager.EmailAddress
            };
        }
    }
}
