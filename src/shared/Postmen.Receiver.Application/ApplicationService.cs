using Postmen.Domain;
using Postmen.Domain.Interfaces;

namespace Postmen.Receiver.Application
{
    public class ApplicationService
    {
        private readonly IBroker _broker;
        private readonly string _subscriptionName;

        public ApplicationService(IBroker broker, string subscriptionName)
        {
            _broker = broker;
            _subscriptionName = subscriptionName;
        }

        public async Task Receive(Func<Post, Task> handler)
        {
            await _broker.Listen("posted", _subscriptionName, handler, null);
        }
    }
}
