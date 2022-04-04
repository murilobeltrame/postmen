using System;
using System.Threading;
using System.Threading.Tasks;

namespace Postmen.Domain.Interfaces
{
    public interface IBroker
    {
        Task PublishAsync<T>(string topicName, T payload, CancellationToken cancellationToken = default);

        Task ReceiveAsync<T>(string topicName, string subscriptionName, Func<T, Task> handler, Func<Exception, Task> errorhandler = null, CancellationToken cancellationToken = default);

        Task ReceiveAsync<T>(string topicName, string subscriptionName, Func<T, Task> handler, CancellationToken cancellationToken = default);
    }
}
