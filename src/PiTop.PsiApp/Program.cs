using System;
using System.Linq;

using Microsoft.Psi;

using PiTopMakerArchitecture.Foundation;
using PiTopMakerArchitecture.Foundation.Components;
using PiTopMakerArchitecture.Foundation.PSI;
using PiTopMakerArchitecture.Foundation.Sensors;

namespace PiTop.PsiApp
{
    class Program
    {
        static void Main(string[] args)
        {
            using var pipeline = Pipeline.Create("PiTop", true);
            using var module = new PiTopModule();
            var plate = module.GetOrCreatePlate<FoundationPlate>();

            var threshold = plate
                .GetOrCreateAnalogueDevice<Potentiometer>(AnaloguePort.A0)
                .CreateComponent(pipeline, TimeSpan.FromSeconds(0.5));

            var distance = plate
                .GetOrCreateDigitalDevice<UltrasonicSensor>(DigitalPort.D3)
                .CreateComponent(pipeline, TimeSpan.FromSeconds(0.5));

            var alert = new DistanceAlertComponent(pipeline,
                new[]
                {
                   plate.GetOrCreateDigitalDevice<Led>(DigitalPort.D0),
                   plate.GetOrCreateDigitalDevice<Led>(DigitalPort.D1),
                   plate.GetOrCreateDigitalDevice<Led>(DigitalPort.D2)
                });

            threshold.Select(t => t * 50).PipeTo(alert.Threshold);
            distance.PipeTo(alert.Distance);

            pipeline.Run();
        }
    }
}
