using System.Device.I2c;

namespace PiTop
{
    public interface II2CDeviceFactory
    {
        I2cDevice GetCreateI2CDevice(int deviceAddress);
    }
}