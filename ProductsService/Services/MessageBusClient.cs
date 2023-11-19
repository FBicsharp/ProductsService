using Azure.Core;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Net.Mime;
using System.Text;

namespace ProductsService.Model
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly ILogger<BolbService> _logger;

        public MessageBusClient(ILogger<BolbService> logger)
        {
            _logger = logger;
        }

        public Task SendMessageAsync<T>(T notification)
        {
            throw new NotImplementedException();
        }
    }

    public interface IMessageBusClient
    {
        Task SendMessageAsync<T>(T notification);



    }


    public record MessageBusClientMessageTempalte(string event_type, string serializedObject );
    public class MessageBusClientMessageBuilder
    {

        public MessageBusClientMessageTempalte Build<T>(object notificationObject)  
        {
            //trovo propietà di 


            return new MessageBusClientMessageTempalte("ProductCreated", "serializedObject");
        }
        

    }

}