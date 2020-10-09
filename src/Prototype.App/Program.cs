using PiTop;
using PiTop.MakerArchitecture.Expansion;
using PiTop.MakerArchitecture.Expansion.Rover;
using PiTop.MakerArchitecture.Foundation;
using Pocket;
using System;
using System.Reactive.Linq;
using UnitsNet;

namespace Prototype.App
{
    class Program
    {
        static void Main(string[] args)
        {
            LogEvents.Subscribe(i => Console.WriteLine(i.ToLogString()), new[]
            {
                typeof(PiTop4Board).Assembly,
                typeof(FoundationPlate).Assembly,
                typeof(ExpansionPlate).Assembly,
                typeof(RoverRobot).Assembly,
            });

            var js = new XboxController();

            //PiTop4Board.Instance.UseCamera();
            //using var rover = new RoverRobot(PiTop4Board.Instance.GetOrCreateExpansionPlate(), null);//, PiTop4Board.Instance.GetOrCreateCamera<OpenCvCamera>(0));
            //var camControl = rover.TiltController;
            //var motorControl = rover.MotionComponent as SteeringMotorController;

            var leftMotor = PiTop4Board.Instance.GetOrCreateExpansionPlate().GetOrCreateEncoderMotor(EncoderMotorPort.M4);
            var rightMotor = PiTop4Board.Instance.GetOrCreateExpansionPlate().GetOrCreateEncoderMotor(EncoderMotorPort.M1);
            leftMotor.ForwardDirection = ForwardDirection.Clockwise;
            rightMotor.ForwardDirection = ForwardDirection.CounterClockwise;

            var panServo = PiTop4Board.Instance.GetOrCreateExpansionPlate().GetOrCreateServoMotor(ServoMotorPort.S1);
            var tiltServo = PiTop4Board.Instance.GetOrCreateExpansionPlate().GetOrCreateServoMotor(ServoMotorPort.S2);

            //rover.AllLightsOn();
            //rover.BlinkAllLights();

            Observable.Interval(TimeSpan.FromMilliseconds(10))
                .Select(_ => (X: js.LeftStick.X, Y: js.LeftStick.Y))
                .DistinctUntilChanged()
                .Subscribe(stick =>
            {
                leftMotor.Power = (stick.Y - stick.X) / 2;
                rightMotor.Power = (stick.Y + stick.X) / 2;
            });

            js.Events.OfType<ButtonEvent>().Where(e => e.Button == Button.A)
                .Subscribe(e =>
                {
                    if (e.Pressed)
                    {
                        //rover.AllLightsOn();
                    }
                    else
                    {
                        //rover.AllLightsOff();
                    }
                });


            Observable.Interval(TimeSpan.FromMilliseconds(100))
                .Select(_ => (X: js.RightStick.X, Y: js.RightStick.Y))
                .DistinctUntilChanged()
                .Subscribe(stick =>
                {
                    panServo.Speed = RotationalSpeed.FromRadiansPerSecond(stick.X / 3);
                    tiltServo.Speed = RotationalSpeed.FromRadiansPerSecond(stick.Y / 3);
                });

            js.Events.OfType<ButtonEvent>().Where(e => e.Button == Button.RightStick)
                .Subscribe(e =>
                {
                    panServo.GoToAngle(Angle.Zero);
                    tiltServo.GoToAngle(Angle.Zero);
                });

            Console.WriteLine("Ok, go drive around");
            Console.ReadKey();
            //rover.AllLightsOff();
        }
    }
}
