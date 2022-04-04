using Postmen.Domain.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Postmen.Sender.Application
{
    public class ApplicationService
    {
        private readonly IBroker _broker;

        public ApplicationService(IBroker broker)
        {
            if (broker != null) throw new ArgumentNullException(nameof(broker));

            _broker = broker;
        }

        public async Task PublishAsync(PostRequest request, CancellationToken cancellationToken)
        {
            var post = request.ToEntity();
            await _broker.PublishAsync("postcreated", post, cancellationToken);
        }
    }
}
