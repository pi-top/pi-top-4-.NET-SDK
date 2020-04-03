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
                PocketViewTags.svg(
                    PocketViewTags.g(
                        PocketViewTags.circle[cx: 20, cy: 20, r: 15, fill:(led.IsOn? "white":"black"), stroke: "black"],
                        PocketViewTags.text[x: 9, y: 50, @class: "text"]("LED")
                    )));
        }
    }
}