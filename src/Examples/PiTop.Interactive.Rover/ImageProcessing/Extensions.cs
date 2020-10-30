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

        public static Image Focus(this Image source)
        {
            var rect = new Rectangle(source.Width / 2, source.Height / 4, source.Width / 2, source.Height / 2);
            return source.Clone(c => c.Crop(rect));
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

        public static Image<TPixel> Focus<TPixel>(this Image<TPixel> source) where TPixel : unmanaged, IPixel<TPixel>
        {
            var rect = new Rectangle(source.Width/4, source.Height/4, source.Width/2, source.Height/2);
            return source.Clone(c => c.Crop(rect));
        }
    }
}