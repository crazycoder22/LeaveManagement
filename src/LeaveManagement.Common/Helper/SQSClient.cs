using Amazon;
using Amazon.SQS;
using LeaveManagement.Common.Model;
using System.Threading.Tasks;

namespace LeaveManagement.Common.Helper
{
    public class SQSClient : ISqsClient
    {
        private AmazonSQSClient _sqsClient;
        private SqsClientConfig _sqsClientConfig;
        public SQSClient(SqsClientConfig sqsClientConfig)
        {
            _sqsClientConfig = sqsClientConfig;
            var amazonSqsConfig = new AmazonSQSConfig
            {
                RegionEndpoint = RegionEndpoint.GetBySystemName(_sqsClientConfig.Region),
            };
            _sqsClient = new AmazonSQSClient(sqsClientConfig.AccessKey, sqsClientConfig.Secret, amazonSqsConfig);
        }

        public async Task SendMessage(string message)
        {
            await _sqsClient.SendMessageAsync(_sqsClientConfig.Url, message);
        }
    }
}
