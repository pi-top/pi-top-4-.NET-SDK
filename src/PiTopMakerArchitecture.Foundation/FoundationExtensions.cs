using PiTop;

using PiTopMakerArchitecture.Foundation.Components;
using PiTopMakerArchitecture.Foundation.Sensors;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace PiTopMakerArchitecture.Foundation
{
    public static class FoundationExtensions
    {
        public static FoundationPlate GetOrCreateFoundationPlate(this PiTop4Board module)
        {
            return module.GetOrCreatePlate<FoundationPlate>();
        }

        public static SoundSensor GetOrCreateSoundSensor(this FoundationPlate plate, AnaloguePort port)
        {
            return plate.GetOrCreateDevice<SoundSensor>(port);
        }

        public static LightSensor GetOrCreateLightSensor(this FoundationPlate plate, AnaloguePort port)
        {
            return plate.GetOrCreateDevice<LightSensor>(port);
        }

        public static Potentiometer GetOrCreatePotentiometer(this FoundationPlate plate, AnaloguePort port)
        {
            return plate.GetOrCreateDevice<Potentiometer>(port);
        }

        public static Button GetOrCreateButton(this FoundationPlate plate, DigitalPort port)
        {
            return plate.GetOrCreateDevice<Button>(port);
        }

        public static Buzzer GetOrCreateBuzzer(this FoundationPlate plate, DigitalPort port)
        {
            return plate.GetOrCreateDevice<Buzzer>(port);
        }

        public static UltrasonicSensor GetOrCreateUltrasonicSensor(this FoundationPlate plate, DigitalPort port)
        {
            return plate.GetOrCreateDevice<UltrasonicSensor>(port);
        }

        public static Led GetOrCreateLed(this FoundationPlate plate, DigitalPort port)
        {
            return plate.GetOrCreateDevice<Led>(port);
        }

        public static Led GetOrCreateLed(this FoundationPlate plate, DigitalPort port, Color displayColor)
        {
            var led = plate.GetOrCreateDevice<Led>(port);
            var p = displayColor.ToPixel<Argb32>();
            var alpha = p.A / 255.0;
            led.DisplayProperties.Add(new RgbaCssColor(p.R, p.G, p.B, alpha));
            return led;
        }
    }
}