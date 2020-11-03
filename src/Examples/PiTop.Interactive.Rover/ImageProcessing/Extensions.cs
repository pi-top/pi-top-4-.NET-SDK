using System;
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

        public static Image Focus(this Image source, bool asSquare = true)
        {
            var rect = CreateFocusRectangle(source, asSquare);
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

        public static Image<TPixel> Focus<TPixel>(this Image<TPixel> source, bool asSquare = true) where TPixel : unmanaged, IPixel<TPixel>
        {
            var rect = CreateFocusRectangle(source, asSquare);
            return source.Clone(c => c.Crop(rect));
        }
        private static Rectangle CreateFocusRectangle(Image source, bool asSquare)
        {
            var width = source.Width / 2;
            var height = source.Height / 2;
            if (asSquare)
            {
                width = height = Math.Min(height, width);
            }
            var x = (source.Width - width) / 2;
            var y = (source.Height - height) / 2;
            var rect = new Rectangle(x, y, width, height);
            return rect;
        }
    }
}