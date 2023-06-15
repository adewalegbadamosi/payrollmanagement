
using Microsoft.Extensions.Configuration;

using System.Threading.Tasks;

namespace GatewayService.AsyncDataServices
{
    public class SalaryMessageClient : ISalaryMessageClient
    {     

        private string requestQueueName = "salary-request";
        private string replyQueueName = "salary-response";
        
        private MessageBusClient _messageBusClient;

        public SalaryMessageClient (IConfiguration configuration) 
        {
                   
            _messageBusClient = new MessageBusClient(configuration,requestQueueName, replyQueueName);
         
        }

       
   
        
        public async Task<string> PublishSalaryMessage(object actionPublished)
        {
            return await _messageBusClient.PublishNewMessage(actionPublished);
          
        }

       

        
    }
}