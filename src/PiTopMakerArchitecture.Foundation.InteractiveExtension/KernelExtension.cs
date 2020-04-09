using System;
using System.Threading.Tasks;
using Microsoft.DotNet.Interactive;
using Microsoft.DotNet.Interactive.Formatting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PiTopMakerArchitecture.Foundation.Components;
using PiTopMakerArchitecture.Foundation.Sensors;

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

            Formatter<Plate>.Register((plate, writer) =>
            {
                var root = new JObject();

                foreach (var digital in plate.DigitalDevices)
                {
                    root[digital.port.ToString()] = new JObject
                    {
                        {"type", digital.device.GetType().Name },
                        {"value", JToken.FromObject( GetDeviceValue(digital.device) )},
                    };
                }

                foreach (var analogue in plate.AnalogueDevices)
                {
                    root[analogue.port.ToString()] = new JObject
                    {
                        {"type", analogue.device.GetType().Name },
                        {"value", JToken.FromObject( GetDeviceValue(analogue.device) )},
                    };
                }

                writer.Write(root.ToString(Formatting.Indented));
            }, JsonFormatter.MimeType);

            return Task.CompletedTask;
        }

        private object GetDeviceValue(AnaloguePortDeviceBase analogueDevice)
        {
            switch (analogueDevice)
            {
                case LightSensor lightSensor:
                    return lightSensor.Value;
                case Potentiometer potentiometer:
                    return potentiometer.Position;
                case SoundSensor soundSensor:
                    return soundSensor.Value;
                default:
                    throw new ArgumentOutOfRangeException(nameof(analogueDevice));
            }
        }


        private object GetDeviceValue(DigitalPortDeviceBase digitalDevice)
        {
            switch (digitalDevice)
            {
                case Buzzer buzzer:
                    return buzzer.IsOn ? 1 : 0;
                case Led led:
                    return led.IsOn ? 1 : 0;
                case Button button:
                    return button.IsPressed ? 1 : 0;
                case UltrasonicSensor ultrasonicSensor:
                    return ultrasonicSensor.Distance;
                default:
                    throw new ArgumentOutOfRangeException(nameof(digitalDevice));
            }
        }
    }
}
