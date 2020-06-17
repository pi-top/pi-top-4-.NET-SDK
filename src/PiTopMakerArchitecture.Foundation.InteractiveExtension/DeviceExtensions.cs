using System;
using Microsoft.AspNetCore.Html;
using Microsoft.DotNet.Interactive.Formatting;
using PiTopMakerArchitecture.Foundation.Components;
using PiTopMakerArchitecture.Foundation.Sensors;
using static Microsoft.DotNet.Interactive.Formatting.PocketViewTags;

namespace PiTopMakerArchitecture.Foundation.InteractiveExtension
{
    internal static class SvgUtilities
    {
        internal const string TextStyle = "font-family:'ArialMT', 'Arial', sans-serif;font-size:8px;";

        public static PocketView CreatePath(string pathData, string? style = null)
        {
            var path = _.path[d: pathData, style: style?? ""]();
            return path;
        }
    }

    public static class DeviceExtensions
    {
       
        internal static IHtmlContent DrawSvg(this DigitalPortDeviceBase digitalDevice)
        {
            var id = "PiTopMakerArchitecture.Foundation.InteractiveExtension" + Guid.NewGuid().ToString("N");
            return div[id: id](svg(digitalDevice.GetSvg()));
        }

        internal static IHtmlContent DrawSvg(this AnaloguePortDeviceBase analogueDevice)
        {
            var id = "PiTopMakerArchitecture.Foundation.InteractiveExtension" + Guid.NewGuid().ToString("N");
            return div[id: id](svg(analogueDevice.GetSvg()));
        }

        public static object GetDeviceValue(this DigitalPortDeviceBase digitalDevice)
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
                    return ultrasonicSensor.Distance.Value;
                default:
                    throw new ArgumentOutOfRangeException(nameof(digitalDevice));
            }
        }
    }
}