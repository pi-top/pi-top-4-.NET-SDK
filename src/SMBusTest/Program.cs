using PiTop;
using PiTop.MakerArchitecture.Expansion;
using PiTop.MakerArchitecture.Expansion.Rover;
using System;
using System.Reflection;
using PiTop.Camera;
using PiTop.MakerArchitecture.Foundation;
using UnitsNet;

using Pocket;

namespace SMBusTest
{
    class Program
    {

        static void Main(string[] args)
        {
            LogEvents.Subscribe(i => Console.WriteLine(i.ToLogString()),new []
            {
                typeof(PiTop4Board).Assembly,
                typeof(FoundationPlate).Assembly,
                typeof(ExpansionPlate).Assembly,
                typeof(RoverRobot).Assembly,
            });
            

            var js = new LinuxJoystick();
            Console.WriteLine($"Connected to {js.Name}!");
            Console.WriteLine($"It has {js.NumAxes} axes!");


            PiTop4Board.Instance.UseCamera();
            using var rover = new RoverRobot(PiTop4Board.Instance.GetOrCreateExpansionPlate(), PiTop4Board.Instance.GetOrCreateCamera<OpenCvCamera>(0),
                RoverRobotConfiguration.Default);
            var camControl = rover.TiltController;
            var motorControl = rover.MotionComponent as SteeringMotorController;

            rover.AllLightsOn();
            rover.BlinkAllLights();

            while (!Console.KeyAvailable)
            {
                try
                {
                    var e = js.ReadEvent();
                    //Console.WriteLine($"ts={e.timestamp}, v={e.value}, t={e.type}, n={e.number}");
                    if (e.type == 1)
                    {
                        switch (e.number)
                        {
                            case 0:
                                if (e.value > 0)
                                {
                                    rover.AllLightsOn();
                                }
                                else
                                {
                                    rover.AllLightsOff();
                                }
                                break;
                        }
                    }
                    if (e.type == 2) // axis
                    {
                        switch (e.number)
                        {
                            //case 5: // left trigger
                            //    leftMotor.Power = MathHelpers.Interpolate(e.value, 0, 1);
                            //    break;
                            //case 4: // right trigger
                            //    rightMotor.Power = MathHelpers.Interpolate(e.value, 0, 1);
                            //    break;

                            case 0: // steer
                                motorControl.Steering = RotationalSpeed.FromDegreesPerSecond(
                                    e.value.Interpolate(-motorControl.MaxSteering.DegreesPerSecond,
                                        motorControl.MaxSteering.DegreesPerSecond) / 2);
                                //motorControl.SetSpeedAndSteering(
                                //    motorControl.Speed,
                                //    RotationalSpeed.FromDegreesPerSecond(MathHelpers.Interpolate(e.value, -motorControl.MaxSteering.DegreesPerSecond, motorControl.MaxSteering.DegreesPerSecond)));
                                break;
                            case 1: // throttle
                                motorControl.Speed = Speed.FromMetersPerSecond(
                                    e.value.Interpolate(motorControl.MaxSpeed.MetersPerSecond,
                                        -motorControl.MaxSpeed.MetersPerSecond) / 2);
                                //motorControl.SetSpeedAndSteering(
                                //    Speed.FromMetersPerSecond(MathHelpers.Interpolate(e.value, leftMotor.MaxSpeed.MetersPerSecond, -leftMotor.MaxSpeed.MetersPerSecond)),
                                //    motorControl.Steering);
                                break;


                            case 2: // pan
                                camControl.Pan = Angle.FromDegrees(e.value.Interpolate(90, -90));
                                break;
                            case 3: // tilt
                                camControl.Tilt = Angle.FromDegrees(
                                    Math.Min(45, e.value.Interpolate(90, -90)));
                                break;


                        }
                    }
                }
                catch
                {
                    motorControl.Stop();
                    throw;
                }
            }

            Console.ReadKey();
            rover.AllLightsOff();
        }
    }
}
