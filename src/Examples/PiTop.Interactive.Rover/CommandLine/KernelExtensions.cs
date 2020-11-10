using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Diagnostics;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Html;
using Microsoft.DotNet.Interactive;
using Microsoft.DotNet.Interactive.Commands;
using Microsoft.DotNet.Interactive.Events;
using Microsoft.DotNet.Interactive.Formatting;
using Microsoft.DotNet.Interactive.Server;

using static Microsoft.DotNet.Interactive.Formatting.PocketViewTags;
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

        public static T UseDefaultMagicCommands<T>(this T kernel)
    where T : Kernel
        {
            kernel.UseLsMagic()
                  .UseTime();

            return kernel;
        }



        private static T UseTime<T>(this T kernel)
            where T : Kernel
        {
            kernel.AddDirective(time());

            return kernel;

            static Command time()
            {
                return new Command("#!time", "Time the execution of the following code in the submission.")
                {
                    Handler = CommandHandler.Create((KernelInvocationContext context) =>
                    {
                        var timer = new Stopwatch();
                        timer.Start();

                        context.OnComplete(invocationContext =>
                        {
                            var elapsed = timer.Elapsed;

                            invocationContext.Publish(
                                new DisplayedValueProduced(
                                    elapsed,
                                    context.Command,
                                    new[]
                                    {
                                        new FormattedValue(
                                            PlainTextFormatter.MimeType,
                                            $"Wall time: {elapsed.TotalMilliseconds}ms")
                                    }));

                            return Task.CompletedTask;
                        });

                        return Task.CompletedTask;
                    })
                };
            }
        }

        private static T UseLsMagic<T>(this T kernel)
            where T : Kernel
        {
            kernel.AddDirective(lsmagic(kernel));

            kernel.VisitSubkernels(k =>
            {
                k.AddDirective(lsmagic(k));
            });

            Formatter.Register<SupportedDirectives>((directives, writer) =>
            {
                var indentLevel = 1.5;
                PocketView t = div(
                    h3(directives.KernelName + " kernel"),
                    div(directives.Commands.Select(v => div[style: $"text-indent:{indentLevel:##.#}em"](Summary(v, 0)))));

                t.WriteTo(writer, HtmlEncoder.Default);

                IEnumerable<IHtmlContent> Summary(ICommand command, double offset)
                {
                    yield return new HtmlString("<pre>");

                    var level = indentLevel + offset;

                    for (var i = 0; i < command.Aliases.ToArray().Length; i++)
                    {
                        var alias = command.Aliases.ToArray()[i];
                        yield return span[style: $"text-indent:{level:##.#}em; color:#512bd4"](alias);

                        if (i < command.Aliases.Count - 1)
                        {
                            yield return span[style: $"text-indent:{level:##.#}em; color:darkgray"](", ");
                        }
                    }

                    var nextLevel = (indentLevel * 2) + offset;
                    yield return new HtmlString("</pre>");

                    yield return div[style: $"text-indent:{nextLevel:##.#}em"](command.Description);

                    foreach (var subCommand in command.Children.OfType<ICommand>())
                    {
                        yield return div[style: $"text-indent:{nextLevel:##.#}em"](Summary(subCommand, nextLevel));
                    }

                }
            }, "text/html");

            return kernel;
        }

        private static Command lsmagic(Kernel kernel)
        {
            return new Command("#!lsmagic", "List the available magic commands / directives")
            {
                Handler = CommandHandler.Create(async (KernelInvocationContext context) =>
                {
                    var supportedDirectives = new SupportedDirectives(kernel.Name);

                    supportedDirectives.Commands.AddRange(
                        kernel.Directives.Where(d => !d.IsHidden));

                    context.Display(supportedDirectives);

                    await kernel.VisitSubkernelsAsync(async k =>
                    {
                        if (k.Directives.Any(d => d.Name == "#!lsmagic"))
                        {
                            await k.SendAsync(new SubmitCode(((SubmitCode)context.Command).Code));
                        }
                    });
                })
            };
        }
    }
}