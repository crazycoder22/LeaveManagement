namespace LeaveManagement.Common.Model
{
    public class ApplicationRequestAction
    {
        public string RequestId { get; set; }
        public Enum.Action Action { get; set; }
        public string Comment { get; set; }
    }
}
