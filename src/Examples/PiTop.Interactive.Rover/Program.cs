using System;
using System.CommandLine.Parsing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Hosting;
using Microsoft.DotNet.Interactive;
using Microsoft.DotNet.Interactive.Http;
using Microsoft.Extensions.DependencyInjection;
using PiTop.Camera;
using PiTop.Interactive.Rover.CommandLine;
using PiTop.MakerArchitecture.Expansion;
using PiTop.MakerArchitecture.Expansion.Rover;
using PiTop.MakerArchitecture.Foundation;
using Pocket;
using Serilog.Sinks.RollingFileAlternate;
using SerilogLoggerConfiguration = Serilog.LoggerConfiguration;
using static Pocket.Logger<PiTop.Interactive.Rover.Program>;

namespace PiTop.Interactive.Rover
{
    public class Program
    {
        private static readonly ServiceCollection ServiceCollection = new ServiceCollection();

        public static async Task<int> Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            return await CommandLineParser.Create(ServiceCollection).InvokeAsync(args);
        }

        private static readonly Assembly[] AssembliesEmittingPocketLoggerLogs =
        {
            typeof(Startup).Assembly, // dotnet-interactive.dll
            typeof(Kernel).Assembly, // Microsoft.DotNet.Interactive.dll
            typeof(KernelHub).Assembly, // Microsoft.DotNet.Interactive.Http.dll
            typeof(PiTop4Board).Assembly,
            typeof(FoundationPlate).Assembly,
            typeof(ExpansionPlate).Assembly,
            typeof(RoverRobot).Assembly,
            typeof(StreamingCamera).Assembly,
            typeof(Program).Assembly,
        };

        internal static IDisposable StartToolLogging(StartupOptions options)
        {
            var disposables = new CompositeDisposable();

            if (options.LogPath != null)
            {
                var log = new SerilogLoggerConfiguration()
                          .WriteTo
                          .RollingFileAlternate(options.LogPath.FullName, outputTemplate: "{Message}{NewLine}")
                          .CreateLogger();

                var subscription = LogEvents.Subscribe(
                    e =>
                    {
                        e.Operation.Id = "";
                        log.Information(e.ToLogString());
                    },
                    AssembliesEmittingPocketLoggerLogs);

                disposables.Add(subscription);
                disposables.Add(log);
            }

            TaskScheduler.UnobservedTaskException += (sender, args) =>
            {
                Log.Warning($"{nameof(TaskScheduler.UnobservedTaskException)}", args.Exception);
                args.SetObserved();
            };

            return disposables;
        }

        public static IWebHostBuilder ConstructWebHostBuilder(
            StartupOptions options,
            IServiceCollection serviceCollection)
        {
            using var _ = Log.OnEnterAndExit();

            // FIX: (ConstructWebHostBuilder) dispose me
            var disposables = new CompositeDisposable
            {
                StartToolLogging(options)
            };

            var httpPort = GetFreePort(options);
            options.HttpPort = httpPort;
            var probingSettings = HttpProbingSettings.Create(options.HttpPort.PortNumber);
          

            var webHost = new WebHostBuilder()
                          .UseKestrel()
                          .UseDotNetInteractiveHttpApi(true, httpPort, probingSettings, serviceCollection)
                          .UseUrls(probingSettings.AddressList.Select(a => a.AbsoluteUri).ToArray())
                          .UseStartup<Startup>();


            return webHost;

            static HttpPort GetFreePort(StartupOptions startupOptions)
            {
                using var __ = Log.OnEnterAndExit(nameof(GetFreePort));
                if (startupOptions.HttpPort != null && !startupOptions.HttpPort.IsAuto)
                {
                    return startupOptions.HttpPort;
                }

                try
                {
                    var l = new TcpListener(IPAddress.Loopback, 5001);
                    l.Start();
                    var port = ((IPEndPoint)l.LocalEndpoint).Port;
                    l.Stop();
                    return new HttpPort(port);
                }
                catch (SocketException)
                {

                }


                throw new InvalidOperationException("Cannot find a port");
            }
        }

        public static IWebHost ConstructWebHost(StartupOptions options)
        {
            var webHost = ConstructWebHostBuilder(options, ServiceCollection)
                          .Build();

            return webHost;
        }
    }
}
