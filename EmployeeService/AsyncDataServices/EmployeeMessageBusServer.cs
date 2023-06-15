using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using EmployeeService.EventsProcessing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;


namespace EmployeeService.AsyncDataServices
{
    public class EmployeeMessageBusServer : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IEventsProcessor _eventsProcessor;
        private IConnection _connection;
        private IModel _channel;
        private string _queueName;

        public EmployeeMessageBusServer(
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
            //  var factory = new ConnectionFactory {
            //      Uri = new Uri("amqp://guest:guest@localhost:5673"),
               
            //  };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            // _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
            // _queueName = _channel.QueueDeclare().QueueName;
            // _channel.QueueBind(queue: _queueName,
            //     exchange: "trigger",
            //     routingKey: "");
            _channel.QueueDeclare("employee-request", exclusive:false, autoDelete: false);


            Console.WriteLine("--> Listenting on the Message Bus...");

            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (ModuleHandle, ea) =>
            {
                Console.WriteLine("--> Event Received!");

                var body = ea.Body;
                var notificationMessage = Encoding.UTF8.GetString(body.ToArray());

                var response = _eventsProcessor.ProcessEvent(notificationMessage);

                
                var responseBody = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(response));
            
                
                _channel.BasicPublish("", ea.BasicProperties.ReplyTo, ea.BasicProperties, responseBody);
                Console.WriteLine($"response {response}");
            };

            _channel.BasicConsume(queue: "employee-request", autoAck: true, consumer: consumer);

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