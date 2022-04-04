using Azure.Messaging.ServiceBus;
using Postmen.Domain.Interfaces;
using System.Text.Json;

namespace Postmen.Infrastructure
{
    public class Broker : IBroker, IDisposable, IAsyncDisposable
    {
        private ServiceBusClient? _client;
        private List<ServiceBusProcessor>? _processors;

        public Broker(string connectionString)
        {
            _client = new ServiceBusClient(connectionString);
            _processors = new List<ServiceBusProcessor>();
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public async ValueTask DisposeAsync()
        {
            await DisposeAsyncCore().ConfigureAwait(false);

            Dispose(disposing: false);
#pragma warning disable CA1816 // Dispose methods should call SuppressFinalize
            GC.SuppressFinalize(this);
#pragma warning restore CA1816 // Dispose methods should call SuppressFinalize
        }

        public async Task Listen<T>(string topicName, string subscriptionName, Func<T, Task> handler, Func<Exception, Task>? exceptionHandler)
        {
            if (string.IsNullOrWhiteSpace(topicName)) throw new ArgumentNullException(nameof(topicName));
            if (string.IsNullOrWhiteSpace(subscriptionName)) throw new ArgumentNullException(nameof(subscriptionName));
            if (handler == null) throw new ArgumentNullException(nameof(handler));
            if (_client == null) throw new Exception("Client disconnected");

            var processor = _client.CreateProcessor(topicName, subscriptionName);
            if (_processors != null) _processors.Add(processor);

            processor.ProcessMessageAsync += async (args) =>
            {
                var parsedPayload = JsonSerializer.Deserialize<T>(args.Message.Body);
                await handler(parsedPayload!);
                await args.CompleteMessageAsync(args.Message);
            };
            if (exceptionHandler != null)
            {
                processor.ProcessErrorAsync += args => exceptionHandler(args.Exception);
            }
            await processor.StartProcessingAsync();
        }

        public async Task Publish<T>(string topicName, T payload)
        {
            if (string.IsNullOrWhiteSpace(topicName)) throw new ArgumentNullException(nameof(topicName));
            if (payload == null) throw new ArgumentNullException(nameof(payload));
            if (_client == null) throw new Exception("Client disconnected");

            var sender = _client.CreateSender(topicName);
            var json = JsonSerializer.Serialize(payload);
            var message = new ServiceBusMessage(json);
            await sender.SendMessageAsync(message);
        }

        protected virtual async ValueTask DisposeAsyncCore()
        {
            if (_processors != null)
            {
                foreach (var processor in _processors)
                {
                    await processor.DisposeAsync().ConfigureAwait(false);
                }
                _processors = null;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _processors = null;
                _client = null;
            }
        }
    }
}

