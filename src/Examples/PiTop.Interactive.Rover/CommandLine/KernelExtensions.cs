using System;
using System.Threading;
using Microsoft.DotNet.Interactive;
using Microsoft.DotNet.Interactive.Server;

namespace PiTop.Interactive.Rover.CommandLine
{
    internal static class KernelExtensions
    {
        public static T UseQuitCommand<T>(this T kernel, IDisposable disposeOnQuit, CancellationToken cancellationToken)
            where T : Kernel
        {
            Quit.DisposeOnQuit = disposeOnQuit;
            KernelCommandEnvelope.RegisterCommandType<Quit>(nameof(Quit));
            cancellationToken.Register(async () => { await kernel.SendAsync(new Quit()); });
            return kernel;
        }
    }
}