using System;
using System.Threading;
using System.Threading.Tasks;

namespace Oonyoen.CommandLineParser.AspNetCore
{
    public record ReadLineOutput(bool Cancelled, string Text);

    public class CancellableConsole : IAsyncDisposable
    {
        private AutoResetEvent gotInput = new(false);
        private AutoResetEvent getInput = new(false);
        private CancellationTokenSource disposalCts = new();
        private TaskCompletionSource threadStopped = new();
        private readonly Thread thread;

        private string input = null;

        public CancellableConsole()
        {
            thread = new Thread(() =>
            {
                try
                {
                    var endOfInput = false;
                    while (!disposalCts.IsCancellationRequested && !endOfInput)
                    {
                        getInput.WaitOne();
                        input = null;
                        input = Console.ReadLine();
                        if (input == null)
                        {
                            endOfInput = true;
                        }
                        gotInput.Set();
                    }
                }
                finally
                {
                    threadStopped.SetResult();
                }
            });
        }

        public void Start()
        {
            thread.IsBackground = true;
            thread.Start();
        }

        public async ValueTask DisposeAsync()
        {
            disposalCts.Cancel();
            gotInput.Set();
            await threadStopped.Task;
        }

        public void Cancel()
        {
            gotInput.Set();
        }

        public bool TryReadLine(out string value)
        {
            getInput.Set();
            gotInput.WaitOne();
            value = input;
            return input != null;
        }
    }
}
