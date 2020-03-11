using System;
using System.Device.I2c;

namespace PiTop
{
    public class PiTopModule: IDisposable
    {
        private const int I2CBusId = 1;

        public PiTopModule()
        {
        }

        public static I2cDevice CreateI2CDevice(int deviceAddress) => I2cDevice.Create(new I2cConnectionSettings(I2CBusId, deviceAddress));
        public void Dispose()
        {
            
        }
    }
}