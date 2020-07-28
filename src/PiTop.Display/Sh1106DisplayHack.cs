using System.IO;
using PiTop.OledDevice;
using SixLabors.ImageSharp;

namespace PiTop
{
    public class Sh1106DisplayHack : Display
    {
        private readonly Sh1106 _device;

        public Sh1106DisplayHack(DisplaySpiConnectionSettings settings, IGpioControllerFactory controllerFactory, ISPiDeviceFactory spiDeviceFactory) : base(Sh1106.Width, Sh1106.Height)
        {
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
            WriteViaPython();
            // save to file
            // execute python to load and show on pitop display
        }

        private void WriteViaPython()
        {
            var imgPath = "";
            var file = File.Create(imgPath);
            Capture().SaveAsBmp(file);

            // run code
        }
    }
}