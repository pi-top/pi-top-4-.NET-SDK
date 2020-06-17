using Microsoft.Psi;
using Microsoft.Psi.Imaging;

using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace PiTop.Camera.Psi
{
    internal class MatImageStreamWrapper : ImageStreamWrapper<Mat>
    {
        public MatImageStreamWrapper(IProducer<Mat> source, Pipeline pipeline) : base(source, pipeline)
        {
        }

        protected override Shared<Image> ProcessImage(Mat image, Envelope envelope)
        {
            var bitmap = image.ToBitmap();
            var sharedImage = ImagePool.GetOrCreateFromBitmap(bitmap);
            return sharedImage;
        }
    }
}