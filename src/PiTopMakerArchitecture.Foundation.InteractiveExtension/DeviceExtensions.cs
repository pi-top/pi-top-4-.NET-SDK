using System;
using Microsoft.AspNetCore.Html;
using Microsoft.DotNet.Interactive.Formatting;
using PiTopMakerArchitecture.Foundation.Components;
using PiTopMakerArchitecture.Foundation.Sensors;

namespace PiTopMakerArchitecture.Foundation.InteractiveExtension
{
    public static class DeviceExtensions
    {
        internal static IHtmlContent DrawSvg(this Led led)
        {
            var id = "PiTopMakerArchitecture.Foundation.InteractiveExtension" + Guid.NewGuid().ToString("N");
            return PocketViewTags.div[id: id](
                PocketViewTags.svg(
                    PocketViewTags.g(
                        PocketViewTags.circle[cx: 20, cy: 20, r: 15, fill:(led.IsOn? "white":"black"), stroke: "black"],
                        PocketViewTags.text[x: 9, y: 50, @class: "text"]("LED")
                    )));
        }

        public static object GetDeviceValue(this AnaloguePortDeviceBase analogueDevice)
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