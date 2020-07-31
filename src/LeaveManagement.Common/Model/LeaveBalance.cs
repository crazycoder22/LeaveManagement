using LeaveManagement.Common.Enum;

namespace LeaveManagement.Common.Model
{
    public class LeaveBalance
    {
        public string EmployeeId { get; set; }
        public LeaveType LeaveType { get; set; }
        public float NoOfDays { get; set; }
    }
}
