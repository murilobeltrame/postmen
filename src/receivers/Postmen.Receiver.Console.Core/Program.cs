using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Postmen.Domain.Interfaces;
using Postmen.Infrastructure;
using Postmen.Receiver.Application;
using System;
using System.Threading;
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
                        .AddSingleton<IBroker, Broker>() // TODO: Configuration
                        .AddScoped<ApplicationService>();
                })
                .RunConsoleAsync();
        }
    }

    sealed class ConsoleHostedService : IHostedService
    {
        private readonly ApplicationService _applicationService;
        private readonly ILogger _logger;
        private readonly IHostApplicationLifetime _appLifetime;

        public ConsoleHostedService(
            ILogger<ConsoleHostedService> logger,
            IHostApplicationLifetime appLifetime, 
            ApplicationService applicationService)
        {
            _applicationService = applicationService;
            _logger = logger;
            _appLifetime = appLifetime;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Starting with arguments: {string.Join(" ", Environment.GetCommandLineArgs())}");

            _appLifetime.ApplicationStarted.Register(() =>
            {
                Task.Run(async () =>
                {
                    try
                    {
                        _logger.LogInformation("Hello World!");
                        await _applicationService.Listen(async post =>
                        {
                            _logger.LogInformation("Received post {0}", post);
                            await Task.CompletedTask;
                        }, async exception =>
                        {
                            _logger.LogError(exception.Message);
                            await Task.CompletedTask;
                        }, cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Unhandled exception!");
                    }
                    finally
                    {
                        // Stop the application once the work is done
                        _appLifetime.StopApplication();
                    }
                });
            });

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
