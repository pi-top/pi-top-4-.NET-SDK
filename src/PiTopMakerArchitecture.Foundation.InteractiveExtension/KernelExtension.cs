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
        public Task OnLoadAsync(Kernel kernel)
        {
            Formatter.Register<FoundationPlate>((plate, writer) =>
            {
                var root = plate.ToJObject();
                writer.Write(root.ToString(Formatting.Indented));
            }, JsonFormatter.MimeType);

            Formatter.Register<FoundationPlate>((plate, writer) =>
            {
                var svg = plate.DrawSvg();
                writer.Write(svg);
            }, HtmlFormatter.MimeType);

            Formatter.Register<Led>((device, writer) =>
            {
                var svg = device.DrawSvg();
                writer.Write(svg);
            }, HtmlFormatter.MimeType);

            Formatter.Register<UltrasonicSensor>((device, writer) =>
            {
                var svg = device.DrawSvg();
                writer.Write(svg);
            }, HtmlFormatter.MimeType);

            Formatter.Register<SoundSensor>((device, writer) =>
            {
                var svg = device.DrawSvg();
                writer.Write(svg);
            }, HtmlFormatter.MimeType);

            Formatter.Register<LightSensor>((device, writer) =>
            {
                var svg = device.DrawSvg();
                writer.Write(svg);
            }, HtmlFormatter.MimeType);

            Formatter.Register<Button>((device, writer) =>
            {
                var svg = device.DrawSvg();
                writer.Write(svg);
            }, HtmlFormatter.MimeType);

            Formatter.Register<Potentiometer>((device, writer) =>
            {
                var svg = device.DrawSvg();
                writer.Write(svg);
            }, HtmlFormatter.MimeType);

            Formatter.Register<Buzzer>((device, writer) =>
            {
                var svg = device.DrawSvg();
                writer.Write(svg);
            }, HtmlFormatter.MimeType);

            Formatter.SetPreferredMimeTypeFor(typeof(FoundationPlate), HtmlFormatter.MimeType);
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
