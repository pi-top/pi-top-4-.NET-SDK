using System;
using System.Drawing;

using Microsoft.Psi;

using OpenCvSharp;

namespace PiTop.Camera.Psi
{
    public static class CameraComponents
    {
        public static IProducer<Mat> CreateComponent(this OpenCvCamera camera, Pipeline pipeline, TimeSpan samplingInterval)
        {
            camera.GetFrame(out Mat initialFrame);
            return Generators.Sequence(pipeline, initialFrame, _ =>
            {
                camera.GetFrame(out Mat frame);
                return frame;
            }, samplingInterval);
        }

        public static IProducer<Bitmap> CreateComponent(this ICamera camera, Pipeline pipeline, TimeSpan samplingInterval)
        {
            camera.GetFrame(out var initialFrame);
            return Generators.Sequence(pipeline, initialFrame, _ =>
            {
                camera.GetFrame(out var frame);
                return frame;
            }, samplingInterval);
        }
    }
}