using Postmen.Infrastructure;
using Postmen.Receiver.Application;
using System;
using System.Threading.Tasks;

namespace Postmen.Receiver.Console.Framework
{
    sealed class Program
    {
        static async Task Main(string[] args)
        {
            var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings:ServiceBus");
            var broker = new Broker(connectionString);
            var service = new ApplicationService(broker, "postcreatedsubscriptionframework");

            await service.Listen(async post =>
            {
                System.Console.WriteLine("Received post {0}", post);
                await Task.CompletedTask;
            }, async exception =>
            {
                System.Console.WriteLine(exception.Message);
                await Task.CompletedTask;
            }, default);

            System.Console.WriteLine("Hit ENTER to quit.");
            System.Console.ReadLine();
        }
    }
}
