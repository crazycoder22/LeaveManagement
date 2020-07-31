using LeaveManagement.Common.Model;
using System.Threading.Tasks;

namespace LeaveManagement.Repository.Interface
{
    public interface IEmployeeRepository
    {
        Task<EmployeeDetails> GetEmployeeDetails(string employeeid);
        Task<EmployeeDetails> GetEmployeeDetailsWithId(string employeeid);
    }
}
