using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LeaveManagement.DataAccess.DataModel
{
    [Table("ApplicationRequest")]
    public class ApplicationRequest
    {
        [Key]
        public Guid Id { get; set; }
        public int RequestType { get; set; }
        public string RequesterId { get; set; }
        public string ApproverId { get; set; }
        public string RequesterComment { get; set; }
        public string ApproverComment { get; set; }
        public string RequestDetails { get; set; }
        public int Status { get; set; }
    }
}
