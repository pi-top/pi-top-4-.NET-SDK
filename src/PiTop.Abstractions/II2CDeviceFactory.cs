using System.Device.I2c;

namespace PiTop.Abstractions
{
    public interface II2CDeviceFactory
    {
        I2cDevice GetOrCreateI2CDevice(int deviceAddress);
    }
}