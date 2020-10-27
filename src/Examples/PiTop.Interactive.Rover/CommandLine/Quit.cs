using System;
using System.Threading.Tasks;
using Microsoft.DotNet.Interactive.Commands;

namespace PiTop.Interactive.Rover.CommandLine
{
    public class Quit : KernelCommand
    {
        internal static IDisposable DisposeOnQuit { get; set; }
        public Quit(string targetKernelName = null) : base(targetKernelName)
        {
            Handler = (command, context) =>
            {
                context.Complete(context.Command);
                DisposeOnQuit?.Dispose();
                Environment.Exit(0);
                return Task.CompletedTask;
            };
        }
    }
}