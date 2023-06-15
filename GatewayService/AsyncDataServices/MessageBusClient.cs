using System;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using GatewayService.Dto;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Newtonsoft.Json.Linq;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace GatewayService.AsyncDataServices
{
    public class MessageBusClient : RabbitMQConnection
    {
        private readonly IConfiguration _configuration;
        private readonly string _requestQueueName = "";
        private readonly string _responseQueueName = "";        
        private  IConnection _connection;
        private  IModel _channel;
        private QueueDeclareOk _replyQueue;        
        private ConcurrentDictionary < string, TaskCompletionSource < string >> _activeTaskQueue = new ConcurrentDictionary < string, TaskCompletionSource < string >> ();  
        public MessageBusClient (IConfiguration configuration, string requestQueueName, string responseQueueName) : base(configuration)
        {
            _configuration = configuration;          
            _requestQueueName = requestQueueName;
            _responseQueueName = responseQueueName;
        
           try
            {
                var factory =  Conn();
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
                _channel.QueueDeclare("", exclusive:false);                
                // _replyQueue = _channel.QueueDeclare(queue: responseQueueName, exclusive: false);
                _replyQueue = _channel.QueueDeclare(responseQueueName, durable:false, exclusive: false, autoDelete:false);

             
                var consumer = new EventingBasicConsumer(_channel);
                consumer.Received += Consumer_Received;
                _channel.BasicConsume(queue: _replyQueue.QueueName, consumer: consumer, autoAck: true);

              
                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;

                Console.WriteLine("--> Connected to MessageBus");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not connect to the Message Bus: {ex.Message}");
            }
        }
 
     
        private void Consumer_Received(object ? sender, BasicDeliverEventArgs args) {
            var body = args.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            if (_activeTaskQueue.TryRemove(args.BasicProperties.CorrelationId, out
                    var taskCompletionSource)) {
                taskCompletionSource.SetResult(message);
            }
        }
   
        public Task<string> PublishNewMessage(object actionPublished)
        {
            var message = JsonSerializer.Serialize(actionPublished);
            if (_connection.IsOpen)
            {
                Console.WriteLine("--> RabbitMQ Connection Open, sending message...");

                var properties = _channel.CreateBasicProperties();
                properties.ReplyTo = _responseQueueName; 
                var messageId = Guid.NewGuid().ToString();
                properties.CorrelationId = messageId;
                var body = Encoding.UTF8.GetBytes(message);
                _channel.BasicPublish("", _requestQueueName, properties, body); 

                Console.WriteLine($"--> We have sent {message}"); 

                var taskCompletionSource = new TaskCompletionSource < string > ();
                _activeTaskQueue.TryAdd(messageId, taskCompletionSource);
                return taskCompletionSource.Task;

                
            }
            else
            {
                Console.WriteLine("--> RabbitMQ connectionis closed, not sending");
            } 
            return null;  
        }

       

        public void Dispose()
        {
            Console.WriteLine("MessageBus Disposed");
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
        }

        private void RabbitMQ_ConnectionShutdown(object? sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--> RabbitMQ Connection Shutdown");
        }
    }
}