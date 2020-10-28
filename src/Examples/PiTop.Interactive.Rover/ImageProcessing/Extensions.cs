using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace PiTop.Interactive.Rover.ImageProcessing
{
    public static class ImageExtensions
    {
        public static Image CloneAndResize(this Image source, int width, int height)
        {
            return source.Clone(c => c.Resize(width, height));
        }

        public static Image Preview(this Image source)
        {
            var width = 800;
            var height = (int)(((double)width / source.Width) * source.Height);
            return source.CloneAndResize(width, height);
        }

        public static Image<TPixel> CloneAndResize<TPixel>(this Image<TPixel> source, int width, int height) where TPixel : unmanaged, IPixel<TPixel>
        {
            return source.Clone(c => c.Resize(width, height));
        }

        public static Image<TPixel> Preview<TPixel>(this Image<TPixel> source) where TPixel : unmanaged, IPixel<TPixel>
        {
            var width = 600;
            var height = (int)(((double)width / source.Width) * source.Height);
            return source.CloneAndResize(width, height);
        }
    }
}