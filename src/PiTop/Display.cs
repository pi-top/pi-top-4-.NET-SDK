using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Reactive.Disposables;

namespace PiTop
{
    public abstract class Display : IDisposable
    {
        public int Width { get; }
        public int Height { get; }

        protected Bitmap InternalBitmap => _image;

        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        private readonly Bitmap _image;

        protected Display(int width, int height)
        {
            Width = width;
            Height = height;
            ClearColor = Color.Black;
            _image = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);
            RegisterForDisposal(_image);
        }

        protected Color ClearColor { get; set; }

        public Bitmap Capture()
        {
            return _image.Clone(new Rectangle(0, 0, _image.Width, _image.Height), _image.PixelFormat);
        }

        protected void RegisterForDisposal(IDisposable disposable)
        {
            if (disposable == null)
            {
                throw new ArgumentNullException(nameof(disposable));
            }
            _disposables.Add(disposable);
        }

        protected void RegisterForDisposal(Action dispose)
        {
            if (dispose == null)
            {
                throw new ArgumentNullException(nameof(dispose));
            }
            RegisterForDisposal(Disposable.Create(dispose));
        }
        public abstract void Show();
        public abstract void Hide();

        public void Draw(Action<Graphics> drawingAction)
        {
            if (drawingAction == null)
            {
                throw new ArgumentNullException(nameof(drawingAction));
            }

            using (var graphics = Graphics.FromImage(_image))
            {
                drawingAction(graphics);
                graphics.Flush(FlushIntention.Flush);
                graphics.Save();
            }

            CommitBuffer();
        }



        protected abstract void CommitBuffer();

        public void Clear(Color? clearColor = null)
        {
            Draw(context =>
            {
                context.Clear(clearColor ?? ClearColor);
            });
        }
        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}