using System;

namespace LeaveManagement.Common.Model
{
    public class EmployeeDetails
    {
        public string EmployeeId { get; set; }
        public string EmailAddress { get; set; }
        public DateTime DateOfJoining { get; set; }
        public string ManagerEmail { get; set; }
    }
}
