using System.Threading;
using System.Threading.Tasks;

namespace Postmen.Sender.Application.Interfaces
{
    public interface IApplicationService
    {
        Task PublishAsync(PostRequest request, CancellationToken cancellationToken);
    }
}