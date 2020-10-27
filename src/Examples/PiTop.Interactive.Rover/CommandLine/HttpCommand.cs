using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace PiTop.Interactive.Rover.CommandLine
{
    public static class HttpCommand
    {
        public static Task<int> Do(
            StartupOptions startupOptions,
            IConsole console,
            CommandLineParser.StartServer startServer = null,
            InvocationContext context = null)
        {
            startServer?.Invoke(startupOptions, context);

            return Task.FromResult(0);
        }
    }
}