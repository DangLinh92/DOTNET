using HRMNS.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.HostedService
{
    public class MyBackgroundService : BackgroundService
    {
        private readonly ILogger<MyBackgroundService> _logger;
        public IServiceProvider Services { get; }
        private int executionCount = 0;

        public MyBackgroundService(IServiceProvider services,
        ILogger<MyBackgroundService> logger)
        {
            Services = services;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await DoWork(stoppingToken);
        }

        private async Task DoWork(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Background Service Hosted Service is working." + DateTime.Now);

            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = Services.CreateScope())
                {
                    if (DateTime.Now.ToString("HH:mm:ss").CompareTo("23:00:00") <= 0 && executionCount == 0)
                    {
                        _logger.LogInformation("DoWork: active " + DateTime.Now.ToString("HH:mm:ss"));

                        var bgService =
                       scope.ServiceProvider
                           .GetRequiredService<IBackgroundService>();

                        await bgService.DoWork(stoppingToken, _logger);

                        executionCount++;
                    }

                    if (DateTime.Now.ToString("HH:mm:ss").CompareTo("23:00:00") > 0 && executionCount > 0)
                    {
                        executionCount = 0;
                        _logger.LogInformation("DoWork: reset count = 0 " + DateTime.Now.ToString("HH:mm:ss"));
                    }

                    await Task.Delay(new TimeSpan(0, 0, 1));
                }
            }
        }
    }
}
