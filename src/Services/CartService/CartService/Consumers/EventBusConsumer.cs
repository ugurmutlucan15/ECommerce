using CartService.Repositories.Interfaces;

using EventBusRabbitMQ;
using EventBusRabbitMQ.Core;
using EventBusRabbitMQ.Events;

using Newtonsoft.Json;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CartService.Consumers
{
    public class EventBusConsumer
    {
        private readonly IRabbitMqPersistentConnection _persistentConnection;
        private readonly ICartRepository _repository;

        public EventBusConsumer(IRabbitMqPersistentConnection persistentConnection,
            ICartRepository repository)
        {
            _persistentConnection =
                persistentConnection ?? throw new ArgumentNullException(nameof(persistentConnection));
            _repository = repository;
        }

        public void Consume()
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            var channel = _persistentConnection.CreateModel();
            channel.QueueDeclare(queue: EventBusConstants.ComplateQueue, durable: false, exclusive: false,
                autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += ReceivedEvent;

            channel.BasicConsume(queue: EventBusConstants.ComplateQueue, autoAck: true, consumer: consumer);
        }

        private async void ReceivedEvent(object sender, BasicDeliverEventArgs e)
        {
            var message = Encoding.UTF8.GetString(e.Body.Span);
            var @event = JsonConvert.DeserializeObject<CreateEventModel>(message);

            if (e.RoutingKey == EventBusConstants.ComplateQueue)
            {
                var data = JsonConvert.DeserializeObject<List<CartEventModel>>(@event!.Data.ToString()!);
                await _repository.DeleteCart(data!.FirstOrDefault()!.UserId.ToString());
            }
        }

        public void Disconnect()
        {
            _persistentConnection.Dispose();
        }
    }
}