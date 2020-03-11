using System.Threading.Tasks;
using Microsoft.DotNet.Interactive;
using Microsoft.DotNet.Interactive.Formatting;
using PiTopMakerArchitecture.Foundation.Components;

namespace PiTopMakerArchitecture.Foundation.InteractiveExtension
{
    public class KernelExtension : IKernelExtension
    {
        public Task OnLoadAsync(IKernel kernel)
        {
            Formatter<Led>.Register((led, writer) =>
            {
                var svg = led.DrawSvg();
                writer.Write(svg);
            }, HtmlFormatter.MimeType);

            Formatter.SetPreferredMimeTypeFor(typeof(Led), HtmlFormatter.MimeType);

            return Task.CompletedTask;
        }
    }
}
