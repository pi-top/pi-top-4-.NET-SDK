using System;
using Microsoft.Psi;
using PiTopMakerArchitecture.Foundation;
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

            var position = plate
                .GetOrCreateAnalogueDevice<Potentiometer>(AnaloguePort.A0)
                .CreateComponent(pipeline, TimeSpan.FromSeconds(0.5));

            var speed = position
                .Delta(deliveryPolicy: DeliveryPolicy.LatencyConstrained(TimeSpan.FromSeconds(0.01)));

            position
                .Join(speed)
                .Do((p) => Console.WriteLine($"current {p.Item1} changing by {p.Item2}"));
            
            pipeline.Run();
        }
    }
}
