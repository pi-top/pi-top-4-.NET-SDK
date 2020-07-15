using System.Drawing;

namespace PiTop.Camera
{
    public interface ICamera : IConnectedDevice
    {
        Bitmap GetFrame();
    }
}