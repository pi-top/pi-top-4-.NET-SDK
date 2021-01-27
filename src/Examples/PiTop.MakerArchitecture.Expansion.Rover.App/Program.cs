using System;
using PiTop.Algorithms;
using PiTop.Camera;
using PiTop.MakerArchitecture.Foundation;
using Pocket;
using UnitsNet;

namespace PiTop.MakerArchitecture.Expansion.Rover.App
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
            Console.WriteLine("Test Rover App");
            PiTop4Board.Instance.UseCamera();

            using var rover = new RoverRobot(
                PiTop4Board.Instance.GetOrCreateExpansionPlate(),
                PiTop4Board.Instance.GetOrCreateCamera<StreamingCamera>(0), 
                RoverRobotConfiguration.Default);

            var camControl = rover.TiltController;
            var motorControl = rover.MotionComponent as SteeringMotorController;
            var js = new LinuxJoystick();
            rover.AllLightsOn();
            rover.BlinkAllLights();

            Console.WriteLine("reset");

            while (!Console.KeyAvailable)
            {
                try
                {
                    var e = js.ReadEvent();

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

                            case 0: // steer
                                motorControl.Steering = RotationalSpeed.FromDegreesPerSecond(
                                    e.value.Interpolate(-motorControl.MaxSteering.DegreesPerSecond,
                                        motorControl.MaxSteering.DegreesPerSecond) / 2);

                                break;
                            case 1: // throttle
                                motorControl.Speed = Speed.FromMetersPerSecond(
                                    e.value.Interpolate(motorControl.MaxSpeed.MetersPerSecond,
                                        -motorControl.MaxSpeed.MetersPerSecond) / 2);

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

            Console.WriteLine("bye");
        }
    }
}
