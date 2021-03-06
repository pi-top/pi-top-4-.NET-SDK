using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.DotNet.Interactive;
using Microsoft.DotNet.Interactive.Formatting;


using PiTop.MakerArchitecture.Foundation.Components;
using PiTop.MakerArchitecture.Foundation.Sensors;

namespace PiTop.MakerArchitecture.Foundation.InteractiveExtension
{
    public class KernelExtension : IKernelExtension
    {
        private static JsonSerializerOptions SerializerOptions { get; } = 
            new JsonSerializerOptions(JsonSerializerDefaults.General)
            {
                WriteIndented = false,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                NumberHandling =System.Text.Json.Serialization.JsonNumberHandling.AllowNamedFloatingPointLiterals|System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString
            };

        public Task OnLoadAsync(Kernel kernel)
        {
            Formatter.Register<FoundationPlate>((plate, writer) =>
            {
                
                var root = JsonSerializer.Serialize(plate.ToDictionary(), SerializerOptions);
                writer.Write(root);
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

            KernelInvocationContext.Current?.Display(
                $@"Added support for FoundationPlate.",
                "text/markdown");

            return Task.CompletedTask;
        }


    }
}
