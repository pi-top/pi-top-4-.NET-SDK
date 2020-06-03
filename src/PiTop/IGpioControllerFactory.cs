using System.Device.Gpio;

namespace PiTop
{
    public interface IGpioControllerFactory
    {
        GpioController GetOrCreateController();
    }
}