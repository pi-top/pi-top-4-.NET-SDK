using System;
using Microsoft.AspNetCore.Html;
using Microsoft.DotNet.Interactive.Formatting;
using PiTopMakerArchitecture.Foundation.Components;

namespace PiTopMakerArchitecture.Foundation.InteractiveExtension
{
    internal static class DeviceExtensions
    {
        public static IHtmlContent DrawSvg(this Led led)
        {
            var id = "PiTopMakerArchitecture.Foundation.InteractiveExtension" + Guid.NewGuid().ToString("N");
            return PocketViewTags.div[id: id](
                PocketViewTags.svg[viewBox: "0 0 40 60"](
                    PocketViewTags.g(
                        PocketViewTags.circle[cx: 20, cy: 20, r: 15, fill:(led.IsOn? "black":"white"), style: "stroke: black, stroke-width:2"],
                        PocketViewTags.text[x: 0, y: 0, @class: "text"]("LED")
                    )));
        }
    }
}