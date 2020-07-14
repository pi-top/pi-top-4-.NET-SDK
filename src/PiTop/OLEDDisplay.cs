using System;
using System.Device.Gpio;
using System.Device.Spi;

namespace PiTop
{
    internal class OLEDDisplay : IDisposable
    {
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


        private readonly GpioController _controller;
        private readonly SpiDevice _device;
        private const int spi_port = 1;
        private const int spi_device = 0;
        private const int spi_bus_speed_hz = 8000000;
        private const int spi_cs_high = 0;
        private const int spi_transfer_size = 4096;
        private const int gpio_DC_pin = 17;
        private const int gpio_RST_pin = 27;
        private const int gpio_command_mode = 0;
        private const int gpio_data_mode = 1;

        public OLEDDisplay(IGpioControllerFactory controllerFactory)
        {
            var connectionSettings = new SpiConnectionSettings(spi_device, spi_port)
            {
                ClockFrequency = spi_bus_speed_hz,
                DataBitLength = spi_transfer_size,
                Mode = SpiMode.Mode0,
                ChipSelectLineActiveState = spi_cs_high
            };

            _device = SpiDevice.Create(connectionSettings);
            _controller = controllerFactory.GetOrCreateController();

            _controller.OpenPin(gpio_DC_pin, PinMode.Output);
            _controller.OpenPin(gpio_RST_pin, PinMode.Output);

            Reset();
        }

        public void Reset()
        {
            _controller.Write(gpio_RST_pin, 0);
            _controller.Write(gpio_RST_pin, 1);
        }

        public void Show()
        {
            SendCommand(Command.DISPLAYALLON);
        }

        private void SendCommand(Command command)
        {
            _controller.Write(gpio_DC_pin, gpio_command_mode);
            var cmdValue = (byte)command;
            for (var i = 0; i < 8; i++)
            {
                //_controller.Write(self._SDA, cmdValue & 0x80);
                //_controller.Write(self._SCLK, 1);
                //cmdValue <<= 1;
                //_controller.Write(self._SCLK, 0);
            }

            throw new NotImplementedException();
        }


        public void Dispose()
        {
            _device.Dispose();
        }

        public void Hide()
        {
            throw new NotImplementedException();
        }
    }
}