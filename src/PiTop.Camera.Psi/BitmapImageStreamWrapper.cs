using System.Drawing;
using Microsoft.Psi;
using Microsoft.Psi.Imaging;
using Image = Microsoft.Psi.Imaging.Image;

namespace PiTop.Camera.Psi
{
    internal class BitmapImageStreamWrapper: ImageStreamWrapper<Bitmap>
    {
        public BitmapImageStreamWrapper(IProducer<Bitmap> source, Pipeline pipeline) : base(source, pipeline)
        {
        }

        protected override Shared<Image> ProcessImage(Bitmap image, Envelope envelope)
        {
            var sharedImage = ImagePool.GetOrCreateFromBitmap(image);
            return sharedImage;
        }
    }
}