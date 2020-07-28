using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Advanced;
using PiTop.Abstractions;
using PiTop.OledDevice;

namespace PiTop
{
    public class Sh1106Display : Display
    {
        private readonly Sh1106 _device;

        public Sh1106Display(DisplaySpiConnectionSettings settings, IGpioControllerFactory controllerFactory, ISPiDeviceFactory spiDeviceFactory) : base(Sh1106.Width, Sh1106.Height)
        {

            _device = new Sh1106(settings.SpiConnectionSettings, settings.DcPin, settings.RstPin, spiDeviceFactory, controllerFactory);
            AcquireDevice();

            RegisterForDisposal(ReleaseDevice);
            RegisterForDisposal(_device);
        }

        private void ReleaseDevice()
        {
            throw new System.NotImplementedException();
        }

        private void AcquireDevice()
        {
            throw new System.NotImplementedException();
        }

        public override void Show()
        {
            _device.Show();
        }

        public override void Hide()
        {
            _device.Hide();
        }

        protected override void CommitBuffer()
        {
            for (int page = 0; page < Height / 8; page++)
            {
                byte[] scan = new byte[Width]; // each byte represents 8 pixels in column

                for (int y = 0; y < 8; y++)
                {
                    var luminance = InternalBitmap.CloneAs<L8>().GetPixelRowMemory(y + page * 8).ToArray();
                    for (int x = 0; x < Width; x++)
                    {
                        if (y == 0) scan[x] = 0; // initialize on the first pixel row of the scan

                        if (luminance[x].PackedValue >= 128) scan[x] |= (byte)(0x80 >> y);
                    }
                }
                _device.WritePage(page, scan);
            }
        }
    }
}