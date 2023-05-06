using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WorkerService_Yield_WHC
{
    public class Worker : IHostedService, IDisposable
    {
        private readonly ILogger<Worker> _logger;
        private readonly IWlpMesService _wlpMesService;
        private Timer _timer;
        private static object _lock = new object();
        private int _counter = 0;

        public Worker(ILogger<Worker> logger, IWlpMesService wlpMesService)
        {
            _logger = logger;
            _wlpMesService = wlpMesService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(60));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        private void DoWork(object state)
        {
            _logger.LogDebug($"Try to execute next iteration {_counter + 1} of DoWork ");
            if (Monitor.TryEnter(_lock))
            {
                try
                {
                    _logger.LogDebug($"Running DoWork iteration {_counter}");
                    _wlpMesService.UpdateOuputDaiLyTrend();
                    _logger.LogDebug($"DoWork {_counter} finished, will start iteration {_counter + 1}");
                }
                finally
                {
                    _counter++;
                    Monitor.Exit(_lock);
                }
            }
        }
    }
}
