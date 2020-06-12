using System;
using System.Drawing.Imaging;
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

            var frame = camera.GetFrame();
              
            frame.SaveImage(file.FullName);

            Console.WriteLine($"Dumping frame at {file.FullName}");
        }
    }
}
