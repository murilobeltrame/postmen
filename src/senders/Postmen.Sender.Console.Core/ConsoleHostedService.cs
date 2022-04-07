using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Postmen.Sender.Application.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Postmen.Sender.Console.Core
{
    sealed class ConsoleHostedService : IHostedService
    {
        private int? _exitCode = 0;
        private readonly IApplicationService _applicationService;
        private readonly ILogger _logger;
        private readonly IHostApplicationLifetime _appLifetime;

        public ConsoleHostedService(
            ILogger<ConsoleHostedService> logger,
            IHostApplicationLifetime appLifetime,
            IApplicationService applicationService)
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
                        while (true)
                        {
                            System.Console.WriteLine("Type your message:");
                            var message = System.Console.ReadLine();
                            await _applicationService.PublishAsync(new Application.PostRequest { Description = message }, default);
                            _logger.LogInformation("Message sent...");
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Unhandled exception!");
                        _exitCode = -99;
                    }
                });
            });

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Environment.ExitCode = _exitCode.GetValueOrDefault(-1);
            return Task.CompletedTask;
        }
    }
}
