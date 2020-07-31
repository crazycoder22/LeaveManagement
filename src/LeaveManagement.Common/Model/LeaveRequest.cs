using LeaveManagement.Common.Enum;
using System;

namespace LeaveManagement.Common.Model
{
    public class LeaveRequest
    {
        public string EmployeeId { get; set; }
        public LeaveType LeaveType { get; set; }
        public DateTime From { get; set; }
        public bool FromFullDay { get; set; }
        public DateTime To { get; set; }
        public bool ToFullDay { get; set; }
        public float NoOfDays { get; set; }
        public string EmployeeComment { get; set; }
        public string ManagerComment { get; set; }
        public ApplicationStatus Status { get; set; }
    }
}
