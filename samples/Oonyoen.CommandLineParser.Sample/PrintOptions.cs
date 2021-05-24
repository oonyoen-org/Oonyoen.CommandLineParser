using CommandLine;

namespace Oonyoen.CommandLineParser.Sample
{
    [Verb("print", HelpText = "Prints a message.")]
    public class PrintOptions
    {
        [Option('m', "message", Required = true, HelpText = "The message you want to print.")]
        public string Message { get; set; }
    }
}
