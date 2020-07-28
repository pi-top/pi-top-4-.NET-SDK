using System.Drawing;

using Microsoft.Psi;

using OpenCvSharp;

namespace PiTop.Camera.Psi
{
    public static class ImageConverter
    {
        public static IProducer<Microsoft.Psi.Imaging.Image> ToImage(this IProducer<Bitmap> bitmapStream, Pipeline pipeline)
        {
            return (new BitmapImageStreamWrapper(bitmapStream, pipeline)).Select(e => e.Resource);
        }

        public static IProducer<Microsoft.Psi.Imaging.Image> ToImage(this IProducer<Mat> matStream, Pipeline pipeline)
        {
            return (new MatImageStreamWrapper(matStream, pipeline)).Select(e => e.Resource);
        }

        public static IProducer<Microsoft.Psi.Imaging.Image> ToImage(this IProducer<SixLabors.ImageSharp.Image> imageStream, Pipeline pipeline)
        {
            return (new ImageSharpImageStreamWrapper(imageStream, pipeline)).Select(e => e.Resource);
        }
    }
}