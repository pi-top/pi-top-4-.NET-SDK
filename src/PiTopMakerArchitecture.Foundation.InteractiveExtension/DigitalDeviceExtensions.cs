using System;
using System.Linq;
using Microsoft.DotNet.Interactive.Formatting;
using PiTopMakerArchitecture.Foundation.Components;
using PiTopMakerArchitecture.Foundation.Sensors;
using static Microsoft.DotNet.Interactive.Formatting.PocketViewTags;
using static PiTopMakerArchitecture.Foundation.InteractiveExtension.SvgUtilities;

namespace PiTopMakerArchitecture.Foundation.InteractiveExtension
{
    internal static class DigitalDeviceExtensions
    {
        internal static PocketView GetSvg(this DigitalPortDeviceBase digitalDevice)
        {
            switch (digitalDevice)
            {
                case Buzzer buzzer:
                    return buzzer.GetSvg();
                case Led led:
                    return led.GetSvg();
                case Button button:
                    return button.GetSvg();
                case UltrasonicSensor ultrasonicSensor:
                    return ultrasonicSensor.GetSvg();
                default:
                    throw new ArgumentOutOfRangeException(nameof(digitalDevice));
            }
        }

        internal static PocketView GetSvg(this Buzzer buzzer)
        {
            throw new NotImplementedException();

        }

        internal static PocketView GetSvg(this Led led)
        {
            var isOn = led.IsOn;
            var color = led.DisplayProperties.OfType<CssColor>().FirstOrDefault();
            var cssColor = color?.Value ?? "rgb(42,236,14)";
            var filledStyle = isOn ? $"fill:{cssColor};" : "fill:rgba(0,0,0,0);";
            return g[transform: "matrix(1,0,0,1,-79.986,-8.17124e-14)"](g[transform: "matrix(1,0,0,1,-681.279,-120.438)"](
                g[transform: "matrix(0.0834819,0,0,0.0762288,750.236,112.727)"](
                    CreatePath("M623.239,203.06C623.239,154.054 586.908,114.267 542.16,114.267L225.172,114.267C180.423,114.267 144.093,154.054 144.093,203.06L144.093,550.209C144.093,599.216 180.423,639.003 225.172,639.003L542.16,639.003C586.908,639.003 623.239,599.216 623.239,550.209L623.239,203.06Z", "fill:white;stroke:black;stroke-width:25.02px;")),
                g[transform: "matrix(1,0,0,1,-6.29204,-1.53952)"](
                    _.circle[cx: 788.557, cy: 138.653, r: 8.673, style: "fill:rgb(235,235,235);fill-opacity:0;stroke:black;stroke-width:2px;"]()),
                g[transform: "matrix(0.689751,0,0,0.689751,238.357,41.4775)"](
                    _.circle[cx: 788.557, cy: 138.653, r: 8.673, style: filledStyle]()),
                g[transform: "matrix(1,0,0,1,5.38499,8.25384)"](
                    _.text[x: "768.748px", y: "149.76px", style: TextStyle]("LED"))
                ));

        }

        internal static PocketView GetSvg(this Button button)
        {
            var isPressed = button.IsPressed;
            var fillStyle = isPressed ? "fill:rgb(0,0,0);fill-opacity:0;stroke:black;stroke-width:2px;" : "fill:rgb(255,255,255);fill-opacity:0;stroke:black;stroke-width:2px;";
            return g[transform: "matrix(1,0,0,1,-760.994,-178.8)"]( g[transform: "matrix(1,0,0,1,-760.994,-178.8)"](
                g[transform: "matrix(0.0834819,0,0,0.0762288,749.965,171.09)"](
                    CreatePath("M623.239,203.06C623.239,154.054 586.908,114.267 542.16,114.267L225.172,114.267C180.423,114.267 144.093,154.054 144.093,203.06L144.093,550.209C144.093,599.216 180.423,639.003 225.172,639.003L542.16,639.003C586.908,639.003 623.239,599.216 623.239,550.209L623.239,203.06Z", "fill:white;stroke:black;stroke-width:25.02px;")),
                g[transform: "matrix(1,0,0,1,-3.08962,66.5187)"](
                    _.text[x: "768.748px", y: "149.76px"]("BUTTON")),
                g[transform: "matrix(1,0,0,1,-6.56329,56.823)"](
                    _.circle[cx: 788.557, cy: 138.653, r: 8.673, style: fillStyle]())
                ));

        }

        internal static PocketView GetSvg(this UltrasonicSensor ultrasonicSensor)
        {
            throw new NotImplementedException();

        }
    }
}