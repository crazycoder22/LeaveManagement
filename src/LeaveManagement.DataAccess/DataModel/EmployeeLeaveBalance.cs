using System.ComponentModel.DataAnnotations.Schema;

namespace LeaveManagement.DataAccess.DataModel
{
    [Table("EmployeeLeaveBalance")]
    public class EmployeeLeaveBalance
    {
        public string EmployeeId { get; set; }
        public int LeaveType { get; set; }
        public float NoOfDays { get; set; }
        [ForeignKey("EmployeeId")]
        public virtual Employee Employee { get; set; }
    }
}
