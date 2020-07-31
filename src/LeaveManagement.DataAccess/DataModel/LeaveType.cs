using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LeaveManagement.DataAccess.DataModel
{
    [Table("LeaveType")]
    public class LeaveType
    {
        [Key]
        public string Type { get; set; }
        public int NoOfDays { get; set; }
    }
}
