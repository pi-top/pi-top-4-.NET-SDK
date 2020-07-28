using System.Threading;
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
            // save to file
            // execute python to load and show on pitop display
        }
    }
}