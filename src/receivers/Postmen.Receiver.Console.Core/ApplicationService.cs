using Postmen.Domain.Interfaces;

namespace Postmen.Receiver.Console.Core
{
    class ApplicationService: Application.ApplicationService
    {
        public ApplicationService(IBroker broker) : base(broker, "postcreatedsubscriptioncore") { }
    }
}
