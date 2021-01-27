using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using lobe;

using Microsoft.DotNet.Interactive;
using Microsoft.DotNet.Interactive.Commands;
using Microsoft.DotNet.Interactive.CSharp;
using Microsoft.DotNet.Interactive.Formatting;

using PiTop.Abstractions;
using PiTop.Algorithms;
using PiTop.Camera;
using PiTop.Interactive.Rover.ML;
using PiTop.MakerArchitecture.Expansion;
using PiTop.MakerArchitecture.Expansion.Rover;
using PiTop.MakerArchitecture.Foundation;
using PiTop.MakerArchitecture.Foundation.Components;
using PiTop.MakerArchitecture.Foundation.Sensors;

using Pocket;

using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;

using UnitsNet;

using static Pocket.Logger;
namespace PiTop.Interactive.Rover
{
    public static class RoverRobotSetup
    {
        public static async Task SetupKernelEnvironment(CSharpKernel csharpKernel)
        {
            using var _ = Log.OnEnterAndExit();

            await ConfigureNamespaces(csharpKernel);
            await ConfigureImageSharp(csharpKernel);

            await ConfigurePiTop(csharpKernel);
            await ConfigureLobe(csharpKernel);
            await ConfigureRover(csharpKernel);

            await ConfigureJoystick(csharpKernel);
        }

        private static async Task ConfigureRover(CSharpKernel csharpKernel)
        {
            Microsoft.DotNet.Interactive.Formatting.Formatter.ListExpansionLimit = 42;
            using var _ = Log.OnEnterAndExit();
            await LoadAssemblyAndAddNamespace<RoverRobot>(csharpKernel);
            await LoadAssemblyAndAddNamespace<ResourceScanner>(csharpKernel);
            await AddNamespace(csharpKernel, typeof(ImageProcessing.ImageExtensions));

            var RoverBody = new RoverRobot(PiTop4Board.Instance.GetOrCreateExpansionPlate(),
                PiTop4Board.Instance.GetOrCreateCamera<StreamingCamera>(0),
                RoverRobotConfiguration.Default);
            var RoverBrain = new RoverRobotAgent();

            var ResourceScanner = new ResourceScanner();

            RoverBody.BlinkAllLights();

            await csharpKernel.SetVariableAsync(nameof(RoverBody), RoverBody);
            await csharpKernel.SetVariableAsync(nameof(RoverBrain), RoverBrain);
            await csharpKernel.SetVariableAsync(nameof(ResourceScanner), ResourceScanner);

            var command = new Command("#!reset", "Reset RoverBody, RoverBrain and BrainState")
            {
new Option<bool>("--body",description:"Resets the rover body"),
new Option<bool>("--brain", description:"Resets the rover brain"),
new Option<bool>("--state", description:"Resets the rover brain state"),
new Option<bool>("--all", description:"Resets the entire rover"),
            };

            command.Handler = CommandHandler.Create<bool, bool, bool, bool, KernelInvocationContext>(async (body, brain, state, all, context) =>
            {
                if (body || brain || state || all)
                {
                    var code = new StringBuilder();
                    var resetTarget = new List<string>();
                    if (brain || all)
                    {
                        code.AppendLine($"{nameof(RoverBrain)}.Reset();");
                        resetTarget.Add("brain");
                    }

                    if (state || all)
                    {
                        code.AppendLine($"{nameof(RoverBrain)}.ClearState();");
                        resetTarget.Add("state");
                    }

                    if (body || all)
                    {
                        code.AppendLine($"{nameof(RoverBody)}.Reset();");
                        resetTarget.Add("body");
                    }

                    var value = context.Display($"Reset for {string.Join(", ", resetTarget)} in progress", PlainTextFormatter.MimeType);

                    await csharpKernel.SendAsync(new SubmitCode(code.ToString()));

                    value.Update($"Reset for {string.Join(", ", resetTarget)} done!");
                }
            });

            csharpKernel.AddDirective(command);

            var source = new CancellationTokenSource();

            var robotLoop = Task.Run(() =>
            {
                using var operation = Log.OnEnterAndExit("roverBrainLoop");
                while (!source.IsCancellationRequested)
                {

                    if (!source.IsCancellationRequested)
                    {
                        using var __ = operation.OnEnterAndExit("Perceive");
                        try
                        {
                            RoverBrain.Perceive();
                        }
                        catch (Exception e)
                        {
                            __.Error(e);
                        }
                    }

                    if (!source.IsCancellationRequested)
                    {
                        var planResult = PlanningResult.NoPlan;
                        using var ___ = operation.OnEnterAndExit("Plan");
                        try
                        {
                            planResult = RoverBrain.Plan();
                        }
                        catch (Exception e)
                        {
                            ___.Error(e);
                            planResult = PlanningResult.NoPlan;
                        }

                        if (!source.IsCancellationRequested && planResult != PlanningResult.NoPlan)
                        {
                            using var ____ = operation.OnEnterAndExit("Act");
                            RoverBrain.Act();
                        }
                    }
                }

                RoverBody.MotionComponent.Stop();
            }, source.Token);

            var reactLoop = Task.Run(() =>
            {
                using var operation = Log.OnEnterAndExit("roverBrainReactLoop");
                while (!source.IsCancellationRequested)
                {
                    if (!source.IsCancellationRequested)
                    {
                        using var __ = operation.OnEnterAndExit("React");
                        try
                        {
                            RoverBrain.React();
                        }
                        catch (Exception e)
                        {
                            __.Error(e);
                        }
                    }
                }

                RoverBody.MotionComponent.Stop();
            }, source.Token);

            csharpKernel.RegisterForDisposal(() =>
            {
                source.Cancel(false);
                Task.WaitAll(new[] { robotLoop, reactLoop }, TimeSpan.FromSeconds(10));
                RoverBody.Dispose();
            });
        }

