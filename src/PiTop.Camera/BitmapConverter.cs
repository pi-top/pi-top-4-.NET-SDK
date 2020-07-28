using System;

using OpenCvSharp;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;


namespace PiTop.Camera
{
    public static class BitmapConverter
    {
        public static unsafe Image ToImage(this Mat src)
        {
            if (src == null)
            {
                throw new ArgumentNullException(nameof(src));
            }

            Image image;
            var imageSize = (int)(src.DataEnd.ToInt64() - src.Data.ToInt64());

            var data = new Span<byte>(src.Data.ToPointer(), imageSize);
            switch (src.Channels())
            {
                case 1:
                    image = Image.LoadPixelData<L8>(data, src.Width, src.Height);
                    break;
                case 3:
                    image = Image.LoadPixelData<Rgb24>(data, src.Width, src.Height);
                    break;
                case 4:
                    image = Image.LoadPixelData<Argb32>(data, src.Width, src.Height);
                    break;
                default:
                    throw new ArgumentException("Number of channels must be 1, 3 or 4.", nameof(src));
            }

            return image;
        }


        public static unsafe Image<TPixel> ToImage<TPixel>(this Mat src) where TPixel : unmanaged, IPixel<TPixel>
        {
            return src.ToImage().CloneAs<TPixel>();
        }

    }

}
