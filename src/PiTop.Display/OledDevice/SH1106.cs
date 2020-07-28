using System;
using System.Device.Spi;
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
            public const byte DISPLAYALLOFF = 0xA4;
            public const byte NORMALDISPLAY = 0xA6;
            public const byte INVERTDISPLAY = 0xA7;
            public const byte SETREMAP = 0xA0;
            public const byte SETMULTIPLEX = 0xA8;
            public const byte SETCONTRAST = 0x81;
            public const byte SETHIGHCOLUMN = 0x10; // + high niblle
            public const byte SETLOWCOLUMN = 0x00; // + low nibble
            public const byte SETSEGMENTREMAP = 0xA1;
            public const byte SETCOMSCANASC = 0xC0; // scan lines in ascending order
            public const byte SETCOMSCANDESC = 0xC8; // scan lines in descending order  
            public const byte SETDISPLAYSTARTLINE = 0x40; // + start line number
            public const byte SETDISPLAYOFFSET = 0xD3;
            public const byte SETDISPLAYCLOCKDIV = 0xD5;
            public const byte SETPRECHARGE = 0xD9;
            public const byte SETCOMPINS = 0xDA;
            public const byte SETVCOMDESELECT = 0xDB;
            public const byte SETPAGEADDRESS = 0xB0; // + page number
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

        public static int Width => 0;
        public static int Height => 0;

        public void Show()
        {
            _serialInterface.Command(
                Command.DISPLAYOFF,
                Command.SETHIGHCOLUMN,
                Command.SETLOWCOLUMN,
                Command.SETDISPLAYSTARTLINE,
                Command.SETPAGEADDRESS,
                Command.SETCOMSCANDESC, // device dependent, 
                Command.SETSEGMENTREMAP, // columns from left to right
                Command.NORMALDISPLAY,
                Command.SETMULTIPLEX, 0,
                Command.DISPLAYALLOFF,
                Command.SETDISPLAYOFFSET, 0,
                Command.SETDISPLAYCLOCKDIV, 0xF0, // +50% Fosc
                Command.SETPRECHARGE, 0x22, // 2 DCLKs (POR)
                Command.SETCOMPINS, 0x12, // sequential COM order
                Command.SETVCOMDESELECT, 0x20 // .635 x Vref (sets contrast?)
            );
        }
        public void Dispose()
        {
            _serialInterface.Dispose();
        }

        public void Hide()
        {
            _serialInterface.Command(
              Command.DISPLAYOFF);
        }
    }
}
