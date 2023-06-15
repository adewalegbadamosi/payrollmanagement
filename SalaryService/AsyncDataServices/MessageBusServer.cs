using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using SalaryService.EventsProcessing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;


namespace SalaryService.AsyncDataServices
{
    public class MessageBusServer : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IEventsProcessor _eventsProcessor;
        private IConnection _connection;
        private IModel _channel;
        private QueueDeclareOk _queueName ;
        // private readonly rabbitMQConnection = new RabbitMQConnection(_configuration);

        public MessageBusServer(
            IConfiguration configuration, 
            IEventsProcessor eventsProcessor)
        {
            _configuration = configuration;
            _eventsProcessor = eventsProcessor;
            

            InitializeRabbitMQ();
        }

        private void InitializeRabbitMQ()
        {
            var factory = new ConnectionFactory() { 
                HostName = _configuration["RabbitMQHost"],
                UserName = _configuration["RabbitMQUser"],
                Password = _configuration["RabbitMQPass"],
                Port = int.Parse(_configuration["RabbitMQPort"]),
                VirtualHost = "/"              
                };
       
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
           
           _queueName = _channel.QueueDeclare("salary-request", durable:false, exclusive: false, autoDelete:false);


            Console.WriteLine("--> Listenting on the salary Message Bus...");

            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (ModuleHandle, ea) =>
            {
                Console.WriteLine("--> Event Received in salary server!");

                var body = ea.Body;
                var notificationMessage = Encoding.UTF8.GetString(body.ToArray());

                var response = _eventsProcessor.ProcessSalaryEvent(notificationMessage);
                
                var responseBody = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(response));
            
                
                _channel.BasicPublish("", ea.BasicProperties.ReplyTo, ea.BasicProperties, responseBody);
                Console.WriteLine($"response {response}");
            };

            _channel.BasicConsume(queue: _queueName.QueueName, autoAck: true, consumer: consumer);

            return Task.CompletedTask;
        }

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--> Connection Shutdown");
        }

        public override void Dispose()
        {
            if(_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }

            base.Dispose(); 
        }
    }
}