using System;
using System.Threading;
using System.Threading.Tasks;

using lobe;

using Microsoft.DotNet.Interactive;
using Microsoft.DotNet.Interactive.Commands;
using Microsoft.DotNet.Interactive.CSharp;
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

            await ConfigureImageSharp(csharpKernel);
            await LoadAssemblyAndAddNamespace<Speed>(csharpKernel);
            await ConfigurePiTop(csharpKernel);
            await ConfigureLobe(csharpKernel);
            await ConfigureRover(csharpKernel);
        }

        private static async Task ConfigureRover(CSharpKernel csharpKernel)
        {
            await LoadAssemblyAndAddNamespace<RoverRobot>(csharpKernel);
            await LoadAssemblyAndAddNamespace<ResourceScanner>(csharpKernel);
            await AddNamespace(csharpKernel, typeof(ImageProcessing.ImageExtensions));


            var roverBody = new RoverRobot(PiTop4Board.Instance.GetOrCreateExpansionPlate(),
                PiTop4Board.Instance.GetOrCreateCamera<StreamingCamera>(0),
                RoverRobotConfiguration.Default);
            var roverBrain = new RoverRobotStrategies();

            var resourceScanner = new ResourceScanner();

            roverBody.AllLightsOn();
            roverBody.BlinkAllLights();

            await csharpKernel.SetVariableAsync(nameof(roverBody), roverBody);
            await csharpKernel.SetVariableAsync(nameof(roverBrain), roverBrain);
            await csharpKernel.SetVariableAsync(nameof(resourceScanner), resourceScanner);

            var source = new CancellationTokenSource();

            var robotLoop = Task.Run(() =>
            {
                while (!source.IsCancellationRequested)
                {
                    if (!source.IsCancellationRequested)
                    {
                        roverBrain.Perceive?.Invoke(roverBody, DateTime.Now, source.Token);
                    }

                    if (!source.IsCancellationRequested)
                    {
                        var planResult = roverBrain.Plan?.Invoke(roverBody, DateTime.Now, source.Token) ??
                                         PlanningResult.NoPlan;
                        if (!source.IsCancellationRequested && planResult != PlanningResult.NoPlan)
                        {
                            roverBrain.Act?.Invoke(roverBody, DateTime.Now, source.Token);
                        }
                    }
                }

                roverBody.MotionComponent.Stop();
            }, source.Token);

            var reactLoop = Task.Run(() =>
            {
                while (!source.IsCancellationRequested)
                {
                    if (!source.IsCancellationRequested)
                    {
                        roverBrain.React?.Invoke(roverBody, DateTime.Now, source.Token);
                    }
                }

                roverBody.MotionComponent.Stop();
            }, source.Token);

            csharpKernel.RegisterForDisposal(() =>
            {
                source.Cancel(false);
                Task.WaitAll(new[] {robotLoop, reactLoop}, TimeSpan.FromSeconds(10));
                roverBody.Dispose();
            });
        }

        private static async Task ConfigureLobe(CSharpKernel csharpKernel)
        {
            ImageClassifier.Register("onnx", () => new OnnxImageClassifier());
            await LoadAssemblyAndAddNamespace<ImageClassifier>(csharpKernel);
            await LoadAssemblyAndAddNamespace<OnnxImageClassifier>(csharpKernel);
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

            await AddNamespace(csharpKernel, typeof(Button));
            await AddNamespace(csharpKernel, typeof(Led));
            await AddNamespace(csharpKernel, typeof(IFilter));
            await AddNamespace(csharpKernel, typeof(Display));
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