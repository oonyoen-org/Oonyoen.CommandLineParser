using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Oonyoen.CommandLineParser.AspNetCore;

namespace Oonyoen.CommandLineParser.Sample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    services.AddTransient<IVerbHandler<PrintOptions>, PrintHandler>();
                })
                .UseInteractiveCommandLine(options =>
                {
                    options.AddVerb<PrintOptions>();
                })
                .ConfigureLogging(logging =>
                {
                    logging.AddConsole();
                });
    }
}
