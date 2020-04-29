using System.Threading.Tasks;
using Microsoft.DotNet.Interactive;
using Microsoft.DotNet.Interactive.Formatting;
using Newtonsoft.Json;
using PiTopMakerArchitecture.Foundation.Components;
using PiTopMakerArchitecture.Foundation.Sensors;

namespace PiTopMakerArchitecture.Foundation.InteractiveExtension
{
    public class KernelExtension : IKernelExtension
    {
        public Task OnLoadAsync(IKernel kernel)
        {
            Formatter<Plate>.Register((plate, writer) =>
            {
                var root = plate.ToJObject();
                writer.Write(root.ToString(Formatting.Indented));
            }, JsonFormatter.MimeType);

            Formatter<Plate>.Register((plate, writer) =>
            {
                var svg = plate.DrawSvg();
                writer.Write(svg);
            }, HtmlFormatter.MimeType);

            Formatter<Led>.Register((led, writer) =>
            {
                var svg = led.DrawSvg();
                writer.Write(svg);
            }, HtmlFormatter.MimeType);

            Formatter.SetPreferredMimeTypeFor(typeof(Plate), HtmlFormatter.MimeType);
            Formatter.SetPreferredMimeTypeFor(typeof(Led), HtmlFormatter.MimeType);
            Formatter.SetPreferredMimeTypeFor(typeof(UltrasonicSensor), HtmlFormatter.MimeType);
            Formatter.SetPreferredMimeTypeFor(typeof(SoundSensor), HtmlFormatter.MimeType);
            Formatter.SetPreferredMimeTypeFor(typeof(LightSensor), HtmlFormatter.MimeType);
            Formatter.SetPreferredMimeTypeFor(typeof(Button), HtmlFormatter.MimeType);
            Formatter.SetPreferredMimeTypeFor(typeof(Potentiometer), HtmlFormatter.MimeType);
            Formatter.SetPreferredMimeTypeFor(typeof(Buzzer), HtmlFormatter.MimeType);

            return Task.CompletedTask;
        }

  
    }
}
