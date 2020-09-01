using System;
using System.IO;

namespace PiTop.Camera.App
{
    class Program
    {
        static void Main(string[] args)
        {
            using var module = PiTop4Board.Instance
                .UseCamera();

            var count = OpenCvCamera.GetCameraCount();

            Console.WriteLine($"Found {count} cameras available");

            var camera = module.GetOrCreateCamera<OpenCvCamera>(0);

            var file = new FileInfo("./test.png");

            var frame = camera.GetFrameAsMat();
              
            frame.SaveImage(file.FullName);

            module.DisposeDevice(camera);

            Console.WriteLine($"Dumping frame at {file.FullName}");
        }
    }
}
