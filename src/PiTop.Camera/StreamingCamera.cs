using System.Diagnostics;

using Pocket;

using static Pocket.Logger;

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
            using var _ = Log.OnEnterAndExit();
            var processStartInfo = new ProcessStartInfo("mjpg_streamer",
                $"-i \"input_uvc.so -d /dev/video{_cameraId} -r 1280x720\" -o output_http.so")
            {
                RedirectStandardError = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                UseShellExecute = false
            };

            _process = Process.Start(processStartInfo);

            _process.OutputDataReceived += (sender, args) =>
            {
                using var operation = Log.OnEnterAndExit("mjpg_streamer");
                operation.Info(args.Data);
            };

            _process.ErrorDataReceived += (sender, args) =>
            {
                using var operation = Log.OnEnterAndExit("mjpg_streamer");
                operation.Error(args.Data);
            };

            base.Connect();

        }

        public override void Dispose()
        {
            base.Dispose();
            _process?.Dispose();
        }
    }
}