using System.Device.Gpio;

namespace PiTop.Abstractions
{
    public interface IGpioControllerFactory
    {
        GpioController GetOrCreateController();
    }
}