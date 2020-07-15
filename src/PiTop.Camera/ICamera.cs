using System.Drawing;

namespace PiTop.Camera
{
    public interface ICamera : 
        IConnectedDevice, 
        IFrameSource<Bitmap>
    {
        
    }

    public interface IFrameSource<out T>
    {
        T GetFrame();
    }
}