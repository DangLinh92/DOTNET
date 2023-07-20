using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CarMNS.Application.Interfaces
{
    public interface IBackgroundService
    {
        Task DoWork(CancellationToken stoppingToken, ILogger logger);
    }
}
