using Azure.Messaging.ServiceBus;
using Postmen.Domain.Abstractions;
using Postmen.Domain.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Postmen.Infrastructure
{
    public class Broker : IBroker
    {
        private readonly ServiceBusClient _client;

        public Broker(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString)) throw new ArgumentNullException(nameof(connectionString));

            _client = new ServiceBusClient(connectionString);
        }

        public async Task PublishAsync<T>(string topicName, T payload, CancellationToken cancellationToken = default) where T: JsonSerializableEntity<T>
        {
            if (string.IsNullOrWhiteSpace(topicName)) throw new ArgumentNullException(nameof(topicName));
            if (payload == null) throw new ArgumentNullException(nameof(payload));

            var sender = _client.CreateSender(topicName);
            var message = new ServiceBusMessage(payload.ToJson());
            await sender.SendMessageAsync(message, cancellationToken);
        }

        public async Task ReceiveAsync<T>(string topicName, string subscriptionName, Func<T, Task> handler, Func<Exception, Task> errorhandler = null, CancellationToken cancellationToken = default) where T : JsonSerializableEntity<T>
        {
            if (string.IsNullOrWhiteSpace(topicName)) throw new ArgumentNullException(nameof(topicName));
            if (string.IsNullOrWhiteSpace(subscriptionName)) throw new ArgumentNullException(nameof(subscriptionName));
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            var processor = _client.CreateProcessor(topicName, subscriptionName, new ServiceBusProcessorOptions
            {
                AutoCompleteMessages = true
            });
            processor.ProcessMessageAsync += async args =>
            {
                var payload = JsonSerializableEntity<T>.FromJson(args.Message.Body.ToString());
                await handler(payload);
            };
            if (errorhandler != null)
            {
                processor.ProcessErrorAsync += args => errorhandler(args.Exception);
            }
            await processor.StartProcessingAsync();
        }

        public async Task ReceiveAsync<T>(string topicName, string subscriptionName, Func<T, Task> handler, CancellationToken cancellationToken = default) where T : JsonSerializableEntity<T>
        {
            await ReceiveAsync(topicName, subscriptionName, handler, null, cancellationToken);
        }
    }
}
