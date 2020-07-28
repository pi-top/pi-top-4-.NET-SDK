using System;
using System.Reactive.Disposables;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace PiTop
{
    public abstract class Display : IDisposable
    {
        public int Width { get; }
        public int Height { get; }

        protected Image InternalBitmap => _image;

        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        private readonly Image<Rgba32> _image;

        protected Display(int width, int height)
        {
            Width = width;
            Height = height;
            ClearColor = Color.Black;
            _image = new Image<Rgba32>(Width,Height,ClearColor);
            RegisterForDisposal(_image);
        }

        protected Color ClearColor { get; set; }

        public Image Capture()
        {
            return _image.Clone();
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

        public void Draw(Action<IImageProcessingContext> drawingAction)
        {
            if (drawingAction == null)
            {
                throw new ArgumentNullException(nameof(drawingAction));
            }

            _image.Mutate(drawingAction);

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