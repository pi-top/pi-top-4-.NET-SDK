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

        public static PocketView CreatePath(string pathData, string style = null)
        {
            var path = _.path[d: pathData]();

            if (!string.IsNullOrWhiteSpace(style))
            {
                path["style"](style);
            }

            return path;
        }
    }

    public static class DeviceExtensions
    {
       
        internal static IHtmlContent DrawSvg(this Led led)
        {
            var id = "PiTopMakerArchitecture.Foundation.InteractiveExtension" + Guid.NewGuid().ToString("N");
            return div[id: id](
                svg(
                    g(
                        circle[cx: 20, cy: 20, r: 15, fill: (led.IsOn ? "white" : "black"), stroke: "black"],
                        text[x: 9, y: 50, @class: "text"]("LED")
                    )));
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
                    return ultrasonicSensor.Distance;
                default:
                    throw new ArgumentOutOfRangeException(nameof(digitalDevice));
            }
        }
    }
}