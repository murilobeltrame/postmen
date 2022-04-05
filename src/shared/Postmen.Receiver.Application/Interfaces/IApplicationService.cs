using Postmen.Domain;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Postmen.Receiver.Application.Interfaces
{
    public interface IApplicationService
    {
        Task Listen(Func<Post, Task> handler, Func<Exception, Task> errorHandler, CancellationToken cancellationToken);
    }
}