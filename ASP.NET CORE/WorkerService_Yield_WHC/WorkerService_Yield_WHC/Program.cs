using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkerService_Yield_WHC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseWindowsService()
             .ConfigureLogging((_, builder) =>
             {
                 builder.AddFile("logs/app-{Date}.json", isJson: true);
             })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddTransient<IWlpMesService, WLP_Mes_Service>();
                    services.AddHostedService<Worker>();

                });
    }
}
