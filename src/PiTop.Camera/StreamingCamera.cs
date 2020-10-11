using System.Diagnostics;

namespace PiTop.Camera
{
    /// <summary>
    /// Follow instructions from https://github.com/jacksonliam/mjpg-streamer to compile and install mjpg-streamer.
    /// When the camera is connected, the stream is also viewable externally on port 8080 (barring any firewalls).
    /// </summary>
    public class StreamingCamera : HttpSnapshotCamera
    {
        private Process? _process;
        private readonly int _cameraId;

        public StreamingCamera(int cameraId) : base(cameraId)
        {
            _cameraId = cameraId;
        }

        public override void Connect()
        {
            _process = Process.Start(new ProcessStartInfo("mjpg_streamer",
                $"-i \"input_uvc.so -d /dev/video{_cameraId}\" -o output_http.so"));
            base.Connect();
        }

        public override void Dispose()
        {
            base.Dispose();
            _process?.Dispose();
        }
    }
}