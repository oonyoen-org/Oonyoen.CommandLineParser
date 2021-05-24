using Microsoft.Extensions.Logging;
using Oonyoen.CommandLineParser.AspNetCore;

namespace Oonyoen.CommandLineParser.Sample
{
    public class PrintHandler : IVerbHandler<PrintOptions>
    {
        private readonly ILogger<PrintHandler> logger;

        public PrintHandler(ILogger<PrintHandler> logger)
        {
            this.logger = logger;
        }

        public void Handle(PrintOptions result)
        {
            logger.LogInformation(result.Message);
        }
    }
}
