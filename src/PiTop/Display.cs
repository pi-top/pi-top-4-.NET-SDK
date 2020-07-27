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
        public PixelFormat PixelFormat { get; }

        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        private readonly Bitmap _image;

        protected Display(int width, int height, PixelFormat pixelFormat)
        {
            Width = width;
            Height = height;
            PixelFormat = pixelFormat;
            ClearColor = Color.Black;

            _image = new Bitmap(Width, Height, PixelFormat);
            RegisterForDisposal(_image);
        }

        protected Color ClearColor { get; set; }

        protected void RegisterForDisposal(IDisposable disposable)
        {
            if (disposable == null) throw new ArgumentNullException(nameof(disposable));
            _disposables.Add(disposable);
        }

        protected internal void RegisterForDisposal(Action dispose)
        {
            if (dispose == null) throw new ArgumentNullException(nameof(dispose));
            _disposables.Add(Disposable.Create(dispose));
        }
        public abstract void Show();
        public abstract void Hide();

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

        protected abstract void CommitBuffer();

        public void Clear()
        {
            using var context = Graphics.FromImage(_image);
            context.Clear(ClearColor);
            context.Flush(FlushIntention.Flush);
            CommitBuffer();
        }
        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}