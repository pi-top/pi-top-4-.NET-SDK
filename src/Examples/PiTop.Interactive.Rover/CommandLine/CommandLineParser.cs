// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.


using System;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.CommandLine.IO;
using System.CommandLine.Parsing;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Html;
using Microsoft.DotNet.Interactive;
using Microsoft.DotNet.Interactive.Connection;
using Microsoft.DotNet.Interactive.CSharp;
using Microsoft.DotNet.Interactive.Formatting;
using Microsoft.DotNet.Interactive.Http;
using Microsoft.DotNet.Interactive.Server;
using Microsoft.Extensions.DependencyInjection;

using Pocket;

using static Pocket.Logger;

using CommandHandler = System.CommandLine.Invocation.CommandHandler;
using Formatter = Microsoft.DotNet.Interactive.Formatting.Formatter;

namespace PiTop.Interactive.Rover.CommandLine
{
    public static class CommandLineParser
    {
        public delegate void StartServer(
            StartupOptions options,
            InvocationContext context);

        public delegate Task StartHttp(
            StartupOptions options,
            IConsole console,
            StartServer startServer = null,
            InvocationContext context = null);

        public static Parser Create(
            IServiceCollection services,
            StartServer startServer = null,
            StartHttp startHttp = null,
            Action onServerStarted = null)
        {
            var operation = Log.OnEnterAndExit();

            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            var disposeOnQuit = new CompositeDisposable();

            startServer ??= (startupOptions, invocationContext) =>
            {
                operation.Info("constructing webHost");
                var webHost = Program.ConstructWebHost(startupOptions);
                disposeOnQuit.Add(webHost);
                operation.Info("starting  kestrel server");
                webHost.Start();
                onServerStarted?.Invoke();
                webHost.WaitForShutdown();
                operation.Dispose();
            };

            startHttp ??= HttpCommand.Do;



            var rootCommand = HttpServer();

            rootCommand.AddCommand(HttpServer());

            return new CommandLineBuilder(rootCommand)
                   .UseDefaults()
                   .Build();

            Command HttpServer()
            {

                var verboseOption = new Option<bool>(
                    "--verbose",
                    "Enable verbose logging to the console");

                var logPathOption = new Option<DirectoryInfo>(
                    "--log-path",
                    "Enable file logging to the specified directory");

                var httpPortOption = new Option<HttpPort>(
                    "--http-port",
                    description: "Specifies the port on which to enable HTTP services",
                    parseArgument: result =>
                    {
                        if (result.Tokens.Count == 0)
                        {
                            return HttpPort.Auto;
                        }

                        var source = result.Tokens[0].Value;

                        if (source == "*")
                        {
                            return HttpPort.Auto;
                        }

                        if (!int.TryParse(source, out var portNumber))
                        {
                            result.ErrorMessage = "Must specify a port number or *.";
                            return null;
                        }

                        return new HttpPort(portNumber);
                    },
                    isDefault: true);

                var workingDirOption = new Option<DirectoryInfo>(
                    "--working-dir",
                    () => new DirectoryInfo(Environment.CurrentDirectory),
                    "Working directory to which to change after launching the kernel.");


                var httpCommand = new RootCommand("Starts rover and exposes http endpoint")
                {
                    httpPortOption, workingDirOption, logPathOption, verboseOption
                };


                httpCommand.Handler = CommandHandler.Create<StartupOptions, IConsole, InvocationContext, CancellationToken>(
                    (startupOptions, console, context, cancellationToken) =>
                    {
                        var frontendEnvironment = new BrowserFrontendEnvironment();
                        var kernel = CreateKernel(frontendEnvironment, startupOptions);
                        kernel.UseQuitCommand(disposeOnQuit, cancellationToken);
                        services.AddKernel(kernel);

                        var kernelServer = kernel.CreateKernelServer(startupOptions.WorkingDir);
                        kernelServer.Start();
                        
                        onServerStarted ??= () =>
                        {
                            kernelServer.NotifyIsReady();
                        };

                        return startHttp(startupOptions, console, startServer, context);
                    });

                return httpCommand;
            }

        }

        private static KernelServer CreateKernelServer(this Kernel kernel, DirectoryInfo workingDir)
        {
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;
            return kernel.CreateKernelServer(Console.In, Console.Out, workingDir);
        }

        public static KernelServer CreateKernelServer(this Kernel kernel, TextReader inputStream, TextWriter outputStream, DirectoryInfo workingDir)
        {
            if (kernel == null)
            {
                throw new ArgumentNullException(nameof(kernel));
            }

            var input = new TextReaderInputStream(inputStream);
            var output = new TextWriterOutputStream(outputStream);
            var kernelServer = new KernelServer(kernel, input, output, workingDir);

            kernel.RegisterForDisposal(kernelServer);
            return kernelServer;
        }


        private static CompositeKernel CreateKernel(
            FrontendEnvironment frontendEnvironment,
            StartupOptions startupOptions)
        {
            using var _ = Log.OnEnterAndExit();

            var compositeKernel = new CompositeKernel
            {
                FrontendEnvironment = frontendEnvironment
            };

            var csharpKernel = new CSharpKernel()
                .UseDefaultMagicCommands()
                .UseNugetDirective()
                .UseKernelHelpers()
                .UseWho()
                .UseDotNetVariableSharing();

            compositeKernel.Add(
                csharpKernel,
                new[] { "c#", "C#" });

            var kernel = compositeKernel
                         .UseLogMagicCommand()
                         .UseKernelClientConnection(new ConnectNamedPipe())
                         .UseKernelClientConnection(new ConnectSignalR());

            if (startupOptions.Verbose)
            {
                kernel.LogEventsToPocketLogger();
            }

            SetUpFormatters();

            kernel.DefaultKernelName = csharpKernel.Name;
            RoverRobotSetup.SetupKernelEnvironment(csharpKernel).Wait();
            return kernel;
        }

        public static void SetUpFormatters()
        {
            Formatter.DefaultMimeType = HtmlFormatter.MimeType;
            Formatter.SetPreferredMimeTypeFor(typeof(string), PlainTextFormatter.MimeType);
            Formatter.SetPreferredMimeTypeFor(typeof(ScriptContent), HtmlFormatter.MimeType);

            Formatter.Register<ScriptContent>((script, writer) =>
            {
                IHtmlContent content =
                    PocketViewTags.script[type: "text/javascript"](script.ScriptValue.ToHtmlContent());
                content.WriteTo(writer, HtmlEncoder.Default);
            }, HtmlFormatter.MimeType);

        }
    }
}