using System;
using System.Device.Spi;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using PiTop.OledDevice;

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
    public class Display : IDisposable
    {
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        private readonly Sh1106 _device;
        private readonly Bitmap _image;
        private int _width;
        private int _height;

        public Display(DisplaySpiConnectionSettings settings, IGpioControllerFactory controllerFactory, ISPiDeviceFactory spiDeviceFactory)
        {
            _device = new Sh1106(settings.SpiConnectionSettings, settings.DcPin, settings.RstPin, spiDeviceFactory, controllerFactory);

            _image = new Bitmap(_width, _height, PixelFormat.Format16bppGrayScale);
            _disposables.Add(_image);
            _disposables.Add(_device);
        }

        public void Show()
        {
            // grab device
            _device.Show();
        }

        public void Hide()
        {
            // give back to OS
            _device.Hide();
        }

        public void Draw(Action<Graphics> drawingAction)
        {
            if (drawingAction == null)
            {
                throw new ArgumentNullException(nameof(drawingAction));
            }
            using var graphics = Graphics.FromImage(_image);
            drawingAction(graphics);
            graphics.Flush(FlushIntention.Flush);

            CommitBuffer();
        }

        private void CommitBuffer()
        {
            // save to file
            // execute python to load and show on pitop display

            var imgPath = "/tmp/img_039482348294382934238423942384329483234923872349.png";
            Process.Start("python3", $"--show-image {imgPath}");
            throw new NotImplementedException();
        }

        public void Clear()
        {
            using var context = Graphics.FromImage(_image);
            context.Clear(Color.Black);
            context.Flush(FlushIntention.Flush);
            CommitBuffer();
        }


        public void Dispose()
        {
            // on dispose give control back to os
            _disposables.Dispose();
        }
    }
}