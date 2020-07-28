using SixLabors.ImageSharp;

namespace PiTop.Camera
{
    public interface ICamera :
        IConnectedDevice,
        IFrameSource<Image>
    {

    }

    public interface IFrameSource<out T>
    {
        T GetFrame();
    }
}