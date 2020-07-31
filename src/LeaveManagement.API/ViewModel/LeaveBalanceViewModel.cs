using LeaveManagement.Common.Enum;

namespace LeaveManagement.API.ViewModel
{
    public class LeaveBalanceViewModel
    {
        public string EmployeeId { get; set; }
        public LeaveType LeaveType { get; set; }
        public string LeaveTypeDescription { get; set; }
        public float NoOfDays { get; set; }
    }
}
