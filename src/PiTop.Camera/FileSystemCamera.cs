using System;
using System.Drawing;
using System.IO;
using System.Linq;

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

    public class FileSystemCamera: ICamera
    {
        private readonly DirectoryInfo _imageLocation;
        private readonly string _imageSearchPattern;
        private FileSystemInfo[] _images;
        private int _currentIndex;
        private Bitmap _currentFrame;

        public FileSystemCamera(FileSystemCameraSettings settings)
        {
            if (settings == null) throw new ArgumentNullException(nameof(settings));
            _imageLocation = settings.ImageLocation;
            _imageSearchPattern = settings.ImageSearchPattern;
        }
        public void Dispose()
        {
            _currentFrame?.Dispose();   
        }

        public void Connect()
        {
            if (!_imageLocation.Exists)
            {
                throw new DirectoryNotFoundException($"Cannot open {_imageLocation.FullName}");
            }

            _images = _imageLocation.GetFileSystemInfos(_imageSearchPattern).OrderBy(f => f.Name).ToArray();
            _currentIndex = 0;
        }

        public void Reset()
        {
            LoadFrame(0);
        }

        private void LoadFrame(int index)
        {
            _currentFrame.Dispose();
            _currentFrame = new Bitmap(_images[index].FullName);
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

        public Bitmap GetFrame()
        {
          return _currentFrame??= new Bitmap(_images[_currentIndex].FullName);
        }
    }
}