        private static async Task ConfigureNamespaces(CSharpKernel csharpKernel)
        {
            await AddNamespace(csharpKernel, typeof(Task));
            await AddNamespace(csharpKernel, typeof(Directory));
            await AddNamespace(csharpKernel, typeof(List<>));
            await AddNamespace(csharpKernel, typeof(System.Linq.Enumerable));

        }

        private static async Task ConfigureLobe(CSharpKernel csharpKernel)
        {
            ImageClassifier.Register("onnx", () => new OnnxImageClassifier());

            await LoadAssemblyAndAddNamespace<ImageClassifier>(csharpKernel);
            await LoadAssemblyAndAddNamespace<OnnxImageClassifier>(csharpKernel);
            await AddNamespace(csharpKernel, typeof(ClassificationResults));
        }

        private static async Task ConfigurePiTop(CSharpKernel csharpKernel)
        {
            PiTop4Board.Instance.UseCamera();

            var piTopExtension = new InteractiveExtension.KernelExtension();
            await piTopExtension.OnLoadAsync(csharpKernel);

            var cameraExtension = new Camera.InteractiveExtension.KernelExtension();
            await cameraExtension.OnLoadAsync(csharpKernel);

            var foundationExtension = new MakerArchitecture.Foundation.InteractiveExtension.KernelExtension();
            await foundationExtension.OnLoadAsync(csharpKernel);

            var expansionExtension = new MakerArchitecture.Expansion.InteractiveExtension.KernelExtension();
            await expansionExtension.OnLoadAsync(csharpKernel);

            await LoadAssemblyAndAddNamespace<PiTop4Board>(csharpKernel);
            await LoadAssemblyAndAddNamespace<StreamingCamera>(csharpKernel);
            await LoadAssemblyAndAddNamespace<FoundationPlate>(csharpKernel);
            await LoadAssemblyAndAddNamespace<ExpansionPlate>(csharpKernel);
            await LoadAssemblyAndAddNamespace<Speed>(csharpKernel);

            await AddNamespace(csharpKernel, typeof(Button));
            await AddNamespace(csharpKernel, typeof(Led));
            await AddNamespace(csharpKernel, typeof(IFilter));
            await AddNamespace(csharpKernel, typeof(Display));
        }

        private static async Task ConfigureJoystick(CSharpKernel csharpKernel)
        {
            await LoadAssemblyAndAddNamespace<LinuxJoystick>(csharpKernel);
        }

        private static async Task ConfigureImageSharp(CSharpKernel csharpKernel)
        {
            await LoadAssemblyAndAddNamespace<Image>(csharpKernel);
            await LoadAssemblyAndAddNamespace<Color>(csharpKernel);
            await LoadAssemblyAndAddNamespace<ComplexPolygon>(csharpKernel);
            await LoadAssemblyAndAddNamespace<Font>(csharpKernel);
            await AddNamespace(csharpKernel, typeof(ImageExtensions));

        }

        private static async Task LoadAssemblyAndAddNamespace<T>(CSharpKernel csharpKernel)
        {
            await csharpKernel.SendAsync(new SubmitCode(@$"#r ""{typeof(T).Assembly.Location}""
using {typeof(T).Namespace};"));
        }

        private static async Task AddNamespace(CSharpKernel csharpKernel, Type type)
        {
            await csharpKernel.SendAsync(new SubmitCode(@$"using {type.Namespace};"));
        }
    }
}