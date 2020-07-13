using System.Device.Gpio;

namespace PiTop
{
    public interface IGpioControllerFactory
    {
        IGpioController GetOrCreateController();
    }
}