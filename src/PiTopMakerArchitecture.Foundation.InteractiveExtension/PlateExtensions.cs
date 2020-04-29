using System;
using Microsoft.AspNetCore.Html;
using Microsoft.DotNet.Interactive.Formatting;
using Newtonsoft.Json.Linq;

namespace PiTopMakerArchitecture.Foundation.InteractiveExtension
{
    public static class PlateExtensions
    {
        public static JObject ToJObject(this Plate plate)
        {
            var root = new JObject();

            foreach (var digital in plate.DigitalDevices)
            {
                root[digital.port.ToString()] = new JObject
                {
                    {"type", digital.device.GetType().Name },
                    {"value", JToken.FromObject( digital.device.GetDeviceValue()) },
                };
            }

            foreach (var analogue in plate.AnalogueDevices)
            {
                root[analogue.port.ToString()] = new JObject
                {
                    {"type", analogue.device.GetType().Name },
                    {"value", JToken.FromObject( analogue.device.GetDeviceValue() )},
                };
            }

            return root;
        }

        internal static IHtmlContent DrawSvg(this Plate plate)
        {
            var id = "PiTopMakerArchitecture.Foundation.InteractiveExtension" + Guid.NewGuid().ToString("N");
            return PocketViewTags.div[id: id](
                PocketViewTags.svg(
                    PocketViewTags.g(
                        plate.GetPlateSvg(),
                        plate.GetWiresSvg(),
                        plate.GetDevicesSvg()
                    )
                )
                );
        }

        internal static PocketView GetWiresSvg(this Plate plate)
        {
            return PocketViewTags.g[@class: "wires_group"](

            );
        }

        internal static PocketView GetDevicesSvg(this Plate plate)
        {
            return PocketViewTags.g[@class: "devices_group"](

                );
        }

        internal static PocketView GetPlateSvg(this Plate plate)
        {
            return PocketViewTags.g[@class: "devices_group"](

            );
        }

    }
}