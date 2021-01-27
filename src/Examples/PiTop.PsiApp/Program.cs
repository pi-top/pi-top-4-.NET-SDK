using System;

using Microsoft.Psi;

using PiTop.MakerArchitecture.Foundation;
using PiTop.MakerArchitecture.Foundation.Psi;

using Pocket;

using SixLabors.ImageSharp;

namespace PiTop.PsiApp
{
    class Program
    {
        static void Main(string[] args)
        {
            LogEvents.Subscribe(i =>
            {
                i.Operation.Id = "";
                Console.WriteLine(i.ToLogString());
            }, new[]
            {
                //typeof(PiTop4Board).Assembly,
                //typeof(FoundationPlate).Assembly,
                typeof(Program).Assembly
            });

            using var pipeline = Pipeline.Create("PiTop", DeliveryPolicy.Unlimited);
            using var module = PiTop4Board.Instance;
            var plate = module.GetOrCreateFoundationPlate();

            var threshold = plate
                .GetOrCreatePotentiometer(AnaloguePort.A0)
                .CreateComponent(pipeline, TimeSpan.FromSeconds(0.5));

            var distance = plate
                .GetOrCreateUltrasonicSensor(DigitalPort.D0)
                .CreateComponent(pipeline, TimeSpan.FromSeconds(0.2));

            var alert = new ValueAlertComponent(pipeline,
                new[]
                {
                   plate.GetOrCreateLed(DigitalPort.D1, Color.Green),
                   plate.GetOrCreateLed(DigitalPort.D2, Color.Gold),
                   plate.GetOrCreateLed(DigitalPort.D3, Color.RebeccaPurple)
                });

            threshold
                .Select(t => t * 50)
                .PipeTo(alert.Threshold);

            distance
                .Select(d => Math.Min(d.Value, 50))
                .PipeTo(alert.Value);

            pipeline.Run();
        }
    }
}
