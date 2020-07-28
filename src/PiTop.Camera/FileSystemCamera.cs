using System;
using System.IO;
using System.Linq;

using SixLabors.ImageSharp;

namespace PiTop.Camera
{
    public class FileSystemCameraSettings
    {
        public FileSystemCameraSettings(DirectoryInfo imageLocation, string imageSearchPattern)
        {
            if (string.IsNullOrWhiteSpace(imageSearchPattern))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(imageSearchPattern));
            }
            ImageLocation = imageLocation ?? throw new ArgumentNullException(nameof(imageLocation));
            ImageSearchPattern = imageSearchPattern;
        }

        public DirectoryInfo ImageLocation { get; }
        public string ImageSearchPattern { get; }

    }

    public class FileSystemCamera : ICamera
    {
        private readonly DirectoryInfo _imageLocation;
        private readonly string _imageSearchPattern;
        private FileInfo[] _images = Array.Empty<FileInfo>();
        private int _currentIndex;
        private Image? _currentImage;

        public FileSystemCamera(FileSystemCameraSettings settings)
        {
            if (settings == null) throw new ArgumentNullException(nameof(settings));
            _imageLocation = settings.ImageLocation;
            _imageSearchPattern = settings.ImageSearchPattern;
        }

        public object FrameCount => _images.Length;

        public void Dispose()
        {

        }

        public void Connect()
        {
            _imageLocation.Refresh();
            if (!_imageLocation.Exists)
            {
                throw new DirectoryNotFoundException($"Cannot open {_imageLocation.FullName}");
            }

            _images = _imageLocation.GetFiles(_imageSearchPattern).OrderBy(f => f.Name).ToArray();
            _currentIndex = 0;
            LoadFrame(0);
        }

        public void Reset()
        {
            LoadFrame(0);
        }

        public FileInfo CurrentFrameSource => _images[_currentIndex];

        private void LoadFrame(int index)
        {
            if (index >= 0 && index < _images.Length)
            {
                _currentImage = Image.Load(_images[_currentIndex].FullName);
            }
        }

        public void Advance()
        {
            var index = Math.Min(_currentIndex + 1, _images.Length - 1);

            if (index == _currentIndex)
            {
                return;
            }

            _currentIndex = index;
            LoadFrame(_currentIndex);
        }

        public Image GetFrame()
        {
            return _currentImage ??= Image.Load(_images[_currentIndex].FullName);
        }
    }
}