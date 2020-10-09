using PiTop;
using PiTop.Camera;
using PiTop.MakerArchitecture.Expansion;
using PiTop.MakerArchitecture.Expansion.Rover;
using PiTop.MakerArchitecture.Foundation;
using Pocket;
using SixLabors.ImageSharp;
using System;
using System.Reactive.Linq;
using UnitsNet;

using static Pocket.Logger;

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
                typeof(StreamingCamera).Assembly,
                typeof(Program).Assembly,
            });

            var js = new XboxController();

            // using ` mjpg_streamer -i "input_uvc.so -d /dev/video0" -o output_http.so`
            // ==> http://pi-top.local:8080/?action=stream
            PiTop4Board.Instance.UseCamera();
            using var rover = new RoverRobot(PiTop4Board.Instance.GetOrCreateExpansionPlate(), PiTop4Board.Instance.GetOrCreateCamera<StreamingCamera>(0),
                RoverRobotConfiguration.Default);
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
                        rover.AllLightsOn();
                    }
                    else
                    {
                        rover.AllLightsOff();
                    }
                });

            js.Events.OfType<ButtonEvent>().Where(e => e.Button == Button.X && e.Pressed)
                .Subscribe(e =>
                {
                    using var operation = Log.OnEnterAndConfirmOnExit();
                    try
                    {
                        rover.Camera.GetFrame().Save("/home/pi/shot.jpg");
                        operation.Succeed();
                    }
                    catch (Exception ex)
                    {
                        operation.Error(ex);
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
