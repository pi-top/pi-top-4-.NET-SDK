using PiTop;
using PiTop.MakerArchitecture.Expansion;
using System;
using UnitsNet;

namespace SMBusTest
{
    class Program
    {

        static void Main(string[] args)
        {
            using var module = PiTop4Board.Instance;
            var plate = module.GetOrCreatePlate<ExpansionPlate>();
            using var motor = plate.GetOrCreateEncoderMotor(EncoderMotorPort.M2);

            //var sign = 1;
            //while (!Console.KeyAvailable)
            //{
            //    motor.Power += .1 * sign;
            //    if (motor.Power == 1) sign = -1;
            //    if (motor.Power == -1) sign = 1;
            //    Thread.Sleep(200);
            //}

            //Console.ReadKey();

            using var servo = plate.GetOrCreateServoMotor(ServoMotorPort.S2);

            Console.WriteLine("Setting servo to 0 deg");
            servo.GoToAngle(Angle.FromDegrees(0));
            Console.ReadKey();

            Console.WriteLine("Setting servo to 90 deg");
            servo.GoToAngle(Angle.FromDegrees(90));
            Console.ReadKey();

            Console.WriteLine("Setting servo to -90 deg");
            servo.GoToAngle(Angle.FromDegrees(-90));
            Console.ReadKey();

            Console.WriteLine("Setting servo zeropoint to 20 deg");
            servo.ZeroPoint = Angle.FromDegrees(20);
            Console.ReadKey();

            Console.WriteLine("Setting servo to -40 deg");
            servo.GoToAngle(Angle.FromDegrees(-40));
            Console.ReadKey();

        }
    }
}
