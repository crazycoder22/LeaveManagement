using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LeaveManagement.DataAccess.DataModel
{
    [Table("Employee")]
    public class Employee
    {
        [Key]
        public string EmployeeId { get; set; }
        public string EmailAddress { get; set; }
        public DateTime DateOfJoining { get; set; }
        public string ManagerId { get; set; }
        [ForeignKey("ManagerId")]
        public virtual Employee Manager { get; set; }
    }
}
