using Pocket;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Diagnostics;
using System.Net.Http;

using static Pocket.Logger;

namespace PiTop.Camera
{
    /// <summary>
    /// Follow instructions from https://github.com/jacksonliam/mjpg-streamer to compile and install mjpg-streamer.
    /// When the camera is connected, the stream is also viewable externally on port 8080 (barring any firewalls).
    /// </summary>
    public class StreamingCamera :
         ICamera,
         IFrameSource<Image<Rgb24>>,
        IDisposable
    {
        private Process? _process;
        private HttpClient _client;
        private readonly int _cameraId;

        public StreamingCamera(int cameraId)
        {
            _cameraId = cameraId;
        }

        public void Connect()
        {
            _client = new HttpClient();
            _process = Process.Start(new ProcessStartInfo("mjpg_streamer",
                $"-i \"input_uvc.so -d /dev/video{_cameraId}\" -o output_http.so"));
        }
        public void Dispose()
        {
            _process?.Dispose();
            _client?.Dispose();
        }

        public Image GetFrame()
        {
            using var operation = Log.OnEnterAndExit();
            var image = _client.GetByteArrayAsync("http://localhost:8080/?action=snapshot").Result;
            using var _ = operation.OnEnterAndExit("Image.Load");
            return Image.Load(image);
        }

        Image<Rgb24> IFrameSource<Image<Rgb24>>.GetFrame()
        {
            return GetFrame().CloneAs<Rgb24>();
        }
    }
}