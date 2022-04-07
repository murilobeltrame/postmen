using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Postmen.Receiver.Application.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Postmen.Receiver.Console.Core
{
    sealed class ConsoleHostedService : IHostedService
    {
        private int? _exitCode;
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
                        await _applicationService.Listen(async post =>
                        {
                            _logger.LogInformation("Received post {0}", post);
                            await Task.CompletedTask;
                        }, async exception =>
                        {
                            _logger.LogError(exception.Message);
                            await Task.CompletedTask;
                        }, cancellationToken);

                        _exitCode = 0;
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
