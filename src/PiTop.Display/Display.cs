using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Reactive.Disposables;
using PiTop.Luma;

namespace PiTop
{
    public class Display : IDisposable
    {
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        private readonly Sh1106 _device;
        private readonly  Bitmap _image;
        private int _width;
        private int _height;

        public Display(IGpioControllerFactory controllerFactory)
        {
          _device = new Sh1106(Sh1106.DefaultSpiConnectionSettings, controllerFactory);
          
          _image = new Bitmap(_width, _height, PixelFormat.Format16bppGrayScale);
          _disposables.Add(_image);
          _disposables.Add(_device);
        }

        public void Show()
        {
            _device.Show();
        }

        public void Hide()
        {
            _device.Hide();
        }

        public void Reset()
        {
            _device.Reset();
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
            _disposables.Dispose();
        }
    }
}