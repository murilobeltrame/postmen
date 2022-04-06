using Postmen.Infrastructure;
using Postmen.Sender.Application;
using System;
using System.Threading.Tasks;

namespace Postmen.Sender.Console.Framework
{
    sealed class Program
    {
        static async Task Main(string[] args)
        {
            var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings:ServiceBus");
            var broker = new Broker(connectionString);
            var service = new ApplicationService(broker);

            while (true)
            {
                System.Console.WriteLine("Type your message:");
                var message = System.Console.ReadLine();
                await service.PublishAsync(new PostRequest { Description = message }, default);
                System.Console.WriteLine("Message sent...");
            }
        }
    }
}
