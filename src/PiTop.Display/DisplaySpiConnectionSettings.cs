using System.Device.Spi;

namespace PiTop
{
    public class DisplaySpiConnectionSettings
    {
        public SpiConnectionSettings SpiConnectionSettings { get; set; }
        public int RstPin { get; set; }
        public int DcPin { get; set; }

        public static DisplaySpiConnectionSettings Default => new DisplaySpiConnectionSettings
        {
            DcPin = 17,
            RstPin = 27,
            SpiConnectionSettings = new SpiConnectionSettings(0, 1)
            {
                ClockFrequency = 8000000,
                DataBitLength = 4096,
                Mode = SpiMode.Mode0,
                ChipSelectLineActiveState = 0
            }
        };
    }
}