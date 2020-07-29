using System;
using System.Device.Spi;
using System.Security.Permissions;
using PiTop.Abstractions;

namespace PiTop.OledDevice
{
    public class Sh1106 : IDisposable
    {
        private readonly ISerialInterface _serialInterface;

        private class Command
        {
            public const byte DISPLAYOFF = 0xAE;
            public const byte DISPLAYON = 0xAF;
            public const byte DISPLAYALLON = 0xA5;
            public const byte DISPLAYALLON_RESUME = 0xA4;
            public const byte NORMALDISPLAY = 0xA6;
            public const byte INVERTDISPLAY = 0xA7;
            public const byte SETREMAP = 0xA0;
            public const byte SETMULTIPLEX = 0xA8;
            public const byte SETCONTRAST = 0x81;
            public const byte CHARGEPUMP = 0x8D;
            public const byte COLUMNADDR = 0x21;
            public const byte COMSCANDEC = 0xC8;
            public const byte COMSCANINC = 0xC0;
            public const byte EXTERNALVCC = 0x1;
            public const byte MEMORYMODE = 0x20;
            public const byte PAGEADDR = 0x22;
            public const byte SETCOMPINS = 0xDA;
            public const byte SETDISPLAYCLOCKDIV = 0xD5;
            public const byte SETDISPLAYOFFSET = 0xD3;
            public const byte SETHIGHCOLUMN = 0x10;
            public const byte SETLOWCOLUMN = 0x00;
            public const byte SETPRECHARGE = 0xD9;
            public const byte SETSEGMENTREMAP = 0xA1;
            public const byte SETSTARTLINE = 0x40;
            public const byte SETVCOMDETECT = 0xDB;
            public const byte SWITCHCAPVCC = 0x2;
        }

        public Sh1106(SpiConnectionSettings connectionSettings, int dcPin, int rstPin, ISPiDeviceFactory sPiDeviceFactory, IGpioControllerFactory controllerFactory)
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

        public static int Width => 128;
        public static int Height => 64;

        public void Init()
        {
            _serialInterface.Command(
                Command.DISPLAYOFF,
                Command.MEMORYMODE,
                Command.SETLOWCOLUMN,
                Command.SETHIGHCOLUMN, 0xB0, 0xC8,
                Command.SETLOWCOLUMN, 0x10, 0x40,
                Command.SETSEGMENTREMAP,
                Command.NORMALDISPLAY,
                Command.SETMULTIPLEX, 0x3F,
                Command.DISPLAYALLON_RESUME,
                Command.SETDISPLAYOFFSET, 0x00,
                Command.SETDISPLAYCLOCKDIV, 0xF0,
                Command.SETPRECHARGE, 0x22,
                Command.SETCOMPINS, 0x12,
                Command.SETVCOMDETECT, 0x20,
                Command.CHARGEPUMP, 0x14
            );
            SetContrast(0X7F);
            Show();
        }
        public void Show()
        {
            _serialInterface.Command(Command.DISPLAYON);

        }
        public void Dispose()
        {
            _serialInterface.Dispose();
        }

        public void SetContrast(byte contrastLevel)
        {
            _serialInterface.Command(Command.SETCONTRAST, contrastLevel);
        }

        public void Hide()
        {
            _serialInterface.Command(
              Command.DISPLAYOFF);
        }

        public void WritePage(int page, byte[] scan)
        {
            const byte setPageAddress = 0xB0;
            _serialInterface.Command(
                (byte)(setPageAddress | page), 0x02, 0x10
                );
            _serialInterface.Data(scan);
        }

    }
}
