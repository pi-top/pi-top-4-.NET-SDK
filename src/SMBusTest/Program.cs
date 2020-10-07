using PiTop;
using PiTop.MakerArchitecture.Expansion;
using System;
using System.Threading;

namespace SMBusTest
{
    class Program
    {

        static void Main(string[] args)
        {
            using var module = PiTop4Board.Instance;
            var plate = module.GetOrCreatePlate<ExpansionPlate>();
            using var motor = plate.GetOrCreateEncoderMotor(EncoderMotorPort.M2);

            var sign = 1;
            while (!Console.KeyAvailable)
            {
                motor.Power += .1 * sign;
                if (motor.Power == 1) sign = -1;
                if (motor.Power == -1) sign = 1;
                Thread.Sleep(200);
            }

            Console.ReadKey();
        }
    }
}
