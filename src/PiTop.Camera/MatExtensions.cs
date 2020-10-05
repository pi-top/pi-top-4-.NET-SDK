using System;

using OpenCvSharp;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;


namespace PiTop.Camera
{
    public static class MatExtensions
    {
        public static unsafe Image ToImage(this Mat src)
        {
            if (src == null)
            {
                throw new ArgumentNullException(nameof(src));
            }

            var imageSize = (int)(src.DataEnd.ToInt64() - src.Data.ToInt64());

            var data = new Span<byte>(src.Data.ToPointer(), imageSize);
            Image image = src.Channels() switch
            {
                1 => Image.LoadPixelData<L8>(data, src.Width, src.Height),
                3 => Image.LoadPixelData<Rgb24>(data, src.Width, src.Height),
                4 => Image.LoadPixelData<Argb32>(data, src.Width, src.Height),
                _ => throw new ArgumentException("Number of channels must be 1, 3 or 4.", nameof(src))
            };

            return image;
        }


        public static Image<TPixel> ToImage<TPixel>(this Mat src) where TPixel : unmanaged, IPixel<TPixel>
        {
            return src.ToImage().CloneAs<TPixel>();
        }

    }

}
