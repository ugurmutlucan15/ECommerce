using AutoMapper;

using EventBusRabbitMQ;
using EventBusRabbitMQ.Contact;
using EventBusRabbitMQ.Core;
using EventBusRabbitMQ.Events;

using Newtonsoft.Json;

using OrderService.Entities;
using OrderService.Repositories.Interfaces;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Consumers
{
    public class EventBusConsumer
    {
        private readonly IRabbitMqPersistentConnection _persistentConnection;
        private readonly IMapper _mapper;
        private readonly IOrderRepository _repository;
        private readonly EventBusRabbitMqECommerce _eventBus;

        public EventBusConsumer(IRabbitMqPersistentConnection persistentConnection, IMapper mapper,
            IOrderRepository repository, EventBusRabbitMqECommerce eventBus)
        {
            _persistentConnection =
                persistentConnection ?? throw new ArgumentNullException(nameof(persistentConnection));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _repository = repository;
            _eventBus = eventBus;
        }

        public void Consume()
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            var channel = _persistentConnection.CreateModel();
            channel.QueueDeclare(queue: EventBusConstants.CreateQueue, durable: false, exclusive: false,
                autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += ReceivedEvent;

            channel.BasicConsume(queue: EventBusConstants.CreateQueue, autoAck: true, consumer: consumer);
        }

        private async void ReceivedEvent(object sender, BasicDeliverEventArgs e)
        {
            var message = Encoding.UTF8.GetString(e.Body.Span);
            var @event = JsonConvert.DeserializeObject<CreateEventModel>(message);

            if (e.RoutingKey == EventBusConstants.CreateQueue)
            {
                var data = JsonConvert.DeserializeObject<IEnumerable<CartEventModel>>(@event!.Data.ToString()!);
                foreach (var item in data)
                {
                    var model = _mapper.Map<Order>(item);
                    await _repository.Add(model);
                }

                _eventBus.Publish(EventBusConstants.ComplateQueue, @event);
            }
        }

        public void Disconnect()
        {
            _persistentConnection.Dispose();
        }
    }
}