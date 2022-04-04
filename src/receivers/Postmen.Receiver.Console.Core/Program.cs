using Postmen.Infrastructure;
using Postmen.Receiver.Application;
using System.Threading.Tasks;

namespace Postmen.Receiver.Console.Core
{
    class Program
    {
        protected Program() { }

        static async Task Main(string[] args)
        {
            System.Console.WriteLine("Starting to Receiving messages");

            var broker = new Broker("");
            var service = new ApplicationService(broker, "postcreatedsubscriptioncore");
            await service.Listen(post =>
            {
                System.Console.WriteLine("Received {0}",post.ToJson());
                return Task.CompletedTask;
            }, exception =>
            {
                System.Console.WriteLine("Error receiving message: {0}", exception);
                return Task.CompletedTask;
            }, default);

            System.Console.ReadLine();
        }
    }
}
