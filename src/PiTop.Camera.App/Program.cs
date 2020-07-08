using System;
using System.IO;
using OpenCvSharp;

namespace PiTop.Camera.App
{
    class Program
    {
        static void Main(string[] args)
        {
            using var module = new PiTopModule()
                .UseCamera();

            var count = OpenCvCamera.GetCameraCount();

            Console.WriteLine($"Found {count} cameras available");

            var camera = module.GetOrCreateCamera<OpenCvCamera>(0);

            var file = new FileInfo("./test.png");

            camera.GetFrame(out Mat frame);
              
            frame.SaveImage(file.FullName);

            module.DisposeDevice(camera);

            Console.WriteLine($"Dumping frame at {file.FullName}");
        }
    }
}
