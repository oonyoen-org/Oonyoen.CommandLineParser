using CommandLine;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Oonyoen.CommandLineParser.AspNetCore
{
    public class InteractiveCommandLineService : IHostedService
    {
        private readonly CancellationTokenSource stoppingTokenSource = new CancellationTokenSource();
        private readonly TaskCompletionSource stopped = new TaskCompletionSource();
        private readonly IServiceProvider serviceProvider;
        private readonly IOptions<InteractiveCommandLineOptions> options;
        private readonly ILogger<InteractiveCommandLineService> logger;

        public InteractiveCommandLineService(
            IServiceProvider serviceProvider,
            IOptions<InteractiveCommandLineOptions> options,
            ILogger<InteractiveCommandLineService> logger)
        {
            this.serviceProvider = serviceProvider;
            this.options = options;
            this.logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(() =>
            {
                try
                {
                    while (!stoppingTokenSource.IsCancellationRequested)
                    {
                        var nextLine = Console.ReadLine();
                        var args = StringArgumentifier.SplitCommandLine(nextLine);
                        var result = Parser.Default.ParseArguments(args, options.Value.Verbs.Keys.ToArray());
                        foreach (var (type, handler) in options.Value.Verbs)
                        {
                            handler(result, serviceProvider);
                        }
                        options.Value.ErrorHandler?.Invoke(result, serviceProvider);
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError($"Encountered an exception in the {nameof(InteractiveCommandLineService)} execution loop!", ex);
                }
            });
            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            stoppingTokenSource.Cancel();
            await stopped.Task;
        }
    }
}
