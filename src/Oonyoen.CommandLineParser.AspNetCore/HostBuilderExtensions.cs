using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Oonyoen.CommandLineParser.AspNetCore
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder UseInteractiveCommandLine(this IHostBuilder hostBuilder, Action<InteractiveCommandLineOptions> configure)
        {
            hostBuilder
                .ConfigureServices((context, services) =>
                {
                    if (!services.Any(service => service.ServiceType == typeof(InteractiveCommandLineService)))
                    {
                        services.AddHostedService<InteractiveCommandLineService>();
                    }

                    services.Configure(configure);
                });

            return hostBuilder;
        }
    }
}
