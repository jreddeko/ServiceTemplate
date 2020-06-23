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
            _rootConfiguration = rootConfiguration;
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
                    _logger.LogInformation("Sleeping for {@Time} min", _rootConfiguration.TimerConfiguration.DueTimeInMin);
                    await Task.Delay(_rootConfiguration.TimerConfiguration.DueTimeInMin * 1000 * 60, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    throw ex;
                }
            }
        }
    }
}