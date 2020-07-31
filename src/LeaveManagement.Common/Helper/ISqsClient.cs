using System.Threading.Tasks;

namespace LeaveManagement.Common.Helper
{
    public interface ISqsClient
    {
        Task SendMessage(string message);
    }
}
