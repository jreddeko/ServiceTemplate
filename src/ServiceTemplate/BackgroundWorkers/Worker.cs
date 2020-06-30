using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ServiceTemplate.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceTemplate
{
    internal class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IRootConfiguration _rootConfiguration;

        public Worker(ILogger<Worker> logger,
            IRootConfiguration rootConfiguration)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _rootConfiguration = rootConfiguration ?? throw new ArgumentNullException(nameof(rootConfiguration));
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            Log.WorkerGracefulShutdown(_logger);
            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting execution");

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                try
                {
                    // do some task

                    Log.WorkerSleeping(_logger, _rootConfiguration.TimerConfiguration.DueTimeInMin);
                    await Task.Delay(_rootConfiguration.TimerConfiguration.DueTimeInMin * 1000 * 60, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    throw ex;
                }
            }
            Log.WorkerGracefulShutdown(_logger);
        }

        private static class Log
        {
            private static readonly Action<ILogger, Exception> _workerStopping = LoggerMessage.Define(
                LogLevel.Information,
                EventIds.WorkerStopping,
                "Worker stopping.");

            private static readonly Action<ILogger, Exception> _workerGracefulShutdown = LoggerMessage.Define(
                LogLevel.Information,
                EventIds.WorkerGracefulShutdown,
                "Worker gracefully shut down.");

            private static readonly Action<ILogger, int, Exception> _workerSleeping = LoggerMessage.Define<int>(
                LogLevel.Information,
                EventIds.WorkerGracefulShutdown,
                "Worker sleeping for '{dueTimeInMin}'.");

            public static void WorkerStopping(ILogger logger)
            {
                _workerStopping(logger, null);
            }

            public static void WorkerGracefulShutdown(ILogger logger)
            {
                _workerGracefulShutdown(logger, null);
            }

            public static void WorkerSleeping(ILogger logger, int dueTimeInMin)
            {
                _workerSleeping(logger, dueTimeInMin, null);
            }
        }
    }
}