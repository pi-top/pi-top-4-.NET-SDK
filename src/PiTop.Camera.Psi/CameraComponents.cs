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
            var initialFrame =  camera.GetFrameAsMat();
            return Generators.Sequence(pipeline, initialFrame, _ =>
            {
                var frame = camera.GetFrameAsMat();
                return frame;
            }, samplingInterval);
        }

        public static IProducer<Bitmap> CreateComponent(this ICamera camera, Pipeline pipeline, TimeSpan samplingInterval)
        {
            var initialFrame = camera.GetFrame();
            return Generators.Sequence(pipeline, initialFrame, _ =>
            {
                var frame = camera.GetFrame();
                return frame;
            }, samplingInterval);
        }
    }
}