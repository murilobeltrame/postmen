using Postmen.Domain.Abstractions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Postmen.Domain.Interfaces
{
    public interface IBroker
    {
        Task PublishAsync<T>(string topicName, T payload, CancellationToken cancellationToken = default) where T : JsonSerializableEntity<T>;

        Task ReceiveAsync<T>(string topicName, string subscriptionName, Func<T, Task> handler, Func<Exception, Task> errorhandler = null, CancellationToken cancellationToken = default) where T : JsonSerializableEntity<T>;

        Task ReceiveAsync<T>(string topicName, string subscriptionName, Func<T, Task> handler, CancellationToken cancellationToken = default) where T : JsonSerializableEntity<T>;
    }
}
