using LeaveManagement.Common.Model;
using System.Threading.Tasks;

namespace LeaveManagement.Service.Interface
{
    public interface IEmployeeService
    {
        Task<EmployeeDetails> GetEmployeeDetails(string employeeEmailAddress);
    }
}
