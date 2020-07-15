using System;
using Microsoft.Psi;

namespace PiTop.Camera.Psi
{
    public static class CameraComponents
    {
        public static IProducer<T> CreateComponent<T>(this IFrameSource<T> frameSource, Pipeline pipeline, TimeSpan samplingInterval)
        {
            var initialFrame =  frameSource.GetFrame();
            return Generators.Sequence(pipeline, initialFrame, _ =>
            {
                var frame = frameSource.GetFrame();
                return frame;
            }, samplingInterval);
        }
    }
}