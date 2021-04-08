using System;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;

using Microsoft.DotNet.Interactive;
using Microsoft.DotNet.Interactive.Commands;
using Microsoft.DotNet.Interactive.Formatting;

using OpenCvSharp;

using SixLabors.ImageSharp.Formats.Png;

using static Microsoft.DotNet.Interactive.Formatting.PocketViewTags;

namespace PiTop.Camera.InteractiveExtension
{
    public class KernelExtension : IKernelExtension
    {
        public async Task OnLoadAsync(Kernel kernel)
        {
            Formatter.Register<Mat>((openCvImage, writer) =>
            {
                var id = Guid.NewGuid().ToString("N");
                var data = openCvImage.ImEncode(".png");
                var imgTag = CreateImgTag(data, id, openCvImage.Height, openCvImage.Width);
                writer.Write(imgTag);
            }, HtmlFormatter.MimeType);

            Formatter.Register<System.Drawing.Image>((image, writer) =>
            {
                var id = Guid.NewGuid().ToString("N");
                using var stream = new MemoryStream();
                image.Save(stream, ImageFormat.Png);
                stream.Flush();
                var data = stream.ToArray();
                var imgTag = CreateImgTag(data, id, image.Height, image.Width);
                writer.Write(imgTag);
            }, HtmlFormatter.MimeType);

            Formatter.Register<SixLabors.ImageSharp.Image>((image, writer) =>
            {
                var id = Guid.NewGuid().ToString("N");
                var data = GetImageBytes(image);
                var imgTag = CreateImgTag(data, id, image.Height, image.Width);
                writer.Write(imgTag);
            }, HtmlFormatter.MimeType);

            Formatter.Register<ICamera>((camera, writer) =>
            {
                var image = camera.GetFrame();
                var id = Guid.NewGuid().ToString("N");
                var data = GetImageBytes(image);
                var imgTag = CreateImgTag(data, id, image.Height, image.Width);
                writer.Write(imgTag);
            }, HtmlFormatter.MimeType);


            await kernel.SendAsync(
                new DisplayValue(new FormattedValue(
                    "text/markdown",
                    "Added support for Camera.")));
        }

        private static byte[] GetImageBytes(SixLabors.ImageSharp.Image image)
        {
            using var stream = new MemoryStream();
            image.Save(stream, new PngEncoder());
            stream.Flush();
            var data = stream.ToArray();
            return data;
        }

        private static PocketView CreateImgTag(byte[] data, string id, int height, int width)
        {
            var imageSource = $"data:image/png;base64, {Convert.ToBase64String(data)}";
            PocketView imgTag = img[id: id, src: imageSource, height: height, width: width]();
            return imgTag;
        }
    }
}
