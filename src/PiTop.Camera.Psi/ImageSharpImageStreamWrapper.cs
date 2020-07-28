using System.Drawing;
using System.IO;
using Microsoft.Psi;
using Microsoft.Psi.Imaging;
using SixLabors.ImageSharp;
using Image = Microsoft.Psi.Imaging.Image;

namespace PiTop.Camera.Psi
{
    internal class ImageSharpImageStreamWrapper :  ImageStreamWrapper<SixLabors.ImageSharp.Image>
    {
        public ImageSharpImageStreamWrapper(IProducer<SixLabors.ImageSharp.Image> source, Pipeline pipeline) : base(source, pipeline)
        {
        }

        protected override Shared<Image> ProcessImage(SixLabors.ImageSharp.Image image, Envelope envelope)
        {
            using var stream = new MemoryStream();
            image.SaveAsBmp(stream);
            stream.Flush();
            stream.Position = 0;
            var bitmap = new Bitmap(stream);
            var sharedImage = ImagePool.GetOrCreateFromBitmap(bitmap);
            return sharedImage;
        }
    }
}