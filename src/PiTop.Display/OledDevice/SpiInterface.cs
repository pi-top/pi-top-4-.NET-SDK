using System;
using System.Device.Gpio;
using System.Device.Spi;

namespace PiTop.OledDevice
{
    internal class SpiInterface : ISerialInterface
    {
        private readonly SpiDevice _device;
        private readonly GpioController _controller;
        private readonly int _dcPin;
        private readonly int _rstPin;

        public SpiInterface(SpiDevice device, GpioController controller, int dcPin, int rstPin)
        {
            _device = device ?? throw new ArgumentNullException(nameof(device));
            _controller = controller ?? throw new ArgumentNullException(nameof(controller));
            _dcPin = dcPin;
            _rstPin = rstPin;

            _controller.OpenPin(_dcPin, PinMode.Output);
            _controller.OpenPin(_rstPin, PinMode.Output);

            _controller.Write(_rstPin, PinValue.Low);
            _controller.Write(_rstPin, PinValue.High);
        }

        public void Dispose()
        {
            _controller.ClosePin(_dcPin);
            _controller.ClosePin(_rstPin);
        }

    }
}