using Pocket;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Net.Http;

using static Pocket.Logger;

namespace PiTop.Camera
{
    /// <summary>
    /// Follow instructions from https://github.com/jacksonliam/mjpg-streamer to compile and install mjpg-streamer.
    /// When the camera is connected, the stream is also viewable externally on port 8080 (barring any firewalls).
    /// </summary>
    public class HttpSnapshotCamera :
        ICamera,
         IFrameSource<Image<Rgb24>>
    {
        private readonly HttpClient _client = new HttpClient();
        private readonly string _url;

        public HttpSnapshotCamera(int index) : this($"http://localhost:{8080 + index}/?action=snapshot") { }

        public HttpSnapshotCamera(string url)
        {
            _url = url;
        }

        public virtual void Connect()
        {
            // warm up the http client
            try
            {
                GetFrame();
            }
            catch
            {

            }
        }

        public Image GetFrame()
        {
            using var operation = Log.OnEnterAndExit();
            var image = _client.GetByteArrayAsync(_url).Result;
            using var _ = operation.OnEnterAndExit("Image.Load");
            return Image.Load(image);
        }

        Image<Rgb24> IFrameSource<Image<Rgb24>>.GetFrame()
        {
            return GetFrame().CloneAs<Rgb24>();
        }
        public virtual void Dispose()
        {
            _client?.Dispose();
        }
    }
}