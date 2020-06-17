using System.Drawing;

namespace PiTop.Camera
{
    public interface ICamera : IConnectedDevice
    {
        void GetFrame(out Bitmap frame);
    }
}