using Postmen.Domain.Interfaces;

namespace Postmen.Sender.Application
{
    public class ApplicationService
    {
        private readonly IBroker _broker;

        public ApplicationService(IBroker broker)
        {
            _broker = broker;
        }

        public async Task Publish(PostRequest request)
        {
            await _broker.Publish("posted", request.ToEntity());
        }
    }

}
