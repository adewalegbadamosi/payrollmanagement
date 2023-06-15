
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;


namespace GatewayService.AsyncDataServices
{
   public class RabbitMQConnection
   {
        private readonly IConfiguration _configuration;

      public RabbitMQConnection (IConfiguration configuration)
        {
            _configuration = configuration;          
        }

        public ConnectionFactory Conn (){
               var factory = new ConnectionFactory() { 
                HostName = _configuration["RabbitMQHost"],
                UserName = _configuration["RabbitMQUser"],
                Password = _configuration["RabbitMQPass"],
                Port = int.Parse(_configuration["RabbitMQPort"]),
                VirtualHost = "/"              
            }; 

            return factory;
        }
   }
}