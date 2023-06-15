
using Microsoft.Extensions.Configuration;

using System.Threading.Tasks;

namespace GatewayService.AsyncDataServices
{
    public class EmployeeMessageClient : IEmployeeMessageClient
    {
        private string requestQueueName = "employee-request";
        private string replyQueueName = "employee-response";
        
        private MessageBusClient _messageBusClient;
        public EmployeeMessageClient (IConfiguration configuration) 
        {            
            _messageBusClient = new MessageBusClient(configuration,requestQueueName, replyQueueName);            
        }     
        
        public async Task<string> PublishEmployeeMessage(object actionPublished)
        {
            return await _messageBusClient.PublishNewMessage(actionPublished);
            
        }

    }
}