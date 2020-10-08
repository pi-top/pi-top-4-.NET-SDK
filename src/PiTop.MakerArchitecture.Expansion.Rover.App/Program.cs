using System;
using System.Threading.Tasks;
using PiTop.Camera;
using UnitsNet;

namespace PiTop.MakerArchitecture.Expansion.Rover.App
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Test Rover App");
            PiTop4Board.Instance.UseCamera();
            using var rover = new RoverRobot(PiTop4Board.Instance.GetOrCreateExpansionPlate(), PiTop4Board.Instance.GetOrCreateCamera<OpenCvCamera>(0));


            Console.WriteLine("reset");

            rover.MotionComponent.Stop();
            rover.TiltController.Reset();

            await Task.Delay(1000);
            rover.TiltController.Pan = Angle.FromDegrees(45);
            await Task.Delay(1000);
            rover.TiltController.Pan = Angle.FromDegrees(-45);
            await Task.Delay(1000);
            rover.TiltController.Pan = Angle.FromDegrees(0);


            await Task.Delay(1000);
            rover.TiltController.Tilt = Angle.FromDegrees(45);
            await Task.Delay(1000);
            rover.TiltController.Tilt = Angle.FromDegrees(-45);
            await Task.Delay(1000);
            rover.TiltController.Tilt = Angle.FromDegrees(0);
            Console.WriteLine("reset");

            Console.WriteLine("bye");
        }
    }
}
