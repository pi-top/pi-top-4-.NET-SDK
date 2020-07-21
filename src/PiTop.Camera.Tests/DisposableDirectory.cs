using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace PiTop.Camera.Tests
{
    public class DisposableDirectory : IDisposable
    {
        public DirectoryInfo Root { get; }

        public DisposableDirectory(DirectoryInfo directoryInfo)
        {
            Root = directoryInfo;
            if (!Root.Exists)
            {
                Root.Create();
                Root.Refresh();
            }
        }

        public void Dispose()
        {
            try
            {
                Root.Delete(true);
            }
            catch 
            {

            }
        }

        public static DisposableDirectory CreateTemp([CallerMemberName] string name = null)
        {
            var path = Path.Combine(Path.GetTempPath(),name, Path.GetFileNameWithoutExtension(Path.GetTempFileName()));
            var dir = new DisposableDirectory(new DirectoryInfo(path));
            
            var assembly = typeof(FileSystemCameraTests).Assembly;
            var index = 0;
            var finished = false;
            while (!finished)
            {
                try
                {
                    var file = $"image_{index:000}.png";
                    using (var resourceStream = assembly.GetManifestResourceStream($"{typeof(FileSystemCameraTests).Namespace}.Images.{file}"))
                    {
                        if (resourceStream != null)
                        {
                            var image = new byte[resourceStream.Length];
                            resourceStream.Read(image, 0, image.Length);
                            var filePath = Path.Combine(dir.Root.FullName,file);
                            File.WriteAllBytes(filePath, image);
                        }
                        else
                        {
                            finished = true;
                        }

                    }
                }
                catch
                {
                    finished = true;
                }
                index++;
            }
            return dir;
        }

    }
}