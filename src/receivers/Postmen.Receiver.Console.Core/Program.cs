using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Postmen.Domain.Interfaces;
using Postmen.Infrastructure;
using Postmen.Receiver.Application.Interfaces;
using System.Threading.Tasks;

namespace Postmen.Receiver.Console.Core
{
    sealed class Program
    {
        private Program() { }

        static async Task Main(string[] args)
        {
            await Host
                .CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) => {
                    services
                        .AddHostedService<ConsoleHostedService>()
                        .AddSingleton<IBroker, Broker>(s => new Broker(hostContext.Configuration.GetSection("ConnectionStrings:ServiceBus").Value)) // TODO: Configuration
                        .AddSingleton<IApplicationService, ApplicationService>();
                })
                .RunConsoleAsync();
        }
    }
}
