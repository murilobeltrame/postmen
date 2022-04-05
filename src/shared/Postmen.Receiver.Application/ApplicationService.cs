using Postmen.Domain;
using Postmen.Domain.Interfaces;
using Postmen.Receiver.Application.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Postmen.Receiver.Application
{
    public class ApplicationService : IApplicationService
    {
        private readonly IBroker _broker;
        private readonly string _subscriptionName;

        public ApplicationService(IBroker broker, string subscriptionName)
        {
            if (string.IsNullOrWhiteSpace(subscriptionName)) throw new ArgumentNullException(nameof(subscriptionName));
            if (broker == null) throw new ArgumentNullException(nameof(broker));

            _broker = broker;
            _subscriptionName = subscriptionName;
        }

        public async Task Listen(Func<Post, Task> handler, Func<Exception, Task> errorHandler, CancellationToken cancellationToken)
        {
            await _broker.ReceiveAsync("postcreated", _subscriptionName, handler, errorHandler, cancellationToken);
        }
    }
}
