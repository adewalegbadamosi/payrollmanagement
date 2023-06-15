
using Microsoft.Extensions.Configuration;

using System.Threading.Tasks;

namespace SalaryService.AsyncDataServices
{
    public class EmployeeMessageClient : IEmployeeMessageClient
    {
        private string requestQueueName = "employee-request";
        private string replyQueueName = "salary-employee-response";
        
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