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
            using var leftMotor = plate.GetOrCreateEncoderMotor(EncoderMotorPort.M3);
            using var rightMotor = plate.GetOrCreateEncoderMotor(EncoderMotorPort.M2);
            rightMotor.ForwardDirection = ForwardDirection.CounterClockwise;

            //var sign = 1;
            //while (!Console.KeyAvailable)
            //{
            //    motor.Power += .1 * sign;
            //    if (motor.Power == 1) sign = -1;
            //    if (motor.Power == -1) sign = 1;
            //    Thread.Sleep(200);
            //}

            //Console.ReadKey();

            //using var servo = plate.GetOrCreateServoMotor(ServoMotorPort.S2);

            //Console.WriteLine("Setting servo to 0 deg");
            //servo.GoToAngle(Angle.FromDegrees(0));
            //Console.ReadKey();

            //Console.WriteLine("Setting servo to 90 deg");
            //servo.GoToAngle(Angle.FromDegrees(90));
            //Console.ReadKey();

            //Console.WriteLine("Setting servo to -90 deg");
            //servo.GoToAngle(Angle.FromDegrees(-90));
            //Console.ReadKey();

            //Console.WriteLine("Setting servo zeropoint to 20 deg");
            //servo.ZeroPoint = Angle.FromDegrees(20);
            //Console.ReadKey();

            //Console.WriteLine("Setting servo to -40 deg");
            //servo.GoToAngle(Angle.FromDegrees(-40));
            //Console.ReadKey();

            var js = new LinuxJoystick();
            Console.WriteLine($"Connected to {js.Name}!");
            Console.WriteLine($"It has {js.NumAxes} axes!");

            while (!Console.KeyAvailable)
            {
                var e = js.ReadEvent();
                Console.WriteLine($"ts={e.timestamp}, v={e.value}, t={e.type}, n={e.number}");
                if (e.type == 2) // axis
                {
                    switch (e.number)
                    {
                        case 5: // left trigger
                            leftMotor.Power = MathHelpers.Interpolate(e.value, short.MinValue, short.MaxValue, 0, 1);
                            break;
                        case 4: // right trigger
                            rightMotor.Power = MathHelpers.Interpolate(e.value, short.MinValue, short.MaxValue, 0, 1);
                            break;
                    }
                }
            }

            Console.ReadKey();
        }
    }
}
