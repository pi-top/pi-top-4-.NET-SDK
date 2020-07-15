using System;
using System.Drawing;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace PiTop.Camera
{
    public class OpenCvCamera : 
        ICamera,
        IFrameSource<Mat>
    {
        private readonly int _index;
        private readonly VideoCapture _capture;

        public static int GetCameraCount()
        {
            var cameraCount = 0;
            var camera = new VideoCapture();
            while (true)
            {
                if (!camera.Open(cameraCount++))
                {
                    break;
                }

                camera.Release();
            }
            camera.Dispose();
            return cameraCount;
        }

        public OpenCvCamera(int index)
        {
            _index = index;
            _capture = new VideoCapture();
        }

        public Mat GetFrameAsMat()
        {
            if (_capture.IsOpened())
            {
                var image = new Mat();
                _capture.Read(image);
                return image;
            }

            throw new InvalidOperationException("Camera not initialized");
        }

        public void Dispose() {

            _capture.Release();
            _capture.Dispose();
        }

        public void Connect()
        {
            _capture.Open(_index);
        }

        public Bitmap GetFrame()
        {
            var raw = GetFrameAsMat();
            var frame = raw.ToBitmap();
            return frame;
        }

        Mat IFrameSource<Mat>.GetFrame()
        {
            return GetFrameAsMat();
        }
    }
}