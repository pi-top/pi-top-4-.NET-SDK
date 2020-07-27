using System;
using System.Device.Spi;

namespace PiTop.OledDevice
{
    public class Sh1106 : IDisposable
    {
        private readonly ISerialInterface _serialInterface;

        private enum Command
        {
            DISPLAYOFF = 0xAE,
            DISPLAYON = 0xAF,
            DISPLAYALLON = 0xA5,
            DISPLAYALLON_RESUME = 0xA4,
            NORMALDISPLAY = 0xA6,
            INVERTDISPLAY = 0xA7,
            SETREMAP = 0xA0,
            SETMULTIPLEX = 0xA8,
            SETCONTRAST = 0x81,
        }

        public Sh1106(SpiConnectionSettings connectionSettings, int dcPin, int rstPin, ISPiDeviceFactory sPiDeviceFactory, IGpioControllerFactory controllerFactory )
        {
            if (connectionSettings == null)
            {
                throw new ArgumentNullException(nameof(connectionSettings));
            }

            if (sPiDeviceFactory == null)
            {
                throw new ArgumentNullException(nameof(sPiDeviceFactory));
            }

            if (controllerFactory == null)
            {
                throw new ArgumentNullException(nameof(controllerFactory));
            }

            var spiDevice = sPiDeviceFactory.GetOrCreateSpiDevice(connectionSettings);
            var controller = controllerFactory.GetOrCreateController();

            _serialInterface = new SpiInterface(spiDevice, controller, dcPin, rstPin);
        }

        public Sh1106(int deviceAddress, II2CDeviceFactory i2CDeviceFactory)
        {
            if (i2CDeviceFactory == null)
            {
                throw new ArgumentNullException(nameof(i2CDeviceFactory));
            }
            var bus = i2CDeviceFactory.GetOrCreateI2CDevice(deviceAddress);
            _serialInterface = new I2cInterface(bus);
            
        }

        public static int Width => 0;
        public static int Height => 0;

        public void Show()
        {
            throw new NotImplementedException();
        }
        public void Dispose()
        {
            _serialInterface.Dispose();
        }

        public void Hide()
        {
            throw new NotImplementedException();
        }
    }
}
