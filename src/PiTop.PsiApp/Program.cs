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
            using var plate = module.GetOrCreatePlate<FoundationPlate>();

            plate.GetOrCreateAnalogueDevice<Potentiometer>(AnaloguePort.A0)
                .CreateComponent(pipeline, TimeSpan.FromSeconds(0.5))
                .Do(Console.WriteLine);


            pipeline.Run();
        }
    }
}
