using System.IO;
using System.Threading.Tasks;

using Microsoft.DotNet.Interactive;
using Microsoft.DotNet.Interactive.Commands;
using Microsoft.DotNet.Interactive.Formatting;

using SixLabors.ImageSharp;

using static Microsoft.DotNet.Interactive.Formatting.PocketViewTags;

namespace PiTop.InteractiveExtension
{
    public class KernelExtension : IKernelExtension
    {
        public async Task OnLoadAsync(Kernel kernel)
        {
            Formatter.Register<Sh1106Display>((d, w) =>
            {
                using (MemoryStream s = new MemoryStream())
                {
                    d.Capture().SaveAsPng(s);
                    PocketView view = img[src: @"data:image/png;base64, " + System.Convert.ToBase64String(s.ToArray())];
                    w.Write(view);
                }
            }, "text/html");

            await kernel.SendAsync(
                new DisplayValue(new FormattedValue(
                    "text/markdown",
                    "Added support for pi-top[4] module.")));
        }
    }
}