using PiTop;
using PiTop.MakerArchitecture.Expansion;
using System;

namespace SMBusTest
{
    class Program
    {

        static void Main(string[] args)
        {
            using var module = PiTop4Board.Instance;
            var plate = module.GetOrCreatePlate<ExpansionPlate>();
            using var motor = plate.GetOrCreateEncoderMotor(EncoderMotorPort.M2);

            // set power
            motor.Power = 0.5;

            Console.ReadKey();
        }
    }
}
