using LeaveManagement.Common.Model;
using LeaveManagement.Repository.Interface;
using LeaveManagement.Service.Interface;
using System.Threading.Tasks;

namespace LeaveManagement.Service
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }
        public Task<EmployeeDetails> GetEmployeeDetails(string employeeEmailAddress)
        {
            return _employeeRepository.GetEmployeeDetails(employeeEmailAddress);
        }
    }
}
