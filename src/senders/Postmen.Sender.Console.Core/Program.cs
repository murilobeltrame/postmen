using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Postmen.Domain.Interfaces;
using Postmen.Infrastructure;
using Postmen.Sender.Application;
using Postmen.Sender.Application.Interfaces;
using System.Threading.Tasks;

namespace Postmen.Sender.Console.Core
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
                        .AddSingleton<IBroker, Broker>(s => new Broker(hostContext.Configuration.GetSection("ConnectionStrings:ServiceBus").Value))
                        .AddSingleton<IApplicationService, ApplicationService>();
                })
                .RunConsoleAsync();
        }
    }
}
