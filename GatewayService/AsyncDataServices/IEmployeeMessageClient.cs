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
    public interface IEmployeeMessageClient {     
        Task<string> PublishEmployeeMessage(object actionPublished);
        
    }
}