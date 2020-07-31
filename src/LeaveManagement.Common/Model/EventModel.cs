namespace LeaveManagement.Common.Model
{
    public class EventModel
    {
        public string TemplateId { get; set; }
        public string EmailAddress { get; set; }
        public string Subject { get; set; }
        public Parameter Parameter { get; set; }

    }
}